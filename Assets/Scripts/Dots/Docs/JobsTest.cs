using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class JobsTest : MonoBehaviour
{
// JOBS
    // If a Job DOESN'T need to WRITE to a container, mark it with [ReadOnly]
        //[ReadOnly]
        //public NativeArray<int> input;
    // Access static data circumvents all safety systems and can crash Unity
    
    // When creating a NativeContainer, you must specify the type of memory allocation you need
    // 3 Types
        // Allocator.Temp - FASTEST ALLOCATION, for allocations with a LIFETIME of ONE FRAME or FEWER. SHOULD NOT PASS TO TO JOBS. Need to call DISPOSE method BEFORE you return from the method call.
           NativeArray<float> resultsOneFrame = new NativeArray<float>(1, Allocator.Temp);
        // Allocator.TempJob - SLOWER than TEMP but FASTER than PERSISTENT. For allocations within lifespan of FOUR FRAMES and is Thread-Safe. Need to DISPOSE within four frames or native code sends a warning. Most common for SMALL JOBS.
           NativeArray<float> resultsFourFrames = new NativeArray<float>(1, Allocator.TempJob);
        // Allocator.Persistent - SLOWEST but can last forever if needed. WRAPPER for direct call to malloc. USED for long jobs, should not be used where PERFORMANCE is ESSENTIAL.
           NativeArray<float> resultsAllFrames = new NativeArray<float>(1, Allocator.Persistent);
           
// DEBUGGING
    // You can use the Run function in place of Schedule to execute immediately on the main thread.

// CREATING A JOB
    // To create Job, you need to implement the IJob Interface. 
    // IJob allows you to schedule a single job that runs in parallel to any other jobs that are running
    // IJOB is a term for any struct that implements the IJob Interface.
    
        // Create a struct that implements IJob
        // Add member variables that the Job uses
        // Create a method in the struct called Execute with implementation inside
            // Execute methods runs once on a single core
            
    // The only way to access data from a job in the main thread is by writing to a NativeContainer.
        // Otherwise Jobs operate on copies of data
        
    public struct MyFirstJob: IJob
    {
        private float a;
        private float b;
        private NativeArray<float> result;
        
        public void Execute()
        {
            result[0] = a * b;
        }
    }
    
// SCHEDULING JOBS
    // To schedule a job in the main thread
        // Instantiate Job
        // Populate Jobs Data
        // Call the Schedule Method
            // Calling Schedule puts the job into the job queue for execution at the appropriate time. Once Scheduled, you can't interrupt a job.
            // Can only call schedule from the main thread.
            // Create a native array of a single float to store the result. This example waits for the job to complete for illustration purposes
    
// Example 
    NativeArray<float> result = new NativeArray<float>(1, Allocator.TempJob);
    
    // Set up the job data
     MyFirstJob jobData = new MyFirstJob();
    // jobData.a = 10;
    // jobData.b = 10;
    // jobData.result = result;
    
    // Schedule the job, which returns a JobHandle
    // JobHandle handle = jobData.Schedule();
    
    // Wait for the job to complete
    // handle.Complete();
    
    // All copies of the NativeArray point to the same memory, you can access the result in "your" copy of the NativeArray
    // float aPlusB = result[0];
    
    // Free the memory allocated by the result array
    // result.Dispose();
    
// COMBINING DEPENDENCIES
    // You can use a JobHandle in your code as a dependency for other jobs. If a job depends on the results of another job, you can pass the first job’s JobHandle as a parameter to the second job’s Schedule method, like so:
    // JobHandle firstJobHandle = firstJob.Schedule();
    // secondJob.Schedule(firstJobHandle);
    
    // You can combine dependencies if a job has many dependencies.
    NativeArray<JobHandle> handles = new NativeArray<JobHandle>(100, Allocator.TempJob);
    // Populate `handles` with `JobHandles` from multiple scheduled jobs...
    // JobHandle jh = JobHandle.CombineDependencies(handles);
    
//Example
    // Schedule job #1
    // JobHandle firstHandle = jobData.Schedule();

// Setup the data for job #2
    // AddOneJob incJobData = new AddOneJob();
    // incJobData.result = result;

// Schedule job #2
    // JobHandle secondHandle = incJobData.Schedule(firstHandle);

// Wait for job #2 to complete
    // secondHandle.Complete();

// All copies of the NativeArray point to the same memory, you can access the result in "your" copy of the NativeArray
    // float aPlusB = result[0];

// Free the memory allocated by the result array
    // result.Dispose();    
    
//PARALLELFOR JOBS
    // Used for performing the same operation on a large number of objects.
    // ParallelFor Jobs invokes the Execute method once per item in the data source. 
    // There is an integer parameter in the Execute method. This index is to access and operate on a single element of the data source within the job implementation.
//Example
    struct IncrementByDeltaTimeJob: IJobParallelFor
    {
        public NativeArray<float> values;
        public float deltaTime;

        public void Execute (int index)
        {
            float temp = values[index];
            temp += deltaTime;
            values[index] = temp;
        }
    }
    // Job adding two floating point values together
    public struct MyParallelJob : IJobParallelFor
    {
        [ReadOnly]
        public NativeArray<float> a;
        [ReadOnly]
        public NativeArray<float> b;
        public NativeArray<float> result;

        public void Execute(int i)
        {
            result[i] = a[i] + b[i];
        }
    }
    // ParallelFor Jobs are split into BATCHES to execute in separate native jobs in the JOB QUEUE
//Example
    // NativeArray<float> a = new NativeArray<float>(2, Allocator.TempJob);
    //
    // NativeArray<float> b = new NativeArray<float>(2, Allocator.TempJob);
    //
    //NativeArray<float> result = new NativeArray<float>(2, Allocator.TempJob);
    
    // a[0] = 1.1;
    // b[0] = 2.2;
    // a[1] = 3.3;
    // b[1] = 4.4;
    //
    // MyParallelJob jobData = new MyParallelJob();
    // jobData.a = a;  
    // jobData.b = b;
    // jobData.result = result;

// Schedule the job with one Execute per index in the results array and only 1 item per processing batch
    // JobHandle handle = jobData.Schedule(result.Length, 1);

// Wait for the job to complete
    // handle.Complete();

// Free the memory allocated by the arrays
    // a.Dispose();
    // b.Dispose();
    // result.Dispose();
    
//PARALLELFORTRANSFORM Jobs
    // Jobs designed specifically to operate on Transforms
    public struct MyTransformJob : IJobParallelForTransform
    {
        [ReadOnly] private float speed;

        public void Execute(int index, TransformAccess transform)
        {
            transform.position += Vector3.forward * speed;
        }
    }
}
