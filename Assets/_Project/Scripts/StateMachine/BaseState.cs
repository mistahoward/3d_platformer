using Platformer.Input;
using UnityEngine;

namespace Platformer {
	public abstract class BaseState : IState {
		protected readonly PlayerController _player;
		protected readonly Animator _animator;

		protected readonly int _locomotionHash = Animator.StringToHash("Locomotion");
		protected readonly int _jumpHash = Animator.StringToHash("Jump");
		protected readonly int _dashHash = Animator.StringToHash("Dash");
		protected const float _crossFadeDuration = 0.1f;

		protected BaseState(PlayerController player, Animator animator) {
			_player = player;
			_animator = animator;
		}

		public virtual void FixedUpdate() {
		}

		public virtual void OnEnter() {
			Debug.Log($"Entered {GetType().Name}");
		}

		public virtual void OnExit() {
			Debug.Log($"Exited {GetType().Name}");
		}

		public virtual void Update() {
		}
	}
}
