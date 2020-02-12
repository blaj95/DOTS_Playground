using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public class TargetFindSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        // Cycle through Units
        Entities.WithNone<HasTarget>().WithAll<Unit>().ForEach((Entity entity, ref Translation unitTranslation) =>
        {
            float3 unitPosition = unitTranslation.Value;
            float3 closestTargetPosition = float3.zero;
            Entity closestTargetEntity = Entity.Null;

            // Cycle through Targets
            Entities.WithAll<Target>().WithNone<IsTargeted>().ForEach((Entity targetEntity, ref Translation targetTranslation) =>
            {
                // There is no target
                if (closestTargetEntity == Entity.Null)
                {
                    closestTargetEntity = targetEntity;
                    closestTargetPosition = targetTranslation.Value;
                }
                else
                {
                    if (math.distance(unitPosition, targetTranslation.Value) <
                        math.distance(unitPosition, closestTargetPosition))
                    {
                        closestTargetEntity = targetEntity;
                        closestTargetPosition = targetTranslation.Value;
                    }
                }
            });

            if (closestTargetEntity != Entity.Null)
            {  
                // Entity Command Buffer once Update is done
                PostUpdateCommands.AddComponent(entity, new HasTarget{TargetEntity = closestTargetEntity});
                PostUpdateCommands.AddComponent(closestTargetEntity, new IsTargeted());
            }
            
        });
        
        // Entities.ForEach((ref Unit unit) =>
        // {
        //     Debug.Log();
        // });
    }
}
