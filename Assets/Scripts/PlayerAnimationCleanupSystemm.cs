using Unity.Entities;
using Unity.Transforms;
using UnityEngine;


[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct PlayerAnimationCleanupSystemm : ISystem
{
    // プレイヤーのアニメーション関連のデータを格納するEntityQuery
    private EntityQuery PlayerAnimationEntityQuery;


    public void OnCreate(ref SystemState state)
    {
        PlayerAnimationEntityQuery = SystemAPI.QueryBuilder()
            .WithAll<PlayerManagedData>() // PlayerManagedDataを持つエンティティ
            .WithNone<PlayerComponentData, LocalTransform>() // PlayerComponentDataとLocalTransformを持たないエンティティ
            .Build();

        // 作成したEntityQueryに基づいて、システムが更新されることを要求
        state.RequireForUpdate(PlayerAnimationEntityQuery);
    }

    public void OnUpdate(ref SystemState state)
    {
        // PlayerAnimationEntityQueryで得たエンティティを取得
        var playerAnimationEntity = PlayerAnimationEntityQuery.GetSingletonEntity();
        // PlayerManagedDataを取得
        var playerManagedData = PlayerAnimationEntityQuery.GetSingleton<PlayerManagedData>();

        // プレイヤーのアニメーション用のゲームオブジェクトを削除
        Object.Destroy(playerManagedData.Animator.gameObject);
        // エンティティからPlayerManagedDataコンポーネントを削除
        state.EntityManager.RemoveComponent<PlayerManagedData>(playerAnimationEntity);
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}
