using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Battle
{
    public class Boss : MonoBehaviour
    {

        public float Healt = 100f;

        private Animator _animator;


        private void Awake()
        {
            _animator = GetComponent<Animator>();            
        }

        private void Start()
        {
            //TODO: Запус idle анимации или что то еще хз пока
        }

    }
}