using UnityEngine;

namespace Platformer {
	[CreateAssetMenu(fileName = "CollectibleData", menuName = "Platformer/CollectibleData")]
	public class CollectibleData : EntityData {
		public int Score;
		// additional props specific to collectibles
	}
}
