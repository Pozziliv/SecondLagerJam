using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using RSG;
using UnityEngine.Analytics;
using Lean.Common;
using Assets.Scripts.Battle;

public class BuyTriesScreen : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private PopupText _levetText;
    [SerializeField] private PopupText _title;
    [SerializeField] private PopupText _restart;
    [SerializeField] private PopupText _ad;
    [SerializeField] private CanvasGroup _canvas;
    [SerializeField] private Tries _tries;
    [SerializeField] private Game _game;
    [SerializeField] private InGameInput _input;

    [SerializeField] private BattleSystem _battleSystem;

    [SerializeField] private AnimationCurve _cameraAnimation;
    [SerializeField] private Transform _finishPos;
    [SerializeField] private float _cameraSpeed = 1f;

    private IPromiseTimer _timer = new PromiseTimer();
    private Color _backColor;
    private int _level;

    public event UnityAction NextButtonClicked;

    public void Appear()
    {
        
        Color startColor = new Color(_backColor.r, _backColor.g, _backColor.b, 0);
        _background.color = startColor;
        
        float fadeDuration = 1f;
        _timer.WaitWhile(time =>
        {
            _background.color = Color.Lerp(startColor, _backColor, time.elapsedTime / fadeDuration);
            return time.elapsedTime < fadeDuration;
        });

        float delay = 0.2f;
        _levetText.Show();
        _timer.WaitFor(delay).Then(() =>
        {
            _title.Show();
            _timer.WaitFor(delay).Then(() =>
            {
                _timer.WaitFor(delay * 2).Then(() =>
                {
                    _ad.Show();
                    _restart.Show();
                });
            });
        });
        _canvas.alpha = 1;
        _input.IsON = false;
        _canvas.interactable = true;
        _canvas.blocksRaycasts = true;
    }

    private void GoBattle()
    {
        StartCoroutine(_battleSystem.SetupBattle());

        StartCoroutine(CameraMove());
        
    }

    private IEnumerator CameraMove()
    {
        float progress = 0f;

        var startPos = Camera.main.transform;

        while (progress < 1)
        {
            Camera.main.transform.position = Vector3.Lerp(startPos.position, _finishPos.position, _cameraAnimation.Evaluate(progress));
           
            Camera.main.transform.rotation = Quaternion.Lerp(startPos.rotation, _finishPos.rotation, _cameraAnimation.Evaluate(progress));
            progress += Time.fixedDeltaTime * _cameraSpeed;
            yield return new WaitForFixedUpdate();
        }
       
    }


    public void Hide()
    {
        StartCoroutine(HideSelf(1f));
        _input.IsON = true;
        _canvas.interactable = false;
        _canvas.blocksRaycasts = false;
    }

    public void HideThenStartAd()
    {
        Hide();
        _tries.StartAD();
    }

    private IEnumerator HideSelf(float showTime)
    {
        float timer = 0;
        while (timer <= showTime)
        {
            timer += Time.deltaTime;
            _canvas.alpha = Mathf.Lerp(1, 0, timer / showTime);
            yield return null;
        }
    }

    private void OnEnable()
    {
        _tries.TriesChanged += OnTriesChanged;
        _game.LevelStarted += OnLevelChanged;
        _game.LevelCompleted += OnLevelCompleted;
        _battleSystem.OnLose += Appear;
    }

    private void OnDisable()
    {
        _tries.TriesChanged -= OnTriesChanged;
        _game.LevelStarted -= OnLevelChanged;
        _game.LevelCompleted -= OnLevelCompleted;
        _battleSystem.OnLose -= Appear;
    }

    private void Awake()
    {
        _backColor = _background.color;
        _background.color = new Color(_backColor.r, _backColor.g, _backColor.b, 0);
        _canvas.interactable = false;
        _canvas.blocksRaycasts = false;
    }

    private void Update()
    {
        _timer.Update(Time.deltaTime);
    }

    private void OnTriesChanged(int tries)
    {
        if (tries == 0)
        {
            _input.IsON = false;
            _canvas.interactable = true;
            _canvas.blocksRaycasts = true;

            GoBattle();

        }
    }

    private void OnLevelChanged(int level, LevelType type)
    {
        _level = level;
    }

    private void OnLevelCompleted()
    {
        gameObject.SetActive(false);
    }
}
