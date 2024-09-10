using Unity.Entities;
using Unity.Mathematics;

namespace Components {
    public struct Mover : IComponentData {
        public float movementSpeed;
        public float3 movementDirection;
    }

}
