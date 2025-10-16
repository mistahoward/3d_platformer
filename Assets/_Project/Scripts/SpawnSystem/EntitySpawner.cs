using UnityEngine;

namespace Platformer {
	public class EntitySpawner<T> where T : Entity {
		private readonly IEntityFactory<T> _entityFactory;
		private readonly ISpawnPointStrategy _spawnPointStrategy;

		public EntitySpawner(IEntityFactory<T> entityFactory, ISpawnPointStrategy spawnPointStrategy) {
			_entityFactory = entityFactory;
			_spawnPointStrategy = spawnPointStrategy;
		}

		public T Spawn() {
			Transform spawnPoint = _spawnPointStrategy.NextSpawnPoint();
			return _entityFactory.Create(spawnPoint);
		}
	}
}
