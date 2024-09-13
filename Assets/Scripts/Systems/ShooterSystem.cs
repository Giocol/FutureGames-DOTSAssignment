using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    public partial struct ShooterSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }


        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer(ref state);
            float time = SystemAPI.Time.DeltaTime;
            new ProjectileSpawnerJob
            {
                ecb = ecb,
                time = time
            }.Schedule();
        }

        private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            EntityCommandBuffer ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            return ecb.AsParallelWriter();
        }
    }

    [BurstCompile]
    public partial struct ProjectileSpawnerJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public float time;

        private void Execute([ChunkIndexInQuery] int chunkIndex, ref Shooter shooter) {
            shooter.timeSinceLastShoot += time;
            if(shooter.isShooting && shooter.timeSinceLastShoot > shooter.shootingCooldown) {
                Entity newEntity = ecb.Instantiate(chunkIndex, shooter.projectilePrefab);
                ecb.SetComponent(chunkIndex, newEntity, LocalTransform.FromPosition(shooter.spawnPosition));
                shooter.timeSinceLastShoot = 0;
            }
        }
    }
}
