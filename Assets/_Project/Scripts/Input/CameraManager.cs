using System;
using System.Collections;
using KBCore.Refs;
using Unity.Cinemachine;
using UnityEngine;

namespace Platformer.Input {
	public class CameraManager : ValidatedMonoBehaviour {
		[SerializeField, Anywhere] private InputReader _inputReader;
		[SerializeField, Anywhere] private CinemachineCamera _freeLookVCam;

		[Header("Settings")]
		[SerializeField, Range(0.5f, 3f)] private float _speedMultiplier = 1f;

		private bool _isRMBPressed;
		private bool _cameraMovementLock;
		private CinemachineOrbitalFollow _orbitalFollow;

		private void Awake() {
			_orbitalFollow = _freeLookVCam.GetComponent<CinemachineOrbitalFollow>();
		}

		private void OnEnable() {
			_inputReader.Look += OnLook;
			_inputReader.EnableMouseControlCamera += OnEnableMouseControlCamera;
			_inputReader.DisableMouseControlCamera += OnDisableMouseControlCamera;
		}
		private void OnDisable() {
			_inputReader.Look -= OnLook;
			_inputReader.EnableMouseControlCamera -= OnEnableMouseControlCamera;
			_inputReader.DisableMouseControlCamera -= OnDisableMouseControlCamera;
		}


		private void OnEnableMouseControlCamera() {
			_isRMBPressed = true;

			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;

			StartCoroutine(DisableMouseForFrame());
		}
		private void OnDisableMouseControlCamera() {
			_isRMBPressed = false;

			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			// reset camera
			if (_orbitalFollow != null) {
				_orbitalFollow.HorizontalAxis.Value = 0f;
				_orbitalFollow.VerticalAxis.Value = 0f;
			}
		}

		private IEnumerator DisableMouseForFrame() {
			_cameraMovementLock = true;
			yield return new WaitForEndOfFrame();
			_cameraMovementLock = false;
		}

		private void OnLook(Vector2 cameraMovement, bool isDeviceMouse) {
			if (_cameraMovementLock) return;
			if (isDeviceMouse && !_isRMBPressed) return;

			if (_orbitalFollow == null) return;

			float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;

			_orbitalFollow.HorizontalAxis.Value += cameraMovement.x * _speedMultiplier * deviceMultiplier;
			_orbitalFollow.VerticalAxis.Value += cameraMovement.y * _speedMultiplier * deviceMultiplier;
		}
	}
}
