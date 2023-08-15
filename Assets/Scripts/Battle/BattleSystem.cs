using Assets.Scripts.Slime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Battle
{


    public class BattleSystem : MonoBehaviour
    {

        

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
            

            yield return new WaitForSeconds(1f);

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

            var bossAttackDamage = _boss.GetDamage();

            //Проверка живы ли все пачки
            int aviaryCounts = 0;
            foreach (var aviary in _aviaryList)
            { 
                if(aviary._animals.Count > 0)
                {
                    aviaryCounts++;
                }
            }

            if( aviaryCounts == 0 ) 
            {
                Lost();
                yield return null;
            }

            

            yield return new WaitForSeconds(1f);

            //Выбор по какой пачке бьет босс

            foreach (var aviary in _aviaryList)
            {
                foreach (var animal in aviary._animals)
                {
                    if(bossAttackDamage > 0)
                    {
                        int temp = (int)animal._stats.CurrentHealth;
                        animal._stats.CurrentHealth -= bossAttackDamage;
                        bossAttackDamage -= temp;

                        if(animal._stats.CurrentHealth <= 0)
                        {
                            //TODO: не могу из ебучего стека удалить пидараса
                            
                            Destroy(animal.gameObject);
                            print("Помер");
                        }
                    } 
                }
            }

            yield return new WaitForSeconds(1f);

            

            int aviaryCounts2 = 0;
            foreach (var aviary in _aviaryList)
            {
                if (aviary._animals.Count > 0)
                {
                    aviaryCounts2++;
                }
            }

            if (aviaryCounts2 == 0)
            {
                Lost();
                yield return null;
            }
            else
            {
                StartCoroutine(PlayerAttack());
            }
           

            
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