using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Battle
{


    public class BattleSystem : MonoBehaviour
    {
        private Boss _boss;
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
            Instantiate(_boss, _bossSpawnPoint.position, Quaternion.identity);

            _slimes.SetBoss(_boss);
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

            yield return new WaitForSeconds(0.1f);

            // Проверка жив ли босс если да то передать ход боссу иначе вызов экрана победы

        }

        IEnumerator BossAttack()
        {
            //Проверка живы ли все пачки

            //Выбор по какой пачке бьет босс

            //Если пачка мертва полностью нанести удар иначе удар по следующей пачке

            yield return new WaitForSeconds(0.3f);

            //Проверка живы ли все пачки
        }

        public void Lost()
        {
            
        }

        public void Win()
        {

        }
    }
}