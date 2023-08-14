using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Slime
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Custom/SlimeLevel", order = 51)]
    public class SlimeLevelType : ScriptableObject
    {
        [SerializeField] private SlimeSet _set;
        [SerializeField] private GameObject _enviroment;

        public SlimeSet SlimeSet => _set;
        public GameObject Enviroment => _enviroment;
    }
}
