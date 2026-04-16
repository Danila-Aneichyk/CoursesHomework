using UnityEngine;

namespace Game
{
    public class BulletDamage : MonoBehaviour
    {
        public int Value => _value;

        private int _value;

        public void Setup(int value)
        {
            _value = value;
        }
    }
}