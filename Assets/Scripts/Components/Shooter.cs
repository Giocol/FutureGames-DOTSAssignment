using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Components {
    public struct Shooter : IComponentData {
        public Entity projectilePrefab;
        public bool isShooting;
        public float3 spawnPosition;
    }
}
