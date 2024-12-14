using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;


[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct EnemyMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // PlayerDataコンポーネントが存在する場合のみシステムを更新する
        state.RequireForUpdate<PlayerData>();        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // プレイヤーのEntityを取得
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerData>();
        // プレイヤーのEntityが存在しない場合は処理を終了
        if (!SystemAPI.Exists(playerEntity)) return;

        // プレイヤーのTransform情報を取得
        var playerTrasnform = SystemAPI.GetComponentRO<LocalTransform>(playerEntity);
        float3 playerposition = playerTrasnform.ValueRO.Position;

        // 敵の移動データとTransformデータを取得してループ処理
        foreach (var (enemyMovement, localTransform) in SystemAPI.Query<RefRW<EnemyMovementData>, RefRW<LocalTransform>>())
        {
            // 敵のターゲット位置をプレイヤーの位置に設定
            enemyMovement.ValueRW.TargetPosition = playerposition;

            // 敵の現在位置を取得
            float3 enemyPosition = localTransform.ValueRO.Position;
            // プレイヤーへの方向を計算し、正規化する
            float3 direction = math.normalize(enemyMovement.ValueRO.TargetPosition - enemyPosition);

            // 敵の位置を更新し、プレイヤーに向かって移動する
            localTransform.ValueRW.Position += direction * enemyMovement.ValueRO.Speed * SystemAPI.Time.DeltaTime;
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
