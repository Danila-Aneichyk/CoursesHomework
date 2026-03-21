using UnityEngine;

namespace Game
{
    public class BulletLinearMovement : MonoBehaviour, IMovable
    {
        private BulletData _bulletData;

        private void Awake()
        {
            _bulletData = gameObject.GetComponent<BulletData>();
        }

        private void FixedUpdate()
        {
            Move();
        }

        public void Move()
        {
            Vector3 moveStep = _bulletData.direction * _bulletData.speed * Time.fixedDeltaTime;
            _bulletData.transform.position += moveStep;
        }
    }
}