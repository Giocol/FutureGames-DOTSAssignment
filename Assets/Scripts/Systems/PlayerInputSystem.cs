using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems {
    public partial class PlayerInputSystem : SystemBase {
        private PlayerControls playerControls;

        protected override void OnCreate() {
            playerControls = new PlayerControls();
        }

        protected override void OnStartRunning() {
            playerControls.Enable();
        }

        protected override void OnUpdate() {
            bool shootInput = playerControls.Player.Shoot.IsPressed();
            float2 movementInput = (float2) playerControls.Player.Move.ReadValue<Vector2>();

            new UpdateInputJob{
                shootInput = shootInput,
                movementInput = movementInput
            }.Schedule();
        }
    }

    [BurstCompile]
    public partial struct UpdateInputJob : IJobEntity {
        public bool shootInput;
        public float2 movementInput;

        [BurstCompile]
        private void Execute(ref Mover mover, ref Shooter shooter, PlayerTag playerTag, LocalTransform localTransform) {
            mover.movementDirection = new float3(movementInput.x, movementInput.y, 0);
            shooter.isShooting = shootInput;
            shooter.spawnPosition = localTransform.Position;
            //TODO: shooting cooldown
        }
    }
}
