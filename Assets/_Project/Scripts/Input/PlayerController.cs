using KBCore.Refs;
using UnityEngine;
using Unity.Cinemachine;

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

		private float _currentSpeed;
		private float _velocity;
		private Vector3 _playerMovement;
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
		}
		private void Start() =>
			_inputReader.EnablePlayerActions();
		private void Update() {
			_playerMovement = new Vector3(_inputReader.Direction.x, 0f, _inputReader.Direction.y);
			HandleMovement();
			UpdateAnimator();
		}

		private void FixedUpdate() {
			HandleMovement();
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
