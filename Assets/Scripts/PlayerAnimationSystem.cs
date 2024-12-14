using Unity.Entities;
using Unity.Transforms;


// シミュレーションシステムグループ内で最初に更新されるように設定
[UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
public partial struct PlayerAnimationSystem : ISystem
{
    // プレイヤーのエンティティを対象にするEntityQuery
    private EntityQuery PlayerEntityQuery;


    public void OnCreate(ref SystemState state)
    {
        PlayerEntityQuery = SystemAPI.QueryBuilder()
            .WithAll<LocalTransform>() // LocalTransformコンポーネントを持つエンティティ
            .WithAll<PlayerManagedData>() // PlayerManagedDataコンポーネントを持つエンティティ
            .Build();

        // 作成したEntityQueryに基づいて、システムが更新されることを要求
        state.RequireForUpdate(PlayerEntityQuery);
    }

    public void OnUpdate(ref SystemState state)
    {
        // プレイヤーのLocalTransformとPlayerManagedDataを取得
        var playerLocalTransform = PlayerEntityQuery.GetSingleton<LocalTransform>();
        var playerAnimationData = PlayerEntityQuery.GetSingleton<PlayerManagedData>();

        // プレイヤーのアニメーショントランスフォームを取得
        var transform = playerAnimationData.Animator.transform;
        transform.position = playerLocalTransform.Position;
        transform.rotation = playerLocalTransform.Rotation;
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}
