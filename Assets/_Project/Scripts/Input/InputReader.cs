using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Platformer.Input {
	[CreateAssetMenu(fileName = "InputReader", menuName = "Platformer/InputReader")]
	public class InputReader : ScriptableObject, InputSystem_Actions.IPlayerActions {
		public event UnityAction<Vector2> Move = delegate { };
		public event UnityAction<Vector2, bool> Look = delegate { };
		public event UnityAction EnableMouseControlCamera = delegate { };
		public event UnityAction DisableMouseControlCamera = delegate { };
		public event UnityAction<bool> Jump = delegate { };
		private InputSystem_Actions _inputActions;

		public Vector3 Direction => _inputActions.Player.Move.ReadValue<Vector2>();

		private void OnEnable() {
			if (_inputActions is null) {
				_inputActions = new InputSystem_Actions();
				_inputActions.Player.SetCallbacks(this);
			}
		}

		public void EnablePlayerActions() {
			_inputActions.Player.Enable();
		}

		private void OnDisable() {
			_inputActions?.Player.Disable();
		}

		private void OnDestroy() {
			_inputActions?.Player.Disable();
			_inputActions?.Dispose();
		}

		public void OnAttack(InputAction.CallbackContext context) {
			// noop
		}

		public void OnCrouch(InputAction.CallbackContext context) {
			// noop
		}

		public void OnInteract(InputAction.CallbackContext context) {
			// noop
		}

		public void OnJump(InputAction.CallbackContext context) {
			switch (context.phase) {
				case InputActionPhase.Started:
					Jump.Invoke(true);
					break;
				case InputActionPhase.Canceled:
					Jump.Invoke(false);
					break;
			}
		}

		public void OnLook(InputAction.CallbackContext context) {
			Look.Invoke(context.ReadValue<Vector2>(), IsDeviceMouse(context));
		}

		private bool IsDeviceMouse(InputAction.CallbackContext context) =>
			context.control.device is Mouse;

		public void OnMove(InputAction.CallbackContext context) {
			Move.Invoke(context.ReadValue<Vector2>());
		}

		public void OnNext(InputAction.CallbackContext context) {
			// noop
		}

		public void OnPrevious(InputAction.CallbackContext context) {
			// noop
		}

		public void OnSprint(InputAction.CallbackContext context) {
			// noop
		}

		public void OnMouseControlCamera(InputAction.CallbackContext context) {
			switch (context.phase) {
				case InputActionPhase.Started:
					EnableMouseControlCamera.Invoke();
					break;
				case InputActionPhase.Canceled:
					DisableMouseControlCamera.Invoke();
					break;
			}
		}
	}
}
