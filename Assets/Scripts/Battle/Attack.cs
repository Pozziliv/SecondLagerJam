using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Battle

{
    public class SlimeAttack : MonoBehaviour
    {
        
        private Boss _boss;

        private void Start ()
        {
            _boss = FindAnyObjectByType<Boss>();
        }
        
        public void GetDamage()
        {
            
        }
        
    }
}