using UnityEngine;

namespace Platformer {
	public class EntityFactory<T> : IEntityFactory<T> where T : Entity, new() {
		private EntityData[] _data;

		public EntityFactory(EntityData[] data) {
			_data = data;
		}
		public T Create(Transform spawnPoint) {
			EntityData entityData = _data[Random.Range(0, _data.Length)];
			GameObject entity = Object.Instantiate(entityData.PreFab, spawnPoint.position, Quaternion.identity);
			return entity.GetComponent<T>();
		}
	}
}
