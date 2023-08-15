using Assets.Scripts.Battle;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Slimes : MonoBehaviour
{
    private Queue<Animal> animals = new Queue<Animal>();
    private List<Aviary> _aviaryList = new List<Aviary>();

    [SerializeField] private Transform _bossPos;

    [SerializeField] private int _rowLength = 10;

    private Boss _boss;

    private void Awake()
    {
        _aviaryList = GameObject.FindObjectsOfType<Aviary>().ToList();
    }

    public void SetBoss(Boss boss)
    {
        _boss = boss;
    }

    public void GetAllSlimes()
    {
        foreach (var aviary in _aviaryList)
            foreach(var animal in aviary.GetAnimals())
                animals.Enqueue(animal);
    }

    public void MoveToBattlePos()
    {
        int columnNumber = 0;
        int rowIndex = 0;
        foreach (var animal in animals)
        {
            rowIndex++;
            if (rowIndex == _rowLength + 1)
            {
                rowIndex = 0;
                columnNumber++;
            }
            Vector3 targetPos = transform.position + new Vector3(-5 * 3f + rowIndex * 3f, 0f, columnNumber * 3f);
            animal.MoveToBattlePos(1f, targetPos);
        }
    }

    public IEnumerator Attack()
    {
        foreach (var animal in animals)
        {
            animal.Attack(0.1f, _bossPos.position);
            yield return new WaitForSeconds(0.05f);
            _boss.TakeDamage((10 /*Base Damage*/ + animal.Level * 5) *
                (((int)animal.Element == (int)_boss.Element || (int)animal.Element == 0) ? 1 :
                ((int)animal.Element % 3 + 1 == (int)_boss.Element) ? 2 : 0.5f));
            yield return new WaitForSeconds(0.05f);
        }
    }
}
