using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Score UI")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _mainBestScoreText;
    [SerializeField] private TMP_Text _gameOverScoreText;
    [SerializeField] private TMP_Text _gameOverBestScoreText;

    private int _score;
    private int _bestScore;

    public static ScoreManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        UpdateUI();
    }

    public void AddScore(int amount)
    {
        _score += amount;

        if (_score > _bestScore)
        {
            _bestScore = _score;
        }

        UpdateUI();
    }

    public void ResetScore()
    {
        _score = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _scoreText.text = _score.ToString();

        _mainBestScoreText.text = _bestScore.ToString();

        _gameOverScoreText.text = _score.ToString();
        _gameOverBestScoreText.text = _bestScore.ToString();
    }

    private void OnEnable()
    {
        GameManager.GameReset += ResetScore;
    }

    private void OnDisable()
    {
        GameManager.GameReset -= ResetScore;
    }

}
