using System.Collections.Generic;
using Modules.Utils;
using UnityEngine;

namespace Game
{
    // +
    public sealed class BulletWorldGO : MonoBehaviour
    {
        [SerializeField] private BulletPool _bulletPool;

        [SerializeField] private TransformBounds _levelBounds;
        
        private readonly List<BulletData> _activeBullets = new();

        private void FixedUpdate()
        {
            IsBulletInBounds();
        }

        public void Spawn(Vector2 position, Vector2 direction, float speed, int damage, TeamType team)
        {
            BulletData bullet = _bulletPool.GetBulletData();

            bullet.transform.position = position;
            bullet.team = team;

            BulletLinearMovement movement = bullet.GetComponent<BulletLinearMovement>();
            movement.Setup(direction, speed);

            BulletDamage bulletDamage = bullet.GetComponent<BulletDamage>();
            bulletDamage.Setup(damage);

            bullet.GetComponent<BulletViewController>().Setup(team);

            bullet.OnTriggerEntered += OnTriggerEntered;

            _activeBullets.Add(bullet);
        }

        private void OnTriggerEntered(BulletData bullet, Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable damageable))
                return;

            if (damageable.Team == bullet.team)
                return;

            BulletDamage bulletDamage = bullet.GetComponent<BulletDamage>();
            damageable.ApplyDamage(bulletDamage.Value);

            ReturnToPool(bullet);
            
            bullet.GetComponent<BulletViewController>()
                .SpawnExplosion(bullet.transform.position);
        }
        
        private void IsBulletInBounds()
        {
            for (int i = _activeBullets.Count - 1; i >= 0; i--)
            {
                BulletData bullet = _activeBullets[i];

                if (!_levelBounds.InBounds(bullet.transform.position))
                {
                    ReturnToPool(bullet);
                }
            }
        }
        
        private void ReturnToPool(BulletData bullet)
        {
            bullet.OnTriggerEntered -= OnTriggerEntered;

            _activeBullets.Remove(bullet);
            _bulletPool.Return(bullet);
        }
    }
}