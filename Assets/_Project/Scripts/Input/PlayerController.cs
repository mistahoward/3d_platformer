using KBCore.Refs;
using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;
using System;

namespace Platformer.Input {
	public partial class PlayerController : ValidatedMonoBehaviour {
		[Header("References")]
		[SerializeField, Self] private Rigidbody _rigidBody;
		[SerializeField, Self] private GroundChecker _groundChecker;
		[SerializeField, Self] private Animator _animator;
		[SerializeField, Anywhere] private CinemachineCamera _freeLookVCam;
		[SerializeField, Anywhere] private InputReader _inputReader;

		[Header("Settings")]
		[SerializeField] private float _moveSpeed = 6f;
		[SerializeField] private float _rotationSpeed = 15f;
		[SerializeField] private float _smoothTime = 0.2f;
		private Transform _mainCamera;

		[Header("Jump Settings")]
		[SerializeField] private float _jumpForce = 10f;
		[SerializeField] private float _jumpDuration = 0.5f;
		[SerializeField] private float _jumpCoolDown = 0f;
		[SerializeField] private float _jumpMaxHeight = 2f;
		[SerializeField] private float _gravityMultiplier = 3f;
		private float _currentSpeed;
		private float _velocity;
		private float _jumpVelocity;
		private Vector3 _playerMovement;
		private List<Timer> _timers;
		private CountdownTimer _jumpTimer;
		private CountdownTimer _jumpCoolDownTimer;
		public static int Speed = Animator.StringToHash("Speed");

		private void Awake() {
			_mainCamera = Camera.main.transform;
			_freeLookVCam.Follow = transform;
			_freeLookVCam.LookAt = transform;
			_freeLookVCam.OnTargetObjectWarped(
				transform,
				transform.position - _freeLookVCam.transform.position - Vector3.forward
			);

			_rigidBody.freezeRotation = true;

			_jumpTimer = new CountdownTimer(_jumpDuration);
			_jumpCoolDownTimer = new CountdownTimer(_jumpCoolDown);
			_timers = new List<Timer>(2) { _jumpTimer, _jumpCoolDownTimer };

			_jumpTimer.OnTimerStop += () => _jumpCoolDownTimer.Start();
		}
		private void Start() =>
			_inputReader.EnablePlayerActions();

		private void OnEnable() =>
			_inputReader.Jump += OnJump;

		private void OnDisable() =>
			_inputReader.Jump -= OnJump;
		private void Update() {
			_playerMovement = new Vector3(_inputReader.Direction.x, 0f, _inputReader.Direction.y);

			HandleTimers();
			UpdateAnimator();
		}

		private void HandleTimers() {
			_timers.ForEach(t => t.Tick(Time.deltaTime));
		}

		private void FixedUpdate() {
			HandleJump();
			HandleMovement();
		}

		private void HandleJump() {
			if (!_jumpTimer.IsRunning && _groundChecker.IsGrounded) {
				_jumpVelocity = 0f;
				_jumpTimer.Stop();
				return;
			}
			if (_jumpTimer.IsRunning) {
				// progress point for initial burst of velocity
				float launchPoint = 0.9f;
				if (_jumpTimer.Progress > launchPoint)
					_jumpVelocity = Mathf.Sqrt(2 * _jumpMaxHeight * Math.Abs(Physics.gravity.y));
				else
					_jumpVelocity += (1 - _jumpTimer.Progress) * _jumpForce * Time.fixedDeltaTime;
			} else
				_jumpVelocity += _gravityMultiplier * Physics.gravity.y * Time.fixedDeltaTime;

			_rigidBody.linearVelocity = new Vector3(_rigidBody.linearVelocity.x, _jumpVelocity, _rigidBody.linearVelocity.z);
		}

		private void OnJump(bool isJumping) {
			if (isJumping && !_jumpTimer.IsRunning && !_jumpCoolDownTimer.IsRunning && _groundChecker.IsGrounded) {
				_jumpTimer.Start();
			} else if (!isJumping && _jumpTimer.IsRunning) {
				_jumpTimer.Stop();
			}
		}

		private void UpdateAnimator() {
			_animator.SetFloat(Speed, _currentSpeed);
		}

		private void HandleMovement() {
			Vector3 adjustedDirection = Quaternion.AngleAxis(_mainCamera.eulerAngles.y, Vector3.up) * _playerMovement;
			if (adjustedDirection.magnitude > 0f) {
				HandleRotation(adjustedDirection);

				HandleHorizontalMovement(adjustedDirection);

				SmoothSpeed(adjustedDirection.magnitude);
			} else {
				SmoothSpeed(0f);

				// reset for a snappy stop
				_rigidBody.linearVelocity = new Vector3(0f, _rigidBody.linearVelocity.y, 0f);
			}
		}

		private void HandleHorizontalMovement(Vector3 adjustedDirection) {
			// move the player
			Vector3 velocity = _moveSpeed * Time.deltaTime * adjustedDirection;
			_rigidBody.linearVelocity = new Vector3(velocity.x, _rigidBody.linearVelocity.y, velocity.z);
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
