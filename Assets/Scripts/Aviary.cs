using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Events;
using RSG;
using System;
using Assets.Scripts.Slime;
using Assets.Scripts.Battle;
using UnityEngine.SocialPlatforms.Impl;

public class Aviary : MonoBehaviour
{
    [SerializeField] private Door _door;
    [SerializeField] private float _movePerAnimal = 0.1f;
    [SerializeField] private ComboText _comboText;
    [SerializeField] private Image _comboImage;
    [SerializeField] private ParticleSystem _confetti;
    [SerializeField] private Game _game;
    [SerializeField] private BattleSystem _battleSystem;
    [SerializeField] private DamageCounter _damageCounter;

    private Queue<Animal>  _animals = new Queue<Animal>();
    private IPromiseTimer _promiseTimer = new PromiseTimer();
    private ComboContainer _comboContainer;

    public Vector3 DoorPosition => _door.transform.position;

    public ComboText ComboText => _comboText;
    public bool HasAnimals => _animals.Count > 0;
    public int AnimalID => _animals.Count > 0 ? _animals.Peek().ID : -100;

    public event UnityAction<Aviary> GotAnimal;
    public event UnityAction<List<Animal>> ReleasedAnimals;
    public event UnityAction<Aviary> Interacted;
    public event UnityAction NiceMove;
    public event UnityAction VeryNiceMove;
    public event UnityAction BadMove;

    public void OpenDoor() => _door.Open();

    public void PlayConfetti() => _confetti.Play();

    public void CloseDoor() => _door.Close();

    private void OnEnable()
    {
        /*NiceMove += OnNiceMove;
        VeryNiceMove += OnNiceMove;
        BadMove += OnBadMove;*/
    }

    private void OnDisable()
    {
        /*NiceMove -= OnNiceMove;
        /*VeryNiceMove -= OnNiceMove;
        BadMove -= OnBadMove;*/
    }

    private void OnNiceMove()
    {
        _comboContainer.AddStreak(transform.position);
    }

    private void OnBadMove()
    {
        _comboContainer.ResetStreak(transform.position);
    }

    public void Init(ComboContainer container)
    {
        _comboContainer = container;
    }
    
    public bool TryTakeGroup(List<Node> nodes)
    {
        List<Node> sortedNodes = nodes.OrderBy(item => Vector3.Distance(item.transform.position, transform.position)).ToList();
        List<Animal> newAnimals = new List<Animal>();
        for (int i = 0; i < sortedNodes.Count; i++)
        {
            Node node = sortedNodes[i];
            if (node.IsBusy)
                newAnimals.Add(node.Animal);

            node.Deselect();
            node.Clear();
        }
        OpenDoor();

        bool inOtherAviary = false;
        Aviary[] aviaries = FindObjectsOfType<Aviary>();
        foreach (var item in aviaries)
            if (item != this && item.HasAnimals && item.AnimalID == newAnimals[0].ID)
                inOtherAviary = true;
        bool sameAnimals = _animals.Count == 0 || newAnimals[0].ID == _animals.Peek().ID;
        StartCoroutine(AddAnimalsLoop(newAnimals, true));

        return sameAnimals;
    }

    private IEnumerator AddAnimalsLoop(List<Animal> newAnimals, bool sameAnimals)
    {
        MoveAnimals(newAnimals.Count * _movePerAnimal);
        float maxDelta = _movePerAnimal * (newAnimals.Count - 1);
        int i = 0;
        foreach (var animal in newAnimals)
        {
            _animals.Enqueue(animal);
            
            int sideDelta = _animals.Count % 2 == 0 ? 1 : -1;
            Vector3 position = transform.position - transform.forward * (2 + maxDelta - i * _movePerAnimal) + transform.right * sideDelta;
            animal.MoveToAviary(this, 0.5f, position);
            _promiseTimer.WaitFor(0.3f).Then(() =>
            {
                UpdateCounter(GetSameAnimalsInRowCount(), animal.CountColor, sameAnimals);
            });
            i++;

            yield return new WaitForSeconds(0.1f);
        }
        //TODO: ������ ������
        int damage = 0;
        foreach(var animal in newAnimals)
        {
            damage += (int)((10 /*Base Damage*/ + animal.Level * 5) *
                (((int)animal.Element == (int)_battleSystem.Element || (int)animal.Element == 0) ? 1 :
                ((int)animal.Element % 3 + 1 == (int)_battleSystem.Element) ? 2 : 0.5f));
        }
        _game.DoneScore(damage, this);
        _promiseTimer.WaitFor(0.4f).Then(() =>
        {
            _door.Close();
            ReactOnNewAnimals(newAnimals);
        });
    }

    private void MoveAnimals(float delta)
    {
        foreach (var item in _animals)
        {
            _promiseTimer.WaitFor(0.2f).Then(() =>
            {
                item.Go(item.transform.position + transform.forward * -delta, 0.2f);
            });
        }
    }

    private void UpdateCounter(int value, Color color, bool sameAnimals)
    {
        /*_comboImage.color = color;
        _comboText.QuickReset();
        _comboText.Increase(value);*/
        if (sameAnimals)
            GotAnimal?.Invoke(this);
    }

    private void ReactOnNewAnimals(List<Animal> newAnimals)
    {
        int newAnimalsID = newAnimals[0].ID;
        /*if (_animals.Where(item => item.ID == newAnimalsID).ToArray().Length == _animals.Count)
        {
            */
                string animation = "Jump";
                foreach (Animal animal in _animals)
                    animal.PlayAnimation(animation);
                if (newAnimals.Count > 4)
                    VeryNiceMove?.Invoke();
                else if (newAnimals.Count >= 1)
                    NiceMove?.Invoke();
            /*}
            else
            {
                foreach (Animal animal in _animals)
                    animal.PlayAnimation("Idle");

                int count = GetSameAnimalsInRowCount();
                BadMove?.Invoke();
                ReleaseAnimals(count);
            }*//*
        }*//*
        else
        {
            Aviary[] aviaries = FindObjectsOfType<Aviary>();
            bool canGet = true;
            *//*foreach (var item in aviaries)
                if (item != this && item.HasAnimals && item.AnimalID == newAnimalsID)
                    canGet = false;*/
            /*if (canGet)
            {*//*
                if (newAnimals.Count > 4)
                    VeryNiceMove?.Invoke();
                else if (newAnimals.Count >= 1)
                    NiceMove?.Invoke();
            *//*}
            else
            {
                foreach (Animal animal in _animals)
                    animal.PlayAnimation("Idle");
                Interacted?.Invoke(this);
                BadMove?.Invoke();
                int count = GetSameAnimalsInRowCount();
                ReleaseAnimals(count);
            }*//*
        }*/
        Interacted?.Invoke(this);
    }

    private int GetSameAnimalsInRowCount()
    {
        if (_animals.Count == 0)
            return 0;

        int count = 0;
        foreach (var item in _animals)
        {
            count++;
        }

        return count;
    }

    private void ReleaseAnimals(int count)
    {
        List<Animal> animals = new List<Animal>();
        for (int i = 0; i < count; i++)
        {
            Animal animal = _animals.Dequeue();
            animals.Add(animal);
        }

        MoveAnimals(-count * _movePerAnimal);

        OpenDoor();
        _promiseTimer.WaitFor(1f).Then(() =>
        {
            CloseDoor();
        });

        ReleasedAnimals?.Invoke(animals);
        UpdateCounter(GetSameAnimalsInRowCount(), _animals.Peek().CountColor, false);
    }

    private void Update()
    {
        _promiseTimer.Update(Time.deltaTime);
    }

    public Queue<Animal> GetAnimals()
    {
        return _animals;
    }

    internal int GetAnimalsDamage()
    {
        float damage = 0;
        foreach(var item in _animals)
        {
            damage += (10 /*Base Damage*/ + item.Level * 5) * 
                (((int)item.Element == (int)_battleSystem.Element || (int)item.Element == 0) ? 1 : 
                ((int)item.Element % 3 + 1 == (int)_battleSystem.Element) ? 2 : 0.5f);
        }
        return (int)damage;
    }
}
