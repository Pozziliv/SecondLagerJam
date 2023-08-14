using Assets.Scripts.Slime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Battle
{


    public class BattleSystem : MonoBehaviour
    {

        //TODO: Получить список списков контейнеров игроков или тп
        //TODO: Прифаб и доступ к босу 
        //TODO: ТОчка спавна боса

        [SerializeField] private Transform _bossSpawnPoint;
        [SerializeField] private GameObject _bossPrefab;


        private DamageCounter _damageCounter;
        private List<Aviary> _aviaryList = new List<Aviary>();
        private Boss _boss;
        


        private void Start()
        {
            
            _boss = _bossPrefab.GetComponent<Boss>();

        }

        public void StartBattle()
        {           
            StartCoroutine(SetupBattle());
        }

        IEnumerator SetupBattle()
        {
            
            //TODO: Инстантиировать боса на нужную точку
            Instantiate(_bossPrefab, _bossSpawnPoint.position, _bossSpawnPoint.rotation);
            _damageCounter = FindAnyObjectByType<DamageCounter>();
            _aviaryList = _damageCounter._aviaryList;
        

            yield return new WaitForSeconds(0.2f);

            StartCoroutine(PlayerAttack());
        }

        IEnumerator PlayerAttack()
        {
            // Проверка жив ли босс           
            if (_boss.Health <= 0)
            {
                Win();
                yield return null;
            }

            yield return new WaitForSeconds(2f);

            // Удар по боссу
            foreach (var aviary in _aviaryList)
            {
                aviary.GetDamageAnimation();
            }

            _boss.Health -= _damageCounter.Damage;
            

            yield return new WaitForSeconds(0.3f);

            if (_boss.Health <= 0)
            {
                Win();
                yield return null;
            }
            else
            {
                StartCoroutine(BossAttack());
                yield return null;
            }



            // Проверка жив ли босс если да то передать ход боссу иначе вызов экрана победы

        }

        IEnumerator BossAttack()
        {

            print("Босс атакует древних русов");

            //Проверка живы ли все пачки

            //Выбор по какой пачке бьет босс

            //Если пачка мертва полностью нанести удар иначе удар по следующей пачке

            yield return new WaitForSeconds(0.3f);

            //Проверка живы ли все пачки
        }

        public void Lost()
        {
            print("Древние русы пали в битве с ящером");
        }

        public void Win()
        {
            print("Ящер помер, Древние русы победили");
        }
    }
}