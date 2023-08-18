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

        [SerializeField] private GameObject _explosiveParticles;
        [SerializeField] private GameObject _soulsParticles;
        [SerializeField] private GameObject _impactParticles;
        [SerializeField] private GameObject _root;
        [SerializeField] private GameObject _healthUI;

        [HideInInspector]
        public Animator _animator;

        [SerializeField] private BossElements _element;

        public int Health => _health;
        public BossElements Element => _element;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _maxHealth = (int)(_maxHealth * ((DB.GetLevel() - 1) % 4 + 1)*1.75f);
            _health = _maxHealth;
            _imgHeathMultiplyer = 1 / (float)_maxHealth;
        }
        
        public bool TakeDamage(int value)
        {
            _health -= value;
            _healthImg.fillAmount = 1 - (_maxHealth - _health) * _imgHeathMultiplyer;
            return _health <= 0;
        }

        public void Attack()
        {
            FindAnyObjectByType<AudioManager>().Play("bossHit1");
            _animator.SetTrigger("Attack");
        }

        public IEnumerator Die()
        {
            FindAnyObjectByType<AudioManager>().Play("bossDie");
            _explosiveParticles.SetActive(true);
            _healthUI.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            _root.SetActive(false);
            yield return new WaitForSeconds(0.4f);
            _soulsParticles.SetActive(true);
        }

        public IEnumerator AdvertisementDie()
        {
            FindAnyObjectByType<AudioManager>().Play("bossDie");
            _healthUI.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            _root.SetActive(false);
            yield return new WaitForSeconds(0.1f);
            _soulsParticles.SetActive(true);
        }

        public void AdvertisementAttack()
        {
            _impactParticles.SetActive(true);
        }
    }
}