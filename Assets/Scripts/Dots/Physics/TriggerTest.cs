/*
 * Creating a Trigger Event:
 * 1.) Create a Job that implements ITriggerEventsJob
 * 2.) In Execute, use TriggerEvent to get the entities involved with the interaction and do whatever to them
 * 3.) Schedule job in OnUpdate using the StepPhysicsWorld.Simulation and the BuildPhysicsWorld.PhysicsWorld
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

public class TriggerTest : JobComponentSystem
{
    [BurstCompile]
    private struct TriggerJob: ITriggerEventsJob
    {
        // This allows us to check the triggered entity for specific components
        public ComponentDataFromEntity<PhysicsVelocity> physicsVelocityEntities;
        
        public void Execute(TriggerEvent triggerEvent)
        {
            // If the triggered Entity has a PhysicsVelocity component, we:
            // 1.) Grab the PhysicsVelocity Component
            // 2.) Modify it
            // 3.) Reassign the component to the entity
            if(physicsVelocityEntities.HasComponent(triggerEvent.Entities.EntityA))
            {
                PhysicsVelocity physicsVelocity = physicsVelocityEntities[triggerEvent.Entities.EntityA];
                physicsVelocity.Linear.y = 5f;
                physicsVelocityEntities[triggerEvent.Entities.EntityA] = physicsVelocity;
            }
            
            if(physicsVelocityEntities.HasComponent(triggerEvent.Entities.EntityB))
            {
                PhysicsVelocity physicsVelocity = physicsVelocityEntities[triggerEvent.Entities.EntityB];
                physicsVelocity.Linear.y = 5f;
                physicsVelocityEntities[triggerEvent.Entities.EntityB] = physicsVelocity;
            }
        }
    }

    // In order to schedule this job, we need to access the BuildPhysicsWorld and StepPhysicsWorld
    // So create the variable
    private BuildPhysicsWorld buildPhysicsWorld;

    private StepPhysicsWorld stepPhysicsWorld;
    // And then OnCreate we get reference to the worlds
    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        TriggerJob triggerJob = new TriggerJob
        {
            physicsVelocityEntities = GetComponentDataFromEntity<PhysicsVelocity>()
        };
        return triggerJob.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
    }
}
