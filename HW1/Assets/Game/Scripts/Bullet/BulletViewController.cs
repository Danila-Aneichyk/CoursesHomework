using System;
using UnityEngine;

namespace Game
{
    public class BulletViewController : MonoBehaviour
    {
        [SerializeField] private GameObject _blueVFX;
        [SerializeField] private GameObject _redVFX;
        [SerializeField] private BulletViewConfig _config;

        public void Setup(TeamType team)
        {
            SetupLayer(team);
            SetupVFX(team);
        }

        public void SpawnExplosion(Vector3 position)
        {
            Instantiate(
                _config.ExplosionVFX,
                position,
                _config.ExplosionVFX.transform.rotation);
        }

        private void SetupLayer(TeamType team)
        {
            gameObject.layer = team switch
            {
                TeamType.None => LayerMask.NameToLayer("Default"),
                TeamType.Player => LayerMask.NameToLayer("PlayerBullet"),
                TeamType.Enemy => LayerMask.NameToLayer("EnemyBullet"),
                _ => throw new ArgumentOutOfRangeException(nameof(team), team, null)
            };
        }

        private void SetupVFX(TeamType team)
        {
            if (team == TeamType.Player)
            {
                _blueVFX.SetActive(true);
                _redVFX.SetActive(false);
            }
            else
            {
                _blueVFX.SetActive(false);
                _redVFX.SetActive(true);
            }
        }
    }
}