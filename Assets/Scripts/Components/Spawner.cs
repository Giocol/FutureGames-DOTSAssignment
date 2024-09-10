using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Components
{
    public struct Spawner : IComponentData
    {
        public Entity prefab;
        public float2 xSpawnRange;
        public float2 ySpawnRange;
        public float2 zSpawnRange;
        public float nextSpawnTime;
        public float spawnRate;
    }
}
