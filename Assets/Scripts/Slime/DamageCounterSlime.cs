using RSG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Slime
{
    public class DamageCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;

        [HideInInspector]
        public List<Aviary> _aviaryList = new List<Aviary>();
        private int _damage = 0;

        public int Damage => _damage;

        private void Awake()
        {
            _aviaryList = GameObject.FindObjectsOfType<Aviary>().ToList();

            foreach (var aviary in _aviaryList)
            {
                _damage += aviary.GetAnimalsDamage();
            }
            _text.text = _damage.ToString();
        }

        public void UpdateDamage()
        {
            _damage = 0;
            foreach (var aviary in _aviaryList)
            {
                _damage += aviary.GetAnimalsDamage();
            }
            _text.text = _damage.ToString();
        }

        public int GetDamage()
        {
            return _damage;
        }
    }
}
