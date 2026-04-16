using System;
using Modules.UI;
using UnityEngine;

namespace Game
{
    public class PlayerHealthPresenter : MonoBehaviour
    {
        private HealthView _healthView;
        private PlayerShip _playerShip;

        private void Awake()
        {
            _playerShip = GetComponent<PlayerShip>();
            _healthView = FindObjectOfType<HealthView>();
        }

        private void OnEnable()
        {
            _playerShip.OnHealthChanged += OnHealthChanged;
        }

        private void Start()
        {
            _healthView.SetHealth(_playerShip.CurrentHp, _playerShip.config.Health);
        }

        private void OnDisable()
        {
            _playerShip.OnHealthChanged -= OnHealthChanged;
        }
        
        private void OnHealthChanged(int health)
        {
            _healthView.SetHealth(health, _playerShip.config.Health);
        }
    }
}