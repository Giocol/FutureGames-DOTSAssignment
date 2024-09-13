using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    public partial struct DespanwerSystem : ISystem {

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }


        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer(ref state);
            float time = SystemAPI.Time.DeltaTime;
            new DespawnerJob
            {
                ecb = ecb,
                time = time
            }.ScheduleParallel();
        }

        private EntityCommandBuffer.ParallelWriter GetEntityCommandBuffer(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            EntityCommandBuffer ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            return ecb.AsParallelWriter();
        }
    }

    [BurstCompile]
    public partial struct DespawnerJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public float time;

        private void Execute([ChunkIndexInQuery] int chunkIndex, ref Despawner despawner, Entity entity) {
            despawner.timeAlive += time;
            if(despawner.timeAlive > despawner.timeToLive) {
                ecb.DestroyEntity(chunkIndex, entity);
            }
        }
    }
}
