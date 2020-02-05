﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

public class PlayerMovementSystem : JobComponentSystem
{
    // Method of calling jobs outside of MonoBehaviours
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dir = Random.Range(-5f, 5f);
        float deltaTime = Time.DeltaTime;
        // ref for read + write, in for read
        JobHandle jobHandle = Entities.ForEach((ref Translation translation, in MoveDirection moveDirection) =>
            {
                translation.Value += new float3(0,dir,0);
            }).Schedule(inputDeps);
        return jobHandle;
    }
}
