﻿using System;
using UnityEngine;

namespace AudioSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour
    {
        [NonSerialized] public bool IsMusic;
        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void SetAudioClip(string clipName)
        {
            _source.clip = AudioManager.GetSoundClip(clipName);
        }
        
        public AudioSource GetSource()
        {
            gameObject.SetActive(true);
            return _source;
        }

        public void ReturnPlayer()
        {
            gameObject.SetActive(false);
            AudioManager.ReturnAudioPlayer(this);
        }

        private void Update()
        {
            if(!_source.isPlaying)
            {
                ReturnPlayer();
            }
        }

        public void UpdateVolume()
        {
            _source.volume = IsMusic ? AudioManager.MusicVolume : AudioManager.SfxVolume;
        }

    }
}