using Unity.Entities;
using UnityEngine;

namespace Components {
    public struct Despawner : IComponentData {
        public float timeToLive;
        public float timeAlive;
    }
}
