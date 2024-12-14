using Unity.Entities;
using UnityEngine;


[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial struct PlayerComponentSystem : ISystem
{
    private EntityQuery PlayerEntityQuery;  // プレイヤーのEntityを検索するためのクエリ


    public void OnCreate(ref SystemState state)
    {
        PlayerEntityQuery = SystemAPI.QueryBuilder()
            .WithAll<PlayerComponentData>() // PlayerComponentDataが存在する
            .WithNone<PlayerManagedData>() // PlayerManagedDataが存在しない
            .Build();

        // クエリに一致するEntityが存在する場合のみシステムを更新する
        state.RequireForUpdate(PlayerEntityQuery);
    }

    public void OnUpdate(ref SystemState state)
    {
        // PlayerComponentDataを持つEntityを取得
        var playerEntity = PlayerEntityQuery.GetSingletonEntity();
        var playerComponentData = PlayerEntityQuery.GetSingleton<PlayerComponentData>();

        // PlayerComponentDataに設定されたプレイヤーのGameObjectをインスタンス化
        var playerComponentGameObject = Object.Instantiate(playerComponentData.PlayerComponentObject);

        // EntityにPlayerManagedDataコンポーネントを追加し、GameObject、Transform、Animatorを設定
        state.EntityManager.AddComponentObject(playerEntity, new PlayerManagedData
        {
            GameObject = playerComponentGameObject, // インスタンス化したGameObject
            Transform = playerComponentGameObject.transform, // Transformコンポーネント
            Animator = playerComponentGameObject.GetComponent<Animator>() // Animatorコンポーネント
        });
    }

    public void OnDestroy(ref SystemState state)
    {
        
    }
}
