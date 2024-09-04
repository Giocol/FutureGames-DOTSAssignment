using Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Entities {
    public class SpawnerAuthoring : MonoBehaviour
    {
        public GameObject prefab;
        public float spawnRate;
        public float2 xSpawnRange;
        public float2 ySpawnRange;
        public float2 zSpawnRange;

        public class SpawnerBaker : Baker<SpawnerAuthoring> {
            public override void Bake(SpawnerAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Spawner {
                    prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
                    nextSpawnTime = 0.0f,
                    spawnRate = authoring.spawnRate,
                    xSpawnRange = authoring.xSpawnRange,
                    ySpawnRange = authoring.ySpawnRange,
                    zSpawnRange = authoring.zSpawnRange
                });
            }
        }
    }
}
