using UnityEngine;

namespace Platformer {
	public class CollectibleSpawnManager : EntitySpawnManager {
		[SerializeField] private CollectibleData[] _collectibleData;
		[SerializeField] private float _spawnInterval = 1f;

		private CountdownTimer _spawnTimer;
		private EntitySpawner<Collectible> _spawner;

		private int _counter;
		protected override void Awake() {
			base.Awake();

			IEntityFactory<Collectible> entityFactory = new EntityFactory<Collectible>(_collectibleData);
			_spawner = new EntitySpawner<Collectible>(entityFactory, _spawnPointStrategy);

			_spawnTimer = new CountdownTimer(_spawnInterval);
			_spawnTimer.OnTimerStop += () => {
				if (_counter++ >= _spawnPoints.Length) {
					_spawnTimer.Stop();
					return;
				}
				Spawn();
				_spawnTimer.Start();
			};
		}

		private void Start() => _spawnTimer.Start();
		private void Update() => _spawnTimer.Tick(Time.deltaTime);
		public override void Spawn() => _spawner.Spawn();
	}
}
