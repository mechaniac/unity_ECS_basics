using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;

public class DrawSprite : MonoBehaviour
{
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;

    private EntityManager entityManager;

    private void Start()
    {
        entityManager = World.Active.EntityManager;
        Entity entity = entityManager.CreateEntity(
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(Translation),
            typeof(Rotation)
           // typeof(Scale)
            );

        entityManager.SetSharedComponentData(entity, new RenderMesh
        {
            mesh = mesh,
            material = material,
        });
    }


}

public class MoveSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation translation) =>
        {
            float moveSpeed = 1f;
            translation.Value.y += moveSpeed * Time.deltaTime;
        });
    }
}

public class RotatorSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Rotation rotation) =>
        {
            
            rotation.Value = Quaternion.Euler(0, 0, math.PI*Time.realtimeSinceStartup*12);
        });
    }
}
