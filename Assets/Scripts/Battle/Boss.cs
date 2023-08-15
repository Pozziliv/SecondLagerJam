using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Battle
{
    public class Boss : MonoBehaviour
    {

        public float Health = 100f;

        private Animator _animator;

        [SerializeField] private BossElements _element;
        public BossElements Element => _element;

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