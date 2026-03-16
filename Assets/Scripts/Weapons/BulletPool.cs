using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialPoolSize = 50;

    private Queue<Bullet> pool;

    private void Awake()
    {
        pool = new Queue<Bullet>();

        for (int i = 0; i < initialPoolSize; i++)
            CreateNewBullet();
    }

    private Bullet CreateNewBullet()
    {
        var obj = Instantiate(bulletPrefab, transform);
        obj.SetActive(false);

        var bullet = obj.GetComponent<Bullet>();
        bullet.SetPool(this);
        pool.Enqueue(bullet);
        return bullet;
    }

    public Bullet GetBullet()
    {
        if (pool.Count == 0)
            CreateNewBullet();
        return pool.Dequeue();
    }

    public void Return(Bullet bullet)
    {
        pool.Enqueue(bullet);
    }
}
