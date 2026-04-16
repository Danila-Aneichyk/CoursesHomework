using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BulletPool : MonoBehaviour
    {
        [SerializeField]
        private BulletData _prefab;

        [SerializeField]
        private Transform _container;
        
        [SerializeField] private int _initialPoolSizeAmount = 10;
        
        private readonly Stack<BulletData> _pool = new();

        private void Awake()
        {
            InstantiateInitialPull();
        }

        public void InstantiateInitialPull()
        {
            for (var i = 0; i < _initialPoolSizeAmount; i++)
            {
                BulletData bullet = Instantiate(_prefab, _container);
                bullet.gameObject.SetActive(false);
                _pool.Push(bullet);
            }
        }
        
        public BulletData GetBulletData()
        {
            if (_pool.TryPop(out BulletData bullet))
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }

            BulletData newBullet = CreateBullet();
            newBullet.gameObject.SetActive(true);
            return newBullet;
        }
        
        private BulletData CreateBullet()
        {
            return Instantiate(_prefab, _container);
        }
        
        public void Return(BulletData bullet)
        {
            bullet.gameObject.SetActive(false);
            _pool.Push(bullet);
        }
    }
}