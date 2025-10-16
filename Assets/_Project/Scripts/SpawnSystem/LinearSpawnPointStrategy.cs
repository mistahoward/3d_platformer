using UnityEngine;

namespace Platformer {
	public class LinearSpawnPointStrategy : ISpawnPointStrategy {
		private int _currentIndex = 0;

		private readonly Transform[] _spawnPoints;
		public LinearSpawnPointStrategy(Transform[] spawnPoints) {
			_spawnPoints = spawnPoints;
		}

		public Transform NextSpawnPoint() {
			Transform result = _spawnPoints[_currentIndex];
			_currentIndex = (_currentIndex + 1) % _spawnPoints.Length;
			return result;
		}
	}
}
