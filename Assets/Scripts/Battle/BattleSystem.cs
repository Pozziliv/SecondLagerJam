using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Battle
{


    public class BattleSystem : MonoBehaviour
    {

        //TODO: Получить список списков контейнеров игроков или тп
        //TODO: Прифаб и доступ к босу 
        //TODO: ТОчка спавна боса



        private void Start()
        {

            StartCoroutine(SetupBattle());
        }

        IEnumerator SetupBattle()
        {
            //TODO: Инстантиировать боса на нужную точку

            yield return new WaitForSeconds(0.2f);

            StartCoroutine(PlayerAttack());
        }

        IEnumerator PlayerAttack()
        {
            // Проверка жив ли босс
            // Удар по боссу

            yield return new WaitForSeconds(0.3f);

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