using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioFX : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [Space]
    [SerializeField] private AudioSource _soundSource;
    [SerializeField] private AudioSource _musicSource;
    [Space]
    [SerializeField] private AudioClip[] _buttonAudioClips;
    [SerializeField] private AudioClip[] _musicAudioClips;

    private AudioClip _playedClip;

    private void Start()
    {
        _playedClip = _musicSource.clip = _musicAudioClips[Random.Range(0, _buttonAudioClips.Length)];
        _musicSource.Play();
    }

    private void Update()
    {
        if (!_musicSource.isPlaying)
        {
            Debug.Log("next clip");
            AudioClip newClip = _musicAudioClips[Random.Range(0, _buttonAudioClips.Length)];
            
            while (newClip == _playedClip)
            {
                newClip = _musicAudioClips[Random.Range(0, _buttonAudioClips.Length)];
            }

            _playedClip = newClip;
            _musicSource.clip = newClip;
            _musicSource.Play();
        }
    }

    public void SetSound(float value)
    {
        _mixer.SetFloat("Sound", Mathf.Log10(value) * 20);
    }
    
    public void SetMusic(float value)
    {
        _mixer.SetFloat("Music", Mathf.Log10(value) * 20);
    }

    public void PlayButtonSound()
    {
        _soundSource.clip = _buttonAudioClips[Random.Range(0, _buttonAudioClips.Length)];
        _soundSource.Play();
    }
}
