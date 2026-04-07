using UnityEngine;

namespace Game
{
    public interface IMovable
    {
        public Vector2 Direction { get; }
        public float Speed { get; }
        
        public void Move(Vector2 direction, float speed);
    }
}