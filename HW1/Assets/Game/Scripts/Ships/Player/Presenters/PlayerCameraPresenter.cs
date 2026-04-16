using Modules.Utils;
using UnityEngine;

namespace Game
{
    public class PlayerCameraPresenter : MonoBehaviour
    {
        private CameraShaker _cameraShaker;
        private PlayerShip _playerShip;
        
        private void Awake()
        {
            _playerShip = GetComponent<PlayerShip>();
            _cameraShaker = FindObjectOfType<CameraShaker>();
        }
        
        private void OnEnable()
        {
            _playerShip.OnHealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _playerShip.OnHealthChanged -= OnHealthChanged;
        }
        
        private void OnHealthChanged(int health)
        {
            _cameraShaker.Shake();
        }
    }
}