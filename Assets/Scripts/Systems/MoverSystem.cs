using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Systems {
    public partial struct MoverSystem : ISystem {

        [BurstCompile]
        public void OnUpdate(ref SystemState state) {
            float deltaTime = SystemAPI.Time.DeltaTime;

            new MoverJob {
                deltaTime = deltaTime
            }.Schedule();
        }

        [BurstCompile]
        public partial struct MoverJob : IJobEntity {
            public float deltaTime;

            [BurstCompile]
            private void Execute(Mover mover, ref LocalTransform localTransform) {
                localTransform.Position += mover.movementDirection * mover.movementSpeed * deltaTime;
            }
        }
    }
}
