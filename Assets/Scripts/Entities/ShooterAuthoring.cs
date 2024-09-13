using Components;
using Unity.Entities;
using UnityEngine;

namespace Entities {

    public class ShooterAuthoring : MonoBehaviour
    {
    }

    public class ShooterBaker : Baker<ShooterAuthoring> {
        public override void Bake(ShooterAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Shooter { isShooting = false });
        }
    }
}
