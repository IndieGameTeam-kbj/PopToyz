using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private const string SoundMutedKey = "SoundMuted";
    private const string BestScoreKey = "BestScore";

    private bool _isMuted;
    private int _bestScore;

    public bool IsMuted => _isMuted;
    public int BestScore => _bestScore;

    public static SaveManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.Save();
        Load();
    }

    public void SetSoundMuted(bool isMuted)
    {
        _isMuted = isMuted;
        PlayerPrefs.SetInt(SoundMutedKey, isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetBestScore(int score)
    {
        if (score <= BestScore) return;

        _bestScore = score;
        PlayerPrefs.SetInt(BestScoreKey, BestScore);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        _isMuted = PlayerPrefs.GetInt(SoundMutedKey, 0) == 1;
        _bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
    }

}
