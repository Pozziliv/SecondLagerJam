using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Battle
{
    public class Boss : MonoBehaviour
    {

        [SerializeField] private int _maxHealth = 100;
        private int _health;
        [SerializeField] private Image _healthImg;
        private float _imgHeathMultiplyer;

        [HideInInspector]
        public Animator _animator;

        [SerializeField] private BossElements _element;

        public int Health => _health;
        public BossElements Element => _element;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _maxHealth *= (int)(DB.GetLevel() * 0.75f);
            _health = _maxHealth;
            _imgHeathMultiplyer = 1 / (float)_maxHealth;
        }
        
        public bool TakeDamage(float value)
        {
            _health -= (int)value;
            Debug.Log(_health);
            _healthImg.fillAmount = 1 - (_maxHealth - _health) * _imgHeathMultiplyer;
            return _health <= 0;
        }

        public void Attack()
        {
            _animator.SetTrigger("Attack");
        }
    }
}