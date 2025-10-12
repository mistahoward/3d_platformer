using KBCore.Refs;
using UnityEngine;
using Unity.Cinemachine;

namespace Platformer.Input {
	public class PlayerController : ValidatedMonoBehaviour {
		[Header("References")]
		[SerializeField, Self] private CharacterController _controller;
		[SerializeField, Self] private Animator _animator;
		[SerializeField, Anywhere] private CinemachineCamera _freeLookVCam;
		[SerializeField, Anywhere] private InputReader _inputReader;

		[Header("Settings")]
		[SerializeField] private float _moveSpeed = 6f;
		[SerializeField] private float _rotationSpeed = 15f;
		[SerializeField] private float _smoothTime = 0.2f;

		private Transform _mainCamera;

		private float _currentSpeed;
		private float _velocity;

		public static int Speed = Animator.StringToHash("Speed");

		private void Awake() {
			_mainCamera = Camera.main.transform;
			_freeLookVCam.Follow = transform;
			_freeLookVCam.LookAt = transform;
			_freeLookVCam.OnTargetObjectWarped(
				transform,
				transform.position - _freeLookVCam.transform.position - Vector3.forward
			);
		}
		private void Start() =>
			_inputReader.EnablePlayerActions();
		private void Update() {
			HandleMovement();
			UpdateAnimator();

		}

		private void UpdateAnimator() {
			_animator.SetFloat(Speed, _currentSpeed);
		}

		private void HandleMovement() {
			var movementDirection = new Vector3(_inputReader.Direction.x, 0, _inputReader.Direction.y);
			Vector3 adjustedDirection = Quaternion.AngleAxis(_mainCamera.eulerAngles.y, Vector3.up) * movementDirection;
			if (adjustedDirection.magnitude > 0f) {
				HandleRotation(adjustedDirection);

				HandleCharacterController(adjustedDirection);

				SmoothSpeed(adjustedDirection.magnitude);
			} else {
				SmoothSpeed(0f);
			}
		}

		private void HandleCharacterController(Vector3 adjustedDirection) {
			// move the player
			Vector3 adjustedMovement = _moveSpeed * Time.deltaTime * adjustedDirection;
			_controller.Move(adjustedMovement);
		}

		private void HandleRotation(Vector3 adjustedDirection) {
			// adjust rotation to match movement direction
			var targetRotation = Quaternion.LookRotation(adjustedDirection);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
			transform.LookAt(transform.position + adjustedDirection);
		}

		private void SmoothSpeed(float value) =>
			_currentSpeed = Mathf.SmoothDamp(_currentSpeed, value, ref _velocity, _smoothTime);
	}
}
