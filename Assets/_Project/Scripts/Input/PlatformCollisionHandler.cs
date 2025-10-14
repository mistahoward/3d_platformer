using UnityEngine;

namespace Platformer.Input {
	public class PlatformCollisionHandler : MonoBehaviour {
		private Transform _platform; // the platform if any, we are on top of

		private void OnCollisionEnter(Collision collision) {
			if (collision.gameObject.CompareTag("Moving Platform")) {
				// only if we landed from above
				ContactPoint contact = collision.GetContact(0);
				if (contact.normal.y < 0.5f) return;

				_platform = collision.transform;
				transform.SetParent(_platform);
			}
		}
		private void OnCollisionExit(Collision collision) {
			if (collision.gameObject.CompareTag("Moving Platform")) {
				transform.SetParent(null);
				_platform = null;
			}
		}
	}
}
