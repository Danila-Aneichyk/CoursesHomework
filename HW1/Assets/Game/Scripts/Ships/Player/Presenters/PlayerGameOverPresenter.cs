using System;
using Modules.UI;
using UnityEngine;

namespace Game
{
    public class PlayerGameOverPresenter : MonoBehaviour
    {
        private PlayerShip _playerShip;
        private GameOverView _gameOverView;

        private void Awake()
        {
            _playerShip = GetComponent<PlayerShip>();
            _gameOverView = FindObjectOfType<GameOverView>();
        }
        
        private void OnEnable()
        {
            _playerShip.OnDead += OnDead;
        }
        
        private void OnDisable()
        {
            _playerShip.OnDead -= OnDead;
        }
        
        private void OnDead()
        {
            _gameOverView.Show();
        }
    }
}