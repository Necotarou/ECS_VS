using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


// プレイヤーのデータ
public struct PlayerData : IComponentData
{
    public float Speed; // 移動速度 
    public float2 Direction; // 移動方向    
}

// プレイヤーにアタッチされるコンポーネント群のデータ
public class PlayerComponentData : IComponentData
{
    public GameObject PlayerComponentObject; // コンポーネント群をアタッチしたPrefab
}

// プレイヤーにアタッチされるコンポーネント群
public class PlayerManagedData : ICleanupComponentData
{
    public GameObject GameObject; 
    public Animator Animator;
    public Transform Transform;
}


public class PlayerAuthoring : MonoBehaviour
{
    [SerializeField] private float Speed = 0.8f; // 移動速度
    [SerializeField] private float2 Direction = float2.zero; // 移動方向
    [SerializeField] private GameObject PlayerComponentObject = null; // コンポーネント群をアタッチしたPrefab


    class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerData
            {
                Speed = authoring.Speed,
                Direction = authoring.Direction
            });

            AddComponentObject(entity, new PlayerComponentData
            {
                PlayerComponentObject = authoring.PlayerComponentObject
            });
        }
    }
}
