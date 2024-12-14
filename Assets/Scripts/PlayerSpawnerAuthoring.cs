using Unity.Entities;
using UnityEngine;


// プレイヤーをスポーンするためのデータを持つIComponentData構造体
public struct PlayerSpawner : IComponentData
{
    public Entity Prefab; // スポーンするプレハブのEntity
    public Vector3 SpawnPosition; // プレイヤーをスポーンする位置
}

public class PlayerSpawnerAuthoring : MonoBehaviour
{
    public GameObject Prefab = null; // スポーンするプレハブのGameObject
    public Vector3 SpawnPosition = Vector3.zero; // スポーンする位置（デフォルトは原点）


    class Baker : Baker<PlayerSpawnerAuthoring>
    {
        public override void Bake(PlayerSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerSpawner()
            {
                Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                SpawnPosition = authoring.SpawnPosition
            });
        }
    }
}
