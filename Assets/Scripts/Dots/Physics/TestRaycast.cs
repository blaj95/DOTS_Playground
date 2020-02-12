using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Ray = UnityEngine.Ray;

public class TestRaycast : MonoBehaviour
{
    private Entity Raycast(float3 fromPosition, float3 toPosition)
    {
        // Get the CollisionWorld
        BuildPhysicsWorld buildPhysicsWorld = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();
        CollisionWorld collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;
        
        // Create Raycast Input, Filters are essentially layers from old raycast
        // ~0u is a bit operation not, meaning we get all bits after 1 and includes all layers
        // Group Index is used to filter for collisions by overriding the bitmask
        RaycastInput raycastInput = new RaycastInput
        {
            Start = fromPosition,
            End = toPosition,
            Filter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = ~0u,
                GroupIndex = 0
            }
        };
        
        // Create a RaycastHit
        Unity.Physics.RaycastHit raycastHit = new Unity.Physics.RaycastHit();
        
        // Create Ray, returns a bool
        if(collisionWorld.CastRay(raycastInput,out raycastHit)) // Can also use a NativeList of RaycastHits for all hits
        {
            // Hit Something
            Entity hitEntity = buildPhysicsWorld.PhysicsWorld.Bodies[raycastHit.RigidBodyIndex].Entity;
            return hitEntity;
        }
        else
        {
            return Entity.Null;
        }
    }

    private void Update()
    {
        // Generate a raycast on a mouse click from the main thread
        if (Input.GetMouseButtonDown(0))
        {
            UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance = 100f;
            
            Debug.Log(Raycast(ray.origin, ray.direction * rayDistance));
        }
    }
}
