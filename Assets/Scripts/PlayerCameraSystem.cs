using Unity.Entities;
using UnityEngine;
using Unity.Cinemachine;


[UpdateInGroup(typeof(SimulationSystemGroup))]
public partial class PlayerCameraSystem : SystemBase
{
    private EntityQuery PlayerEntityQuery; // プレイヤーのEntityを検索するためのクエリ
    private CinemachineCamera CinemachineCamera;  // Cinemachineのカメラ参照


    protected override void OnCreate()
    {
        // PlayerManagedDataコンポーネントを持つEntityを検索するクエリを作成
        PlayerEntityQuery = SystemAPI.QueryBuilder()
            .WithAll<PlayerManagedData>()
            .Build();

        // クエリに一致するEntityが存在する場合のみシステムを更新する
        RequireForUpdate(PlayerEntityQuery);
    }

    protected override void OnUpdate()
    {
        // 既にCinemachineCameraが設定されている場合は何もしない
        if (CinemachineCamera != null) return;

        // タグが "CinemachineCamera" のGameObjectを探し、CinemachineCameraコンポーネントを取得
        CinemachineCamera = GameObject.FindGameObjectWithTag("CinemachineCamera").GetComponent<CinemachineCamera>();
        // プレイヤーにアタッチされるコンポーネント群を取得
        var playerManagedData = PlayerEntityQuery.GetSingleton<PlayerManagedData>();

        // CinemachineカメラにプレイヤーのTransformを設定し、カメラがプレイヤーを追従するようにする
        CinemachineCamera.Follow = playerManagedData.Transform;
    }

    protected override void OnDestroy()
    {

    }
}
