using Components;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Entities
{
    public class MoverAuthoring : MonoBehaviour
    {
        public float movementSpeed;
        public float3 movementDirection;

        public class MoverBaker : Baker<MoverAuthoring> {
            public override void Bake(MoverAuthoring authoring) {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Mover { movementSpeed = authoring.movementSpeed, movementDirection = authoring.movementDirection });
            }
        }
    }
}
