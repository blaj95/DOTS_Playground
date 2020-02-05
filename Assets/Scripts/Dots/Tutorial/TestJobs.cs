using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;
using Random = UnityEngine.Random;

public class TestJobs : MonoBehaviour
{
    [SerializeField] private bool useJobsSystem;
    [SerializeField] private Transform prefab;
    private List<MyPrefab> myPrefabs;
    public class MyPrefab
    {
        public Transform transform;
        public float moveY;
    }
        
    private void Start()
    {
        myPrefabs = new List<MyPrefab>();
        for (int i = 0; i < 5000; i++)
        {
            Transform transform = Instantiate(prefab, new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), Random.Range(-20, 20)),
                quaternion.identity);
            myPrefabs.Add(new MyPrefab{transform = transform, moveY = Random.Range(1f,5f)});
        }
    }

    private void Update()
    {
        float startTime = Time.realtimeSinceStartup;
        if (useJobsSystem)
        {
            // If this was an actual game, we could instantiate this once and update them versus new lists every frame
            // NativeArray<float3> positionArray = new NativeArray<float3>(myPrefabs.Count, Allocator.TempJob);
            NativeArray<float> moveYArray = new NativeArray<float>(myPrefabs.Count, Allocator.TempJob);
            TransformAccessArray transformAccessArray = new TransformAccessArray(myPrefabs.Count);
            for (int i = 0; i < myPrefabs.Count; i++)
            {
                // positionArray[i] = myPrefabs[i].transform.position;
                moveYArray[i] = myPrefabs[i].moveY;
                transformAccessArray.Add(myPrefabs[i].transform);
            }
            // TaskJobParallel jobParallel = new TaskJobParallel
            // {
            //     deltaTime = Time.deltaTime,
            //     positionArray = positionArray,
            //     moveYArray = moveYArray
            // };
            
            TaskJobParallelTransform jobParallelTransform = new TaskJobParallelTransform
            {
                deltaTime = Time.deltaTime,
                moveYArray = moveYArray
            };

            // Need to pass in array length and the size of each job batch (How many indexes each job will be responsible for)
            // Since we are working on 1000 objects, let's send each job to handle 100
            // JobHandle jobHandle = jobParallel.Schedule(myPrefabs.Count,
            //     500);
            // Specific Schedule for ParallelTransform jobs
            JobHandle jobHandle = jobParallelTransform.Schedule(transformAccessArray);
            jobHandle.Complete();
            for (int i = 0; i < myPrefabs.Count; i++)
            {
                // myPrefabs[i].transform.position = positionArray[i];
                myPrefabs[i].moveY = moveYArray[i];
            }

            // positionArray.Dispose();
            moveYArray.Dispose();
            transformAccessArray.Dispose();
        }
        else
        {
            foreach (var obj in myPrefabs)
            {
                obj.transform.position += new Vector3(0,obj.moveY * Time.deltaTime,0);
                if (obj.transform.position.y > 20)
                {
                    obj.moveY = -math.abs(obj.moveY);
                }
                if (obj.transform.position.y < -20)
                {
                    obj.moveY = math.abs(obj.moveY);
                }
            
                float value = 0f;
                for (int i = 0; i < 50000; i++)
                {
                    value = math.exp10(math.sqrt(value));
                }
            }    
        }
        
        // if (useJobsSystem)
        // {
        //     // Calling a job once on a separate thread is the same as calling it once on the main thread
        //     // Assume we are doing something more complex, like pathfinding for 10 units
        //     NativeList<JobHandle> jobHandles = new NativeList<JobHandle>(Allocator.Temp);
        //     for (int i = 0; i < 10; i++)
        //     {
        //         // Doing this would be the same as running the on the main thread
        //         // We need to first create all of the jobs we need and then tell them to complete
        //         // JobHandle jobHandle = ScheduleTaskJob();
        //         // jobHandle.Complete(); // Pauses main thread until this job is complete
        //
        //         JobHandle jobHandle = ScheduleTaskJob();
        //         jobHandles.Add(jobHandle);
        //         
        //     }
        //     // Static method to complete all jobs in a Native collection
        //     JobHandle.CompleteAll(jobHandles);
        //     jobHandles.Dispose(); // Must dispose native collection if Allocator.Temp
        //
        // }
        // else
        // {
        //     for (int i = 0; i < 10; i++)
        //     {
        //         Task();    
        //     }
        // }

        Debug.Log((Time.realtimeSinceStartup-startTime)*1000f + " ms");
    }

    void Task()
    {
        float value = 0f;
        for (int i = 0; i < 50000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }

    JobHandle ScheduleTaskJob()
    {
        TaskJob job = new TaskJob();
        return job.Schedule(); // This tells the job system to schedule this job to be completed by available thread when possible
    }
}

[BurstCompile]
public struct TaskJob : IJob // A job must be a struct that inherits one of the IJob interfaces
{
    //public float something;
    
    public void Execute()
    {
        float value = 0f;
        for (int i = 0; i < 50000; i++)
        {
            value = math.exp10(math.sqrt(value));
        } 
    }
}

[BurstCompile] 
public struct TaskJobParallel : IJobParallelFor
{
    // This is the data we are directly modifying
    
    public NativeArray<float3> positionArray;
    public NativeArray<float> moveYArray;
    public float deltaTime;
    public void Execute(int index)
    {
        // Can't access normal components since this is on a separate thread
        // So we need to think about what data we are directly changing
        // obj.transform.position += new Vector3(0,obj.moveY * Time.deltaTime,0);
        // if (obj.transform.position.y > 20)
        // {
        //     obj.moveY = -math.abs(obj.moveY);
        // }
        // if (obj.transform.position.y < -20)
        // {
        //     obj.moveY = math.abs(obj.moveY);
        // }
        positionArray[index] += new float3(0, moveYArray[index] * deltaTime, 0); // Can't access Time.deltaTime off main thread so much pass it in
        if (positionArray[index].y > 20)
        {
            moveYArray[index] = -math.abs(moveYArray[index]);
        }
        if (positionArray[index].y < -20)
        {
            moveYArray[index] = math.abs(moveYArray[index]);
        }
    }
}


public struct TaskJobParallelTransform : IJobParallelForTransform
{
    public NativeArray<float> moveYArray;
    public float deltaTime;
    public void Execute(int index, TransformAccess transform)
    {
        transform.position += new Vector3(0, moveYArray[index] * deltaTime, 0); // Can't access Time.deltaTime off main thread so much pass it in
        if (transform.position.y > 20)
        {
            moveYArray[index] = -math.abs(moveYArray[index]);
        }
        if (transform.position.y < -20)
        {
            moveYArray[index] = math.abs(moveYArray[index]);
        }
    }
}
