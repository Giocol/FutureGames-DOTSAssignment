using Components;
using Unity.Entities;
using UnityEngine;

namespace Entities {
    public class DespawnerAuthoring : MonoBehaviour
    {
        public float timeToLive;
        public float timeAlive;
    }

    public class DespawnerBaker : Baker<DespawnerAuthoring> {
        public override void Bake(DespawnerAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Despawner { timeToLive = authoring.timeToLive, timeAlive = authoring.timeAlive });
        }
    }
}
