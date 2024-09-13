﻿using Components;
using Unity.Entities;
using UnityEngine;

namespace Entities {
    public class PlayerTagAuthoring : MonoBehaviour
    {
    }

    public class PlayerTagBaker : Baker<PlayerTagAuthoring> {
        public override void Bake(PlayerTagAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<PlayerTag>(entity);
        }
    }
}
