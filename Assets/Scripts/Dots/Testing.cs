using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Random = UnityEngine.Random;

public class Testing : MonoBehaviour
{
    EntityManager manager;

    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;

    private EntityArchetype entityArchetype;
    
    // Start is called before the first frame update
    void Start()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        entityArchetype = manager.CreateArchetype(
            typeof(LevelComponent), 
            typeof(Translation),
            typeof(Rotation),
            typeof(RenderMesh), // RENDERMESH IS A SHARED COMPONENT
            typeof(LocalToWorld),
            typeof(MoveSpeedComponent));
        
        NativeArray<Entity> entityArray = new NativeArray<Entity>(5000, Allocator.Temp);

        manager.CreateEntity(entityArchetype, entityArray);

        for (int i = 0; i < entityArray.Length; i++)
        {
            Entity e = entityArray[i];
            manager.SetComponentData(e, new LevelComponent{level = Random.Range(0,100)});
            manager.SetComponentData(e, new MoveSpeedComponent{moveSpeed = Random.Range(1,10)});
            manager.SetComponentData(e, new Translation{Value = new float3(Random.Range(-100,100),Random.Range(-100,100),Random.Range(-100,100))});
            
            manager.SetSharedComponentData(e, new RenderMesh{material = material, mesh = mesh});
        }

        entityArray.Dispose();

        //Entity entity = manager.CreateEntity(entityArchetype);

        // Entity entity = manager.CreateEntity(typeof(LevelComponent));
//        
//        manager.SetComponentData(entity, new LevelComponent
//        {
//            level = 10.0f
//        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NativeArray<Entity> entityArray = new NativeArray<Entity>(500, Allocator.Temp);

            manager.CreateEntity(entityArchetype, entityArray);

            for (int i = 0; i < entityArray.Length; i++)
            {
                Entity e = entityArray[i];
                manager.SetComponentData(e, new LevelComponent{level = Random.Range(0,100)});
                manager.SetComponentData(e, new MoveSpeedComponent{moveSpeed = Random.Range(1,10)});
                manager.SetComponentData(e, new Translation{Value = new float3(Random.Range(-100,100),Random.Range(-100,100),Random.Range(-100,100))});
            
                manager.SetSharedComponentData(e, new RenderMesh{material = material, mesh = mesh});
            }

            entityArray.Dispose();
        }
    }
}
