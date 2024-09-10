using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Systems
{
    [BurstCompile]
    public partial struct SpawnerSystem : ISystem {
        // some code from https://discussions.unity.com/t/solved-mathematics-random-in-job-without-ecs/740232/2
        private NativeArray<Random> RandomGenerator;
        private Random random;

        public void OnCreate(ref SystemState state) {
            state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
            random = new Random(1);
        }

        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer.ParallelWriter ecb = GetEntityCommandBuffer(ref state);

            RandomGenerator = new NativeArray<Unity.Mathematics.Random>(Environment.ProcessorCount, Allocator.TempJob);
            for (int i = 0; i < RandomGenerator.Length; i++)
            {
                RandomGenerator[i] = new Unity.Mathematics.Random((uint)random.NextInt());
            }

            // Creates a new instance of the job, assigns the necessary data, and schedules the job in parallel.
            new ProcessSpawnerJob
            {
                elapsedTime = SystemAPI.Time.ElapsedTime,
                ecb = ecb,
                RandomGenerator = RandomGenerator
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

        [NativeDisableParallelForRestriction]
        [DeallocateOnJobCompletion]
        public NativeArray<Unity.Mathematics.Random> RandomGenerator;


        [NativeSetThreadIndex]
        private int threadIndex;

        // IJobEntity generates a component data query based on the parameters of its `Execute` method.
        // This example queries for all Spawner components and uses `ref` to specify that the operation
        // requires read and write access. Unity processes `Execute` for each entity that matches the
        // component data query.
        private void Execute([ChunkIndexInQuery] int chunkIndex, ref Components.Spawner spawner) {
            if(!(spawner.nextSpawnTime < elapsedTime)) {
                return;
            }
            Random random = RandomGenerator[threadIndex];
            float3 spawnPosition = new float3(random.NextFloat(spawner.xSpawnRange.x, spawner.xSpawnRange.y), random.NextFloat(spawner.ySpawnRange.x, spawner.ySpawnRange.y), random.NextFloat(spawner.zSpawnRange.x, spawner.zSpawnRange.y));
            RandomGenerator[threadIndex] = random;

            Entity newEntity = ecb.Instantiate(chunkIndex, spawner.prefab);
            ecb.SetComponent(chunkIndex, newEntity, LocalTransform.FromPosition(spawnPosition));

            spawner.nextSpawnTime = (float)elapsedTime + spawner.spawnRate;
        }
    }
}
