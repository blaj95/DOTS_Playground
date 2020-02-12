/*
 * In order to get output from Jobs, you need to use Native Collections 
*/

using TMPro;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class JobsOutput : MonoBehaviour
{
    public TMP_Text jobsText;
        
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        NativeArray<float> result = new NativeArray<float>(1,Allocator.TempJob);
        
        SimpleJob simpleJob = new SimpleJob
        {
            a = 1,
            deltaTime = Time.deltaTime,
            result = result
        };

        JobHandle jobHandle = simpleJob.Schedule();
        jobHandle.Complete();

        jobsText.text = result[0].ToString();
        
        result.Dispose();
    }
}

[BurstCompile]
public struct SimpleJob : IJob
{
    public int a;
    public float deltaTime;
    public NativeArray<float> result;
    
    public void Execute()
    {
        result[0] = a * deltaTime;
    }
}
