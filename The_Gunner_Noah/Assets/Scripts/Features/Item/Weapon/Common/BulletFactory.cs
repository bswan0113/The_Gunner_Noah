using System.Collections.Generic;
using Features.Item.Weapon;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance;

    public List<Pool> pools;

    private Dictionary<GunType, Queue<Bullet>> poolDictionary;
    private Dictionary<GunType, Pool> poolInfoDictionary;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        poolDictionary = new Dictionary<GunType, Queue<Bullet>>();
        poolInfoDictionary = new Dictionary<GunType, Pool>();

        foreach (Pool pool in pools)
        {
            Queue<Bullet> objectPool = new Queue<Bullet>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                Bullet bullet = obj.GetComponent<Bullet>();
                objectPool.Enqueue(bullet);
                obj.transform.SetParent(this.transform);
            }

            poolDictionary.Add(pool.type, objectPool);
            poolInfoDictionary.Add(pool.type, pool);
        }
    }

    public Bullet GetBullet(GunType type)
    {
        if (!poolDictionary.ContainsKey(type))
        {
            return null;
        }

        Queue<Bullet> targetPool = poolDictionary[type];
        Bullet bullet;

        if (targetPool.Count > 0)
        {
            bullet = targetPool.Dequeue();
        }
        else
        {
            Pool poolInfo = poolInfoDictionary[type];
            GameObject obj = Instantiate(poolInfo.prefab);
            obj.transform.SetParent(this.transform);
            bullet = obj.GetComponent<Bullet>();
        }

        Pool pool = poolInfoDictionary[type];
        bullet.Setup(pool.damage, pool.speed, pool.range);
        bullet.gameObject.SetActive(true);

        return bullet;
    }

    public void ReturnBullet(Bullet bullet, GunType type)
    {
        if (!poolDictionary.ContainsKey(type))
        {
            Destroy(bullet.gameObject);
            return;
        }

        bullet.gameObject.SetActive(false);
        poolDictionary[type].Enqueue(bullet);
    }

    [System.Serializable]
    public class Pool
    {
        public GunType type;
        public GameObject prefab;
        public int size;
        public float range;
        public float damage;
        public float speed;
    }
}