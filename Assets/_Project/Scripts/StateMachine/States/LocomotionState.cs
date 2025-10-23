using Platformer.Input;
using UnityEngine;

namespace Platformer {
	public class LocomotionState : BaseState {
		public LocomotionState(PlayerController player, Animator animator) : base(player, animator) {
		}
		public override void OnEnter() {
			_animator.CrossFade(_locomotionHash, _crossFadeDuration);
		}

		public override void FixedUpdate() {
			_player.HandleMovement();
		}
	}
}
