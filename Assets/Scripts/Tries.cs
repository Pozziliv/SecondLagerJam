using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Agava.YandexGames;
using System;
using UnityEngine.Analytics;
using Assets.Scripts.Battle;

public class Tries : MonoBehaviour
{
    [SerializeField] private Aviaries _aviaries;
    [SerializeField] private Game _game;
    [SerializeField] private int _adBuyAmount;
    [SerializeField] private ScreenAppear _adErrorScreen;

    [SerializeField] private BattleSystem _battleSystem;

    private int _tries;
    private int _usedAd = 0;
    private bool _AdActive;

    private bool _isNotMutedBeforeAd = false;

    public int AdBuyAmount => _adBuyAmount;
    public int UsedAd => _usedAd;

    public event UnityAction<int> TriesChanged;

    public Action VideoAdOpened;
    public Action VideoAdClosed;
    public Action VideoAdRewarded;
    public Action<string> VideoAdErrorOccurred;

    private void OnEnable()
    {
        _game.LevelStarted += ResetTries;
        _aviaries.Interacted += Try;
        VideoAdErrorOccurred += OnAdErrorOccured;
        VideoAdRewarded += OnAdRewarded;
        VideoAdClosed += OnAdClosed;
    }

    private void OnDisable()
    {
        _game.LevelStarted -= ResetTries;
        _aviaries.Interacted -= Try;
        VideoAdErrorOccurred -= OnAdErrorOccured;
        VideoAdRewarded -= OnAdRewarded;
        VideoAdClosed -= OnAdClosed;
    }

    //TODO: ������
    private void OnAdRewarded()
    {
        if (_AdActive == false)
            return;
        
        /*_tries += _adBuyAmount;
        TriesChanged?.Invoke(_tries);*/
        _usedAd++;
        _AdActive = false;
    }

    private void OnAdErrorOccured(string error)
    {
        FindFirstObjectByType<InGameSound>().SetMute(false);
        _adErrorScreen.Appear();
    }

    private void Try()
    {
        _tries--;
        TriesChanged?.Invoke(_tries);
        if (_tries == 0 && _AdActive == false)
        {
            int level = DB.GetLevel();
            Dictionary<string, object> eventParameters = new Dictionary<string, object>
        {
            { "Level number",  level},
            {"result",  "lose" },
            {"continues" , _usedAd }
        };
            eventParameters.Clear();
        }
    }

    private void ResetTries(int level, LevelType type)
    {
        int rows = 1 + ((level - 1) % 4 + 1) * 2;
        _tries = rows * 2;
        if (DB.GetLevel() == 1)
            _tries = 9;
        TriesChanged?.Invoke(_tries);
    }

    public void StartAD()
    {
        _AdActive = true;
        if (FindFirstObjectByType<InGameSound>()._muted == false)
            _isNotMutedBeforeAd = true;
        FindFirstObjectByType<InGameSound>().SetMute(true);
        VideoAd.Show(VideoAdOpened, VideoAdRewarded, VideoAdClosed, VideoAdErrorOccurred);
    }

    private void OnAdClosed()
    {
        StartCoroutine(_battleSystem.AdvertisementAttack());
        if (_isNotMutedBeforeAd)
        {
            _isNotMutedBeforeAd = false;
            FindFirstObjectByType<InGameSound>().SetMute(false);
        }
    }
}
