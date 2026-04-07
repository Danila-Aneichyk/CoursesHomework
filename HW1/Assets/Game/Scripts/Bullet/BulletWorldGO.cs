using System;
using System.Collections.Generic;
using Modules.Utils;
using UnityEngine;

namespace Game
{
    // +
    public sealed class BulletWorldGO : MonoBehaviour
    {
        [SerializeField]
        private BulletData _prefab;

        [SerializeField]
        private Transform _container;

        [SerializeField]
        private BulletViewConfig _configView;

        [SerializeField]
        private TransformBounds _levelBounds;

        private readonly Stack<BulletData> _pool = new();
        private readonly List<BulletData> _bullets = new();

        private int _initialPoolSizeAmount = 10;

        private void Awake()
        {
            InstantiateInitialPull();
        }

        private void FixedUpdate()
        {
            IsBulletInBounds();
        }

        public void Spawn(Vector2 position, Vector2 direction, float speed, int damage, TeamType team)
        {
            BulletData bullet = GetBulletData();
            
            bullet.damage = damage;
            bullet.team = team;
            
            BulletLinearMovement movement = bullet.GetComponent<BulletLinearMovement>();
            movement.Setup(direction, speed);

            bullet.transform.position = position;
            bullet.transform.rotation =
                Quaternion.LookRotation(direction, Vector3.forward);

            bullet.GetComponent<BulletViewController>()
                .Setup(team);

            bullet.OnTriggerEntered += OnTriggerEntered;
            
            _bullets.Add(bullet);
        }

        private void OnTriggerEntered(BulletData bullet, Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable damageable))
                return;

            if (damageable.Team == bullet.team)
                return;

            damageable.ApplyDamage(bullet.damage);

            ReturnToPool(bullet);
            
            bullet.GetComponent<BulletViewController>()
                .SpawnExplosion(bullet.transform.position);
        }

        private void ReturnToPool(BulletData bullet)
        {
            bullet.OnTriggerEntered -= this.OnTriggerEntered;

            _bullets.Remove(bullet);

            bullet.gameObject.SetActive(false);
            _pool.Push(bullet);
        }

        private void InstantiateInitialPull()
        {
            for (var i = 0; i < _initialPoolSizeAmount; i++)
            {
                BulletData bullet = Instantiate(_prefab, _container);
                bullet.gameObject.SetActive(false);
                _pool.Push(bullet);
            }
        }

        private void IsBulletInBounds()
        {
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                BulletData bullet = _bullets[i];

                if (!_levelBounds.InBounds(bullet.transform.position))
                {
                    ReturnToPool(bullet);
                }
            }
        }

        private BulletData GetBulletData()
        {
            if (_pool.TryPop(out BulletData bullet))
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }
    
            return Instantiate(_prefab, _container);
        }
    }
}