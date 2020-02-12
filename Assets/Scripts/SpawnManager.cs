using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Material unitMaterial, targetMaterial;
    [SerializeField] private Mesh quadMesh;
    public int unitCount;
    
    private static EntityManager entityManager;
    
    // Start is called before the first frame update
    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        for (int i = 0; i < unitCount; i++)
        {
            SpawnUnitEntity();     
        }
       

        for (int i = 0; i < 10; i++)
        {
            SpawnTargetEntity();
        }
        
    }

    void SpawnUnitEntity()
    {
       SpawnUnitEntity(new float3(UnityEngine.Random.Range(-8,8f),UnityEngine.Random.Range(-8,8f),0));     
    }
    
    void SpawnUnitEntity(float3 position)
    {
        Entity entity = entityManager.CreateEntity(typeof(Translation), typeof(LocalToWorld), typeof(RenderMesh),
            typeof(Scale), typeof(Unit));
        SetEntityComponentData(entity, position,1.2f, quadMesh, unitMaterial);
    }
    void SpawnTargetEntity()
    {
        Entity entity = entityManager.CreateEntity(typeof(Translation), typeof(LocalToWorld), typeof(RenderMesh),
            typeof(Scale), typeof(Target));
        SetEntityComponentData(entity, new float3(UnityEngine.Random.Range(-8,8f),UnityEngine.Random.Range(-8,8f),0),.5f,quadMesh, targetMaterial);
    }
    
    private void SetEntityComponentData(Entity entity, float3 spawnPosition, float scale ,Mesh mesh, Material material)
    {
        entityManager.SetSharedComponentData(entity,new RenderMesh{material = material,mesh = mesh}); 
        entityManager.SetComponentData(entity,new Translation{Value = spawnPosition});
        entityManager.SetComponentData(entity,new Scale{Value = scale});
    }
}

// Unit Tag
public struct Unit: IComponentData{}
// Target Tag
public struct Target: IComponentData{}
