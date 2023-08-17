using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class BattleSystem : MonoBehaviour
    {
        
        private Boss _boss;
        private Boss _spawnedBoss;
        [SerializeField] private Transform _bossSpawnPoint;

        [SerializeField] private Game _game;
        [SerializeField] private Slimes _slimes;

        public event Action OnLose;
        public event Action OnWin;

        public BossElements Element => _boss.Element;

        private void OnEnable()
        {
            _game.LevelStarted += ChangeBoss;
        }

        private void OnDisable()
        {
            _game.LevelStarted -= ChangeBoss;
        }

        private void ChangeBoss(int level, LevelType levelType)
        {
            _boss = levelType.BossPrefab;
        }

        public IEnumerator SetupBattle()
        {
            _spawnedBoss = Instantiate(_boss, _bossSpawnPoint.position, Quaternion.identity);

            _slimes.SetBoss(_spawnedBoss);
            _slimes.GetAllSlimes();

            yield return new WaitForSeconds(4f);

            _slimes.MoveToBattlePos();

            yield return new WaitForSeconds(1.5f);

            StartCoroutine(PlayerAttack());
        }

        IEnumerator PlayerAttack()
        {
            StartCoroutine(_slimes.Attack());

            Debug.Log(0.3f * _slimes.GetDamagableCount() + 0.2f);

            yield return new WaitForSeconds(0.3f * _slimes.GetDamagableCount() + 0.2f);

            if(_spawnedBoss.Health >= 0)
            {
                Debug.Log(_spawnedBoss.Health);
                StartCoroutine(BossAttack());
            }
            else
            {
                StartCoroutine(_spawnedBoss.Die());
                yield return new WaitForSeconds(1f);
                Win();
            }
        }

        IEnumerator BossAttack()
        {
            _spawnedBoss.Attack();

            yield return new WaitForSeconds(0.3f);

            _slimes.Die();

            Lose();
        }

        public IEnumerator AdvertisementAttack()
        {
            yield return new WaitForSeconds(1f);

            _spawnedBoss.AdvertisementAttack();

            yield return new WaitForSeconds(0.05f);

            _spawnedBoss.TakeDamage(10000f);

            if (_spawnedBoss.Health < 0)
            {
                StartCoroutine(_spawnedBoss.AdvertisementDie());
                yield return new WaitForSeconds(0.5f);
                Win();
            }
            else
            {
                Lose();
            }
        }

        public void Lose()
        {
            StopAllCoroutines();
            OnLose?.Invoke();
        }

        public void Win()
        {
            StopAllCoroutines();
            StartCoroutine(_game.FinishGame());
            OnWin?.Invoke();
        }
    }
}