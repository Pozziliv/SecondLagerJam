using Assets.Scripts.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Custom/Level", order = 51)]
public class LevelType : ScriptableObject
{
    [SerializeField] private AnimalSet _set;
    [SerializeField] private GameObject _enviroment;
    [SerializeField] private Boss _bossPrefab;

    public Boss BossPrefab => _bossPrefab;
    public AnimalSet AnimalSet => _set;
    public GameObject Enviroment => _enviroment;
}
