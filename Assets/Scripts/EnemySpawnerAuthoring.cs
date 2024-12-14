using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


// 敵のスポーンに関するデータを保持するIComponentData構造体
public struct EnemySpawnerData : IComponentData
{
    public Entity Prefab; // スポーンする敵のプレハブ
    public int SpawnCount; // スポーンする敵の数
    public float MinDistance; // 敵がスポーンする最小距離
    public float MaxDistance; // 敵がスポーンする最大距離
    public uint RandomSeed; // ランダムシード（敵のスポーン位置などを決定するため
    public float3 SpawnOriginPosition; // 敵がスポーンする原点位置
}

public class EnemySpawnerAuthoring : MonoBehaviour
{
    [SerializeField] public GameObject Prefab = null; // スポーンする敵のプレハブ
    [SerializeField] public int SpawnCount = 2000; // スポーンする敵の数
    [SerializeField] public float MinDistance = 3; // 最小スポーン距離
    [SerializeField] public float MaxDistance = 10; // 最大スポーン距離
    [SerializeField] public uint RandomSeed = 100; // ランダムシード
    [SerializeField] public float3 SpawnOriginPosition = new float3(0, 0, -0.01f); // スポーン原点位置

    class Baker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new EnemySpawnerData()
            {
                Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                SpawnCount = authoring.SpawnCount,
                MinDistance = authoring.MinDistance,
                MaxDistance = authoring.MaxDistance,
                RandomSeed = authoring.RandomSeed,
                SpawnOriginPosition = authoring.SpawnOriginPosition,
            });
        }
    }
}
