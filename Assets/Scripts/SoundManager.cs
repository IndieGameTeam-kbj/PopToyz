using UnityEngine;

public enum SoundType
{
    ButtonClick,
    Shoot,
    Reload,
    LoseLife,
    TargetHit,
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _uiSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("Sound Clips")]
    [SerializeField] private AudioClip _buttonClickClip;
    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioClip _reloadClip;
    [SerializeField] private AudioClip _loseLifeClip;
    [SerializeField] private AudioClip _targetHitClip;

    [Header("3D Sound")]
    [SerializeField] private float _minDistance = 1.0f;
    [SerializeField] private float _maxDistance = 20.0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Play(SoundType soundType)
    {
        AudioClip clip = GetClip(soundType);
        if (clip == null) return;

        AudioSource source = GetAudioSource(soundType);
        if (source == null) return;

        source.PlayOneShot(clip);
    }

    public void PlayAtPosition(SoundType soundType, Vector3 position)
    {
        AudioClip clip = GetClip(soundType);
        if (clip == null) return;

        GameObject soundObject = new GameObject($"Sound_{soundType}");
        soundObject.transform.position = position;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = _minDistance;
        audioSource.maxDistance = _maxDistance;
        audioSource.Play();
        Destroy(soundObject, clip.length);
    }

    private AudioSource GetAudioSource(SoundType soundType)
    {
        return soundType switch
        {
            SoundType.ButtonClick => _uiSource,
            SoundType.Shoot => _sfxSource,
            SoundType.Reload => _sfxSource,
            SoundType.LoseLife => _sfxSource,
            _ => null
        };
    }

    private AudioClip GetClip(SoundType soundType)
    {
        return soundType switch
        {
            SoundType.ButtonClick => _buttonClickClip,
            SoundType.Shoot => _shootClip,
            SoundType.Reload => _reloadClip,
            SoundType.LoseLife => _loseLifeClip,
            SoundType.TargetHit => _targetHitClip,
            _ => null
        };
    }

}
