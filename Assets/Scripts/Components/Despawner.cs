using Unity.Entities;

namespace Components {
    public struct Despawner : IComponentData {
        public float timeToLive;
        public float timeAlive;
    }
}
