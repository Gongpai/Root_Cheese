using GDD.ObjectPool;
using Unity.VisualScripting;
using UnityEngine;

namespace GDD
{
    public class ShootingTargetObjectPool : ObjectPoolBuilder
    {
        public override GameObjectPool CreatePooledItem()
        {
            GameObjectPool objectPool = new GameObject("Shooting Target").AddComponent<Target_Point>();
            objectPool.objectPool = Pool;
            objectPool.AddComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>("Images/Weapon/ShootingTarget");
            objectPool.GetComponent<SpriteRenderer>().color = Color.red;
            objectPool.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            objectPool.transform.localScale = Vector3.one * 0.1f;
            return objectPool;
        }
    }
}