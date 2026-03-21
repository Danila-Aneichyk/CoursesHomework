using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class ShipControllerView : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Transform _viewTransform;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ShipControllerViewConfig _config;
        [SerializeField] private ParticleSystem _fireVFX;
        [SerializeField] private AudioClip _fireSFX;
        [SerializeField] private AudioClip _damageSFX;

        private ShipController _controller;
        private Material _material;
        private Tweener _damageAnimation;

        private void Awake()
        {
            _controller = GetComponent<ShipController>();

            _material = new Material(_config.MaterialPrefab);
            _renderer.material = _material;
        }

        private void OnEnable()
        {
            _controller.OnHealthChanged += OnHealthChanged;
            _controller.OnDead += OnDead;
            _controller.OnFire += OnFire;
        }

        private void OnDisable()
        {
            _controller.OnHealthChanged -= OnHealthChanged;
            _controller.OnDead -= OnDead;
            _controller.OnFire -= OnFire;
        }

        private void LateUpdate()
        {
            AnimateMovement(Time.deltaTime);
        }

        private void AnimateMovement(float deltaTime)
        {
            Vector3 moveDirection = _controller.transform.forward;

            Vector3 shipAngles = _viewTransform.localEulerAngles;
            shipAngles.x = _config.MoveRotationAngle * moveDirection.y;
            shipAngles.y = _config.MoveRotationAngle / 2 * moveDirection.x * -1f;

            Quaternion shipRotation = Quaternion.Euler(shipAngles);
            float t = _config.MoveSpeed * deltaTime;

            _viewTransform.localRotation =
                Quaternion.Lerp(_viewTransform.localRotation, shipRotation, t);
        }

        private void OnFire(ShipController controller)
        {
            if (_fireSFX)
                _audioSource.PlayOneShot(_fireSFX);

            if (_fireVFX)
                _fireVFX.Play();
        }

        private void OnHealthChanged(int health)
        {
            if (health > 0)
                AnimateDamage();
        }

        private void OnDead()
        {
            Instantiate(
                _config.DestroyEffectPrefab,
                _viewTransform.position,
                _config.DestroyEffectPrefab.transform.rotation);
        }

        private void AnimateDamage()
        {
            if (_damageAnimation != null && _damageAnimation.IsActive())
                _damageAnimation.Kill();

            _damageAnimation = DOVirtual.Float(
                0f,
                1f,
                _config.HitDuration,
                progress => _material?.SetFloat(
                    _config.HitPropertyName,
                    _config.HitAnimationCurve.Evaluate(progress))
            ).SetLink(_renderer.gameObject);

            if (_damageSFX)
                _audioSource.PlayOneShot(_damageSFX);
        }
    }
}