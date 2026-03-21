using Modules.UI;
using Modules.Utils;
using UnityEngine;

namespace Game
{
    public class PlayerShipView : MonoBehaviour
    {
        [SerializeField]
        private CameraShaker _cameraShaker;
        private PlayerShip _playerShip;
        
        [Header("UI")]
        [SerializeField]
        private GameOverView _gameOverView;
        [SerializeField]
        private HealthView _healthView;

        private void Awake()
        {
            _playerShip = GetComponent<PlayerShip>();
            _gameOverView = FindObjectOfType<GameOverView>();
            _healthView = FindObjectOfType<HealthView>();
            _cameraShaker = FindObjectOfType<CameraShaker>();
        }

        private void OnEnable()
        {
            _playerShip.OnHealthChanged += OnHealthChanged;
            _playerShip.OnDead += OnDead;
        }

        private void OnDisable()
        {
            _playerShip.OnHealthChanged -= OnHealthChanged;
            _playerShip.OnDead -= OnDead;
        }
        
        private void Start()
        {
            if (_playerShip == null)
                return;

            _healthView.SetHealth(_playerShip.CurrentHp, _playerShip.config.Health);
        }

        private void OnHealthChanged(int health)
        {
            _healthView.SetHealth(health, _playerShip.config.Health);
            _cameraShaker.Shake();
        }

        private void OnDead()
        {
            _gameOverView.Show();
        }
    }
}