using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Battle
{
    public class Boss : MonoBehaviour
    {

        [SerializeField] private int _baseDamage = 50;
        
        public float Health = 100f;
        public float MaxHealth = 100f;

        public BossElements _element;

        

        private Animator _animator;

        
        public int GetDamage()
        {
            //TODO: Расчет домага хз
            return _baseDamage;
        }

        
        private void Awake()
        {
            _animator = GetComponent<Animator>();            
        }

        
        //TODO: Поскольку в этом ебаном аниматоре все ебано сделано походу нада сделать отдельно логику переключения анимацие ебучих
    }
}