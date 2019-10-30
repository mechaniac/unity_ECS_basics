using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class JobsStartScript : MonoBehaviour
{
    [SerializeField] private bool useJobs;

    [SerializeField] private Transform cSprite;
    private List<CSprite> cSpriteList;

    public class CSprite
    {
        public Transform transform;
        public float moveY;
    }

    private void Start()
    {
        cSpriteList = new List<CSprite>();
        for(int i = 0; i < 1000; i++)
        {
            Transform cTransform = Instantiate(cSprite, new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-5f, 5f)), Quaternion.identity);
            cSpriteList.Add(new CSprite
            {
                transform = cTransform,
                moveY = UnityEngine.Random.Range(1f, 2f)
            });
        }
    }

    private void Update()
    {
        float startTime = Time.realtimeSinceStartup;
        if (useJobs) {

            NativeArray<float3> positionArray = new NativeArray<float3>(cSpriteList.Count, Allocator.TempJob);
            NativeArray<float> moveYArray = new NativeArray<float>(cSpriteList.Count, Allocator.TempJob);

            for (int i = 0;i<cSpriteList.Count; i++)
            {
                positionArray[i] = cSpriteList[i].transform.position;
                moveYArray[i] = cSpriteList[i].moveY;
            }

            ReallyToughParallelJob reallyToughParallelJob = new ReallyToughParallelJob
            {
                deltaTime = Time.deltaTime,
                positionArray = positionArray,
                moveYArray = moveYArray

            };

            JobHandle jobHandle = reallyToughParallelJob.Schedule(cSpriteList.Count, 100);
            jobHandle.Complete();

            for(int i = 0; i < cSpriteList.Count; i++)
            {
                cSpriteList[i].transform.position = positionArray[i];
                cSpriteList[i].moveY = moveYArray[i];
            }
            positionArray.Dispose();
            moveYArray.Dispose();
        }

        
        else { 
        foreach(CSprite csprite in cSpriteList)
        {
            csprite.transform.position += new Vector3(0, csprite.moveY * Time.deltaTime);

            if (csprite.transform.position.y > 5f)
            {
                csprite.moveY = -math.abs(csprite.moveY);
            }
            if(csprite.transform.position.y < -5f)
            {
                csprite.moveY = math.abs(csprite.moveY);
            }
            float value = 0f;
            for (int i = 0; i < 1000; i++)
            {
                value = math.exp10(math.sqrt(value));
            }
        }
        }

        /*
        if (useJobs) {
            NativeList<JobHandle> jobHandleList = new NativeList<JobHandle>(Allocator.Temp);
            for(int i = 0; i < 10; i++)
            {
                JobHandle jobHandle = ReallyToughTaskJob();
                jobHandleList.Add(jobHandle);
            }
            JobHandle.CompleteAll(jobHandleList);
            jobHandleList.Dispose();
        } else {
            for (int i = 0; i < 10; i++)
            {
                ReallyToughTask();
            }
            
        };

        */


        Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
    }

    private void ReallyToughTask()
    {
        //Represents a complex calculation
        float value = 0f;
        for(int i = 0; i < 50000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }

    private JobHandle ReallyToughTaskJob()
    {
        ReallyToughJob job = new ReallyToughJob();
        return job.Schedule();
    }
}
[BurstCompile]
public struct ReallyToughJob : IJob
{
    public void Execute()
    {
        //Represents a complex calculation
        float value = 0f;
        for (int i = 0; i < 50000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }

    }
}


[BurstCompile]
public struct ReallyToughParallelJob : IJobParallelFor
{
    public NativeArray<float3> positionArray;
    public NativeArray<float> moveYArray;

    [ReadOnly] public float deltaTime;  //doesnt need readonly (use only on native fields)
    public void Execute(int index)
    {

        positionArray[index] += new float3(0, moveYArray[index] * deltaTime,0f);

        if (positionArray[index].y > 5f)
        {
            moveYArray[index] = -math.abs(moveYArray[index]);
        }
        if (positionArray[index].y < -5f)
        {
            moveYArray[index] = math.abs(moveYArray[index]);
        }
        float value = 0f;
        for (int i = 0; i < 1000; i++)
        {
            value = math.exp10(math.sqrt(value));
        }
    }
}
