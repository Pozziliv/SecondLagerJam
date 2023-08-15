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
            
            // Удар по боссу

            yield return new WaitForSeconds(0.3f * _slimes.GetDamagableCount());

            if(_spawnedBoss.Health > 0)
            {
                StartCoroutine(BossAttack());
            }
        }

        IEnumerator BossAttack()
        {
            _spawnedBoss.Attack();

            yield return new WaitForSeconds(0.3f);

            _slimes.Die();
        }

        public void Lost()
        {
            
        }

        public void Win()
        {

        }
    }
}