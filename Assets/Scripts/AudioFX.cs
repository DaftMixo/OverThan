using UnityEngine;

public class AudioFX : MonoBehaviour
{
    [SerializeField] private AudioClip[] _buttonAudioClips;

    private AudioSource _audioSource;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonSound()
    {
        _audioSource.clip = _buttonAudioClips[Random.Range(0, _buttonAudioClips.Length)];
        _audioSource.Play();
    }
}
