using Components;
using Unity.Entities;
using UnityEngine;

namespace Entities {
    public class SpawnerAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        public float spawnRate;

        public class SpawnerBaker : Baker<SpawnerAuthoring> {
            public override void Bake(SpawnerAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Spawner {
                    prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
                    spawnPosition = authoring.transform.position,
                    nextSpawnTime = 0.0f,
                    spawnRate = authoring.spawnRate
                });
            }
        }
    }
}
