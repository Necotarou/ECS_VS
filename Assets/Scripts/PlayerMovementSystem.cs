using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


[UpdateInGroup(typeof(SimulationSystemGroup))]
partial struct PlayerMovementSystem : ISystem
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
        // PlayerDataを持つシングルトンEntityを取得
        var playerEntity = SystemAPI.GetSingletonEntity<PlayerData>();
        // 取得したEntityが存在しない場合は処理を中断
        if (!SystemAPI.Exists(playerEntity)) return;

        // プレイヤーのTransform（位置情報）を読み書き可能な状態で取得
        var playerTransform = SystemAPI.GetComponentRW<LocalTransform>(playerEntity);
        var playerPosition = playerTransform.ValueRW.Position;

        // プレイヤーのデータ（移動方向と速度）を取得
        var playerData = SystemAPI.GetComponent<PlayerData>(playerEntity);

        // 前フレームからの経過時間を取得
        float DeltaTime = SystemAPI.Time.DeltaTime;

        // 現在のプレイヤー位置を2D座標に変換
        float2 newPosition = new float2(playerPosition.x, playerPosition.y);
        // 移動方向、速度、および経過時間を基に新しい位置を計算
        newPosition += playerData.Direction * playerData.Speed * DeltaTime;
        // 計算した新しい位置を3D座標に戻し、Transformに適用
        playerTransform.ValueRW.Position = new float3(newPosition.x, newPosition.y, playerPosition.z);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
