using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Battle
{
    public class Boss : MonoBehaviour
    {

        [SerializeField] private float _maxHealth = 100f;
        private float _health;
        [SerializeField] private Image _healthImg;
        private float _imgHeathMultiplyer;

        [HideInInspector]
        public Animator _animator;

        [SerializeField] private BossElements _element;
        public BossElements Element => _element;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _maxHealth *= DB.GetLevel() * 0.75f;
            _health = _maxHealth;
            _imgHeathMultiplyer = 1 / _maxHealth;
        }
        
        public void TakeDamage(float value)
        {
            _health -= value;
            _healthImg.fillAmount = 1 - (_maxHealth - _health) * _imgHeathMultiplyer;
        }
    }
}