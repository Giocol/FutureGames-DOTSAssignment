using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    public partial struct OptimizedSpawnerSystem : ISystem
    {
        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
        }

        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer(ref state);

            // Creates a new instance of the job, assigns the necessary data, and schedules the job in parallel.
            new ProcessSpawnerJob
            {
                elapsedTime = SystemAPI.Time.ElapsedTime,
                ecb = ecb
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
    public partial struct ProcessSpawnerJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ecb;
        public double elapsedTime;

        // IJobEntity generates a component data query based on the parameters of its `Execute` method.
        // This example queries for all Spawner components and uses `ref` to specify that the operation
        // requires read and write access. Unity processes `Execute` for each entity that matches the
        // component data query.
        private void Execute([ChunkIndexInQuery] int chunkIndex, ref Components.Spawner spawner) {
            if(!(spawner.nextSpawnTime < elapsedTime)) {
                return;
            }

            Entity newEntity = ecb.Instantiate(chunkIndex, spawner.prefab);
            ecb.SetComponent(chunkIndex, newEntity, LocalTransform.FromPosition(spawner.spawnPosition));

            spawner.nextSpawnTime = (float)elapsedTime + spawner.spawnRate;
        }
    }
}
