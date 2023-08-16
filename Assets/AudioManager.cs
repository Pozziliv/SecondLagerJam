using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] _sounds;
    [SerializeField] private AudioSource _source;

    //private void Awake()
    //{
    //    foreach (var s in _sounds)
    //    {
    //        //s.Source = gameObject.AddComponent<AudioSource>();
            
    //        //s.Source.clip = s.Clip;

    //        //s.Source.volume = s.Volume;
    //        //s.Source.pitch = s.Pitch;

    //        //var volumeChanger = gameObject.AddComponent<AudioSourceVolumeChanger>();
    //        //volumeChanger._audio = s.Source;
    //        //volumeChanger.gameObject.SetActive(true);
    //    }
    //}

    public void Play(string name)
    {
        if (!FindAnyObjectByType<GlobalVolume>()._muted)
        {
            Sound sound = Array.Find(_sounds, s => s.Name == name);
            SourceChange(sound);
            _source.Play();
        }
        
    }

    private void SourceChange(Sound sound)
    {
        _source.clip = sound.Clip;
        _source.volume = sound.Volume;
        _source.pitch = sound.Pitch;
    }
}
