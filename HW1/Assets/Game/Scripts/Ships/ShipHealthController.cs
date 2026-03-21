using System;
using UnityEngine;

namespace Game
{
    public class ShipHealthController : MonoBehaviour, IHealth
    {
        [SerializeField] private ShipControllerSO _shipControllerSO; 
        [SerializeField] private int _startHp;
        [SerializeField] private int _maxHp;

        public event Action<int> OnChanged;
        public event Action OnApplyDamage;
        public event Action OnApplyHeal;
        public event Action PlayerDead;

        public int CurrentHp { get; private set; }
        public int MaxHp => _maxHp;

        private void Awake()
        {
            SetInitialHealth();
        }

        private void SetInitialHealth()
        {
            _startHp = _shipControllerSO.Health;
            _maxHp = _shipControllerSO.Health;
            CurrentHp = _startHp;
            OnChanged?.Invoke(CurrentHp);
        }

        public void ResetHealth()
        {
            CurrentHp = _startHp;
            OnChanged?.Invoke(CurrentHp);
        }

        public void ApplyDamage(int damage)
        {
            if (damage <= 0 || CurrentHp <= 0)
                return;

            CurrentHp = Mathf.Max(0, CurrentHp - damage);
            OnChanged?.Invoke(CurrentHp);
            OnApplyDamage?.Invoke();

            if (CurrentHp <= 0)
            {
                PlayerDead?.Invoke();
            }
        }

        public void ApplyHeal(int heal)
        {
            if (heal <= 0)
                return;

            CurrentHp = Mathf.Min(_maxHp, CurrentHp + heal);
            OnChanged?.Invoke(CurrentHp);
            OnApplyHeal?.Invoke();
        }
    }
}