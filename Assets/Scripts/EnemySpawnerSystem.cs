using Unity.Burst;
using Unity.Entities;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;


[UpdateInGroup(typeof(InitializationSystemGroup))]
partial struct EnemySpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // EnemySpawnerDataコンポーネントが存在するエンティティの更新を要求
        state.RequireForUpdate<EnemySpawnerData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // EnemySpawnerDataコンポーネントを取得
        var enemySpawnerData = SystemAPI.GetSingleton<EnemySpawnerData>();
        // プレハブを指定された数だけインスタンス化
        var instances = state.EntityManager.Instantiate(enemySpawnerData.Prefab, enemySpawnerData.SpawnCount, Allocator.Temp);
        // ランダム生成のためのRandomインスタンスを作成（シード値はEnemySpawnerDataから取得）
        var random = new Random(enemySpawnerData.RandomSeed);

        // スポーン位置の範囲と原点位置を取得
        float minDistance = enemySpawnerData.MinDistance;
        float maxDistance = enemySpawnerData.MaxDistance;
        float3 spawnOriginPosition = enemySpawnerData.SpawnOriginPosition;

        // スポーン原点のx, y位置を取得（zはそのままにする）
        float2 spawnPosition = new float2(spawnOriginPosition.x, spawnOriginPosition.y);

        // 敵エンティティインスタンスごとに処理を行う
        foreach (var entity in instances)
        {
            // インスタンスのTransformコンポーネントを取得
            var localTransform = SystemAPI.GetComponentRW<LocalTransform>(entity);

            // ランダムな角度（0～2π）を生成
            float angle = random.NextFloat(0, math.PI2);
            // 角度に基づいてランダムな方向（2Dベクトル）を生成
            float2 direction = new float2(math.cos(angle), math.sin(angle));
            // 最小距離と最大距離の範囲内でランダムな距離を計算
            float distance = minDistance + random.NextFloat(0, maxDistance - minDistance);

            // 新しいスポーン位置を計算（方向ベクトルに距離を掛けたものを加算
            float2 newPosition = spawnPosition + direction * distance;

            // 新しい位置をエンティティのTransformに設定
            localTransform.ValueRW = LocalTransform.FromPosition(newPosition.x, newPosition.y, spawnOriginPosition.z);
        }

        // スポーンが終了したらシステムを無効化
        state.Enabled = false;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
