using System;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public abstract class ShipController : MonoBehaviour, IDamageable
    {
        public event Action<int> OnHealthChanged;
        public event Action OnDead;
        public event Action<ShipController> OnFire;

        public ShipControllerSO config;

        [SerializeField] private ShipHealthController _shipHealthController;

        protected ShipHealthController HealthController => _shipHealthController;
        public int CurrentHp => _shipHealthController.CurrentHp;

        public TeamType Team { get; }

        [Header("Combat")]
        public Transform firePoint;
        public float bulletSpeed;
        public int bulletDamage;
        private float _fireTime;

        [Header("Movement")]
        [SerializeField]
        protected Motor _motor;

        protected Vector3 moveDirection;

        protected virtual void Awake()
        {
            _motor.SetSpeed(config.MoveSpeed);

            if (_shipHealthController == null)
                _shipHealthController = GetComponent<ShipHealthController>();
        }

        protected virtual void FixedUpdate() => _motor.FixedUpdate();

        protected void Fire()
        {
            float time = Time.time;
            if (time - _fireTime < config.FireCooldown || _shipHealthController.CurrentHp <= 0)
                return;

            OnFire?.Invoke(this);
            _fireTime = time;
        }

        public void NotifyAboutHealthChanged(int health)
        {
            OnHealthChanged?.Invoke(health);
        }

        public void NotifyAboutDead()
        {
            OnDead?.Invoke();
        }

        public void ApplyDamage(int damage)
        {
            if (damage <= 0)
                return;

            _shipHealthController.ApplyDamage(damage);
            NotifyAboutHealthChanged(_shipHealthController.CurrentHp);

            if (_shipHealthController.CurrentHp <= 0)
            {
                NotifyAboutDead();
                gameObject.SetActive(false);
            }
        }

        public void ResetHealth()
        {
            _shipHealthController.ResetHealth();
        }
    }
}