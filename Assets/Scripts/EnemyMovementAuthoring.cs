using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


// 敵の移動データを保持するIComponentData構造体
public struct EnemyMovementData : IComponentData
{
    public float Speed; // 敵の移動速度
    public float3 TargetPosition;  // 敵が向かうターゲットの位置
}

public class EnemyMovementAuthoring : MonoBehaviour
{
    [SerializeField] private float Speed = 1; // 移動速度
    [SerializeField] private Vector3 TargetPosition = Vector3.zero; // ターゲット位置

    class Baker : Baker<EnemyMovementAuthoring>
    {
        public override void Bake(EnemyMovementAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyMovementData
            {
                Speed = authoring.Speed,
                TargetPosition = authoring.TargetPosition
            });
        }
    }
}
