using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;


[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct PlayerSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // PlayerSpawnerコンポーネントが存在する場合のみシステムを更新する
        state.RequireForUpdate<PlayerSpawner>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // PlayerSpawnerのシングルトンコンポーネントを取得
        var playerSpawner = SystemAPI.GetSingleton<PlayerSpawner>();

        // プレハブのEntityをインスタンス化（スポーン）する
        var instance = state.EntityManager.Instantiate(playerSpawner.Prefab);

        // インスタンスのTransform情報を取得し、スポーン位置に設定する
        var localTransform = SystemAPI.GetComponent<LocalTransform>(instance);
        localTransform = LocalTransform.FromPosition(playerSpawner.SpawnPosition);

        // システムを無効化して、再び呼び出されないようにする（1回だけスポーンするため）
        state.Enabled = false;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
}
