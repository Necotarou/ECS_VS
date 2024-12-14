using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;


[DisableAutoCreation]
public partial class PlayerInputSystem : SystemBase
{
    // プレイヤーのアクションを管理するInputSystemのアクションマップ
    private InputSystem_Actions.PlayerActions PlayerActions;


    // プレイヤーアクションを設定し、Moveアクションのイベントを登録する
    public void SetPlayerActions(InputSystem_Actions.PlayerActions playerActions)
    {
        PlayerActions = playerActions;
        // Moveアクションが実行されたときにOnMovePerformedを呼び出す
        PlayerActions.Move.performed += OnMovePerformed;
        // MoveアクションがキャンセルされたときにOnMoveCanceledを呼び出す
        PlayerActions.Move.canceled += OnMoveCanceled;
    }

    protected override void OnUpdate()
    {

    }

    // システムが破棄されるときにMoveアクションのイベント登録を解除する
    protected override void OnDestroy()
    {
        PlayerActions.Move.performed -= OnMovePerformed;
        PlayerActions.Move.canceled -= OnMoveCanceled;
    }


    // Moveアクションが実行されたときに呼び出されるメソッド
    private void OnMovePerformed(InputAction.CallbackContext callbackContext)
    {
        // 入力された方向を正規化して取得
        Vector2 movementDirection = math.normalize(callbackContext.ReadValue<Vector2>());
        // プレイヤーの移動処理を呼び出す
        PlayerMove(movementDirection);
    }

    private void OnMoveCanceled(InputAction.CallbackContext callbackContext)
    {
        // 移動方向をゼロにしてプレイヤーを停止させる
        PlayerMove(Vector2.zero);
    }

    // プレイヤーの移動方向を更新するメソッド
    private void PlayerMove(Vector2 direction)
    {
        // PlayerDataコンポーネントを持つシングルトンEntityを取得
        Entity entity = SystemAPI.GetSingletonEntity<PlayerData>();
        // PlayerDataのコンポーネントを読み書き可能な状態で取得
        var playerData = SystemAPI.GetComponentRW<PlayerData>(entity);
        // PlayerDataのDirectionフィールドに新しい移動方向を設定
        playerData.ValueRW.Direction = direction;
    }
}
