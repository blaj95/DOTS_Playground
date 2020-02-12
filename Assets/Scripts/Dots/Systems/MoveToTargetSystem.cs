using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
[BurstCompile]

public class MoveToTargetSystem : ComponentSystem
{

    protected override void OnUpdate()
    {
       Entities.ForEach((Entity unitEntity, ref HasTarget hasTarget, ref Translation translation, ref LocalToWorld localToWorld) =>
       {
           if (World.DefaultGameObjectInjectionWorld.EntityManager.Exists(hasTarget.TargetEntity))
           {
               Translation targetTranslation =
                   World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Translation>(
                       hasTarget.TargetEntity);
               LocalToWorld targetLocalToWorld =
                   World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LocalToWorld>(
                       hasTarget.TargetEntity);

               float3 targetDirection = math.normalize(targetTranslation.Value - translation.Value);
               float moveSpeed = 10f;
               translation.Value += targetDirection * moveSpeed * Time.DeltaTime;
             
            
               // If are Unit if close to the target, destroy it
               if (math.distance(translation.Value, targetTranslation.Value) < .2f)
               {
                   PostUpdateCommands.DestroyEntity(hasTarget.TargetEntity);
                   PostUpdateCommands.RemoveComponent(unitEntity,typeof(HasTarget));
               }     
           }
           else
           {
               PostUpdateCommands.RemoveComponent(unitEntity, typeof(HasTarget));
           }
          
       });
    }
}
