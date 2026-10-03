using UnityEngine;

public enum SoundType
{
    ButtonClick,
    Shoot,
    Reload,
    LoseLife,
    Countdown,
    GameStart,
    Hit,
    Logo,
    GameOver,
    ScoreCount,
    BestScore,
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _uiSource;
    [SerializeField] private AudioSource _sfxSource;

    [SerializeField] private AudioClip _buttonClickClip;
    [SerializeField] private AudioClip _shootClip;
    [SerializeField] private AudioClip _reloadClip;
    [SerializeField] private AudioClip _loseLifeClip;
    [SerializeField] private AudioClip _countdownClip;
    [SerializeField] private AudioClip _gameStartClip;
    [SerializeField] private AudioClip[] _hitClips;
    [SerializeField] private AudioClip _logoClip;
    [SerializeField] private AudioClip _gameOverClip;
    [SerializeField] private AudioClip _scoreCountClip;
    [SerializeField] private AudioClip _bestScoreClip;

    [SerializeField] private Transform _3dSourcesParent;

    private AudioSource[] _3dSources;
    private float _minDistance = 1.0f;
    private float _maxDistance = 20.0f;
    private int _3dSourceIndex;
    private int _3dSourceCount = 10;

    public static SoundManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Create3dSources();
    }

    private void Create3dSources()
    {
        _3dSources = new AudioSource[_3dSourceCount];

        for (int i = 0; i < _3dSourceCount; i++)
        {
            GameObject soundObject = new GameObject($"3D Source {i}");
            soundObject.transform.SetParent(_3dSourcesParent);

            AudioSource source = soundObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.spatialBlend = 1.0f;
            source.minDistance = _minDistance;
            source.maxDistance = _maxDistance;
            _3dSources[i] = source;
        }
    }

    public void Play(SoundType soundType)
    {
        if (SaveManager.Instance.IsMuted) return;

        AudioClip clip = GetClip(soundType);
        if (clip == null) return;

        AudioSource source = GetAudioSource(soundType);
        if (source == null) return;

        source.PlayOneShot(clip);
    }

    public void PlayAtPosition(SoundType soundType, Vector3 position)
    {
        if (SaveManager.Instance.IsMuted) return;
        if (_3dSources == null || _3dSources.Length == 0) return;

        AudioClip clip = GetClip(soundType);

        if (soundType == SoundType.Hit)
        {
            if (_hitClips == null || _hitClips.Length == 0) return;

            clip = _hitClips[Random.Range(0, _hitClips.Length)];
        }

        if (clip == null) return;

        AudioSource source = GetNext3dSource();
        source.transform.position = position;
        source.PlayOneShot(clip);
    }

    private AudioSource GetNext3dSource()
    {
        AudioSource source = _3dSources[_3dSourceIndex];
        _3dSourceIndex++;
        if (_3dSourceIndex >= _3dSources.Length) _3dSourceIndex = 0;
        return source;
    }

    private AudioSource GetAudioSource(SoundType soundType)
    {
        return soundType switch
        {
            SoundType.ButtonClick => _uiSource,
            SoundType.Shoot => _sfxSource,
            SoundType.Reload => _sfxSource,
            SoundType.LoseLife => _sfxSource,
            SoundType.Countdown => _sfxSource,
            SoundType.GameStart => _sfxSource,
            SoundType.Logo => _uiSource,
            SoundType.GameOver => _uiSource,
            SoundType.ScoreCount => _uiSource,
            SoundType.BestScore => _uiSource,
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
            SoundType.Countdown => _countdownClip,
            SoundType.GameStart => _gameStartClip,
            SoundType.Logo => _logoClip,
            SoundType.GameOver => _gameOverClip,
            SoundType.ScoreCount => _scoreCountClip,
            SoundType.BestScore => _bestScoreClip,
            _ => null
        };
    }

}
