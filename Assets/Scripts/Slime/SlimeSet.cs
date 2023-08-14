using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Slime
{
    
    [CreateAssetMenu(fileName = "NewSlimeSet", menuName = "Custom/SlimeSet", order = 51)]
    public class SlimeSet : ScriptableObject
    {
        [SerializeField] private List<Slime> _slimes;

        public int Size => _slimes.Count;

        private void Awake()
        {
            for (var i = 0; i < _slimes.Count; i++)
            {
                _slimes[i].SetID(i);
            }
        }

        public Slime GetAnimalTemplate(int index)
        {
            return _slimes[index];
        }
    }
}
