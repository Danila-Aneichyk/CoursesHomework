using UnityEngine;

namespace Game
{
    public sealed class BulletLinearMovement : MonoBehaviour
    {
        public Vector2 Direction => _direction;
        public float Speed => _speed;

        private Vector2 _direction;
        private float _speed;

        public void Setup(Vector2 direction, float speed)
        {
            _direction = direction.normalized;
            _speed = speed;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, _direction);
        }

        private void FixedUpdate()
        {
            Vector3 moveStep = (Vector3)(_direction * _speed * Time.fixedDeltaTime);
            transform.position += moveStep;
        }
    }
}