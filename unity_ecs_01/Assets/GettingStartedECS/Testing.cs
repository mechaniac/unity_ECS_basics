using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Entities; // needed 
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;
using Unity.Mathematics;

public class Testing : MonoBehaviour
{

    [SerializeField] public Mesh mesh;
    [SerializeField] public Material material;

    private void Start()
    {

        EntityManager entityManager =  World.Active.EntityManager;

        EntityArchetype myArchetype = entityManager.CreateArchetype(
            typeof(LevelComponent),
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(MoveSpeedComponent)
            );
        
        NativeArray<Entity> myArray = new NativeArray<Entity>(10000, Allocator.Temp);
        entityManager.CreateEntity(myArchetype, myArray);

        for(int i = 0; i < myArray.Length; i++)
        {
            Entity entity = myArray[i];
            entityManager.SetComponentData(entity, new LevelComponent { level = UnityEngine.Random.Range(0,10)});
            entityManager.SetComponentData(entity, new MoveSpeedComponent { moveSpeed = UnityEngine.Random.Range(1f, 2f) });
            entityManager.SetComponentData(entity, new Translation
            {
                Value = new float3(UnityEngine.Random.Range(-8f,8f), UnityEngine.Random.Range(-5f,5f),0)
            });
            entityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = mesh,
                material = material,
            });
        }
        myArray.Dispose();
        
        
    }
}
