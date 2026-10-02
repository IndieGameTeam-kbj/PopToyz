using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _mainBestScoreText;
    [SerializeField] private TMP_Text _gameOverScoreText;
    [SerializeField] private TMP_Text _gameOverBestScoreText;
    [SerializeField] private GameObject _bestImage;

    private int _score;
    private int _bestScore;
    private bool _isBest;

    public static ScoreManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _bestImage.SetActive(false);
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        _score += amount;

        if (_score > _bestScore)
        {
            _bestScore = _score;

            if (!_bestImage.activeSelf)
            {
                if (!_isBest)
                {
                    _isBest = true;
                    PlayBestAnimation();
                }
            }
        }

        UpdateUI();
        PlayScoreAnimation();
    }

    public void ResetScore()
    {
        _score = 0;
        _isBest = false;
        _bestImage.SetActive(false);
        UpdateUI();
    }

    private void UpdateUI()
    {
        _scoreText.text = _score.ToString();
        _mainBestScoreText.text = _bestScore.ToString();
        _gameOverScoreText.text = _score.ToString();
        _gameOverBestScoreText.text = _bestScore.ToString();
    }

    private void PlayScoreAnimation()
    {
        Transform target = _scoreText.transform;
        target.DOKill();
        target.localScale = Vector3.one;
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(target.DOScale(1.2f, 0.1f));
        sequence.Append(target.DOScale(1f, 0.1f));
    }

    public void PlayGameOverScoreAnimation()
    {
        int displayScore = 0;

        _gameOverScoreText.text = "0";

        DOTween.To(
            () => displayScore,
            value =>
            {
                displayScore = value;
                _gameOverScoreText.text = displayScore.ToString();
            },
            _score,
            0.8f
        )
        .SetUpdate(true);
    }

    private void PlayBestAnimation()
    {
        Transform target = _bestImage.transform;
        _bestImage.SetActive(true);
        target.DOKill();
        target.localScale = Vector3.zero;
        target.DOScale(1.0f, 0.2f).SetEase(Ease.OutBack);
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
