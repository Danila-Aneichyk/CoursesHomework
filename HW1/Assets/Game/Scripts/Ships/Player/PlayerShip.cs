using Modules.Utils;
using UnityEngine;

namespace Game
{
    // +
    public sealed class PlayerShip : ShipController
    {
        [SerializeField]
        private TransformBounds _playerArea;
        
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                this.Fire();

            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");
            this.moveDirection = new Vector2(dx, dy);

            if (HealthController.CurrentHp > 0)
            {
                _motor.MoveStep(this.moveDirection);
            }
        }

        protected void LateUpdate()
        {
            this.transform.position = _playerArea.ClampInBounds(this.transform.position);
        }
    }
}