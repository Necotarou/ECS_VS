using Unity.Burst;
using Unity.Entities;


public partial class InputSysytem : SystemBase
{
    // 入力アクションを格納する変数
    private InputSystem_Actions _inputSystem_Actions;


    protected override void OnCreate()
    {
        // 新しいInputSystem_Actionsのインスタンスを作成
        _inputSystem_Actions = new InputSystem_Actions();
        // 入力アクションを有効化（入力を受け取れる状態にする）
        _inputSystem_Actions.Enable();
        // PlayerInputSystemをシステムとして作成
        var playerInputSystem = EntityManager.World.CreateSystemManaged<PlayerInputSystem>();
        // 作成したPlayerInputSystemに対して、入力アクションをセット
        playerInputSystem.SetPlayerActions(_inputSystem_Actions.Player);
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnDestroy()
    {

    }
}
