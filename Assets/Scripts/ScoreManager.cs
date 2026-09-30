using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("Score UI")]
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _mainBestScoreText;
    [SerializeField] private TMP_Text _gameOverScoreText;
    [SerializeField] private TMP_Text _gameOverBestScoreText;

    private int _score;
    private int _bestScore;

    private void Awake()
    {
        Instance = this;

        UpdateUI();
    }

    private void OnEnable()
    {
        GameManager.GameReset += ResetScore;
    }

    private void OnDisable()
    {
        GameManager.GameReset -= ResetScore;
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
}