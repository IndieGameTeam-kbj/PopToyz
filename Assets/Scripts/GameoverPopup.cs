using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameoverPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;

    private float _scoreAnimationDuration = 0.8f;
    private float _scoreSoundInterval = 0.08f;

    public void PlayScoreAnimation()
    {
        int displayScore = 0;
        int score = ScoreManager.Instance.Score;
        float lastSoundTime = -Mathf.Infinity;

        _scoreText.text = "0";

        DOTween.To(
            () => displayScore,
            value =>
            {
                if (value > displayScore && Time.unscaledTime - lastSoundTime >= _scoreSoundInterval)
                {
                    SoundManager.Instance.Play(SoundType.ScoreCount);
                    lastSoundTime = Time.unscaledTime;
                }

                displayScore = value;
                _scoreText.text = displayScore.ToString();
            },
            score,
            _scoreAnimationDuration
        )
        .SetUpdate(true);
    }

    public void OnClickReTry()
    {
        GameManager.Instance.RestartGame();
        SoundManager.Instance.Play(SoundType.ButtonClick);
    }

    public void OnClickHome()
    {
        GameManager.Instance.GoMainMenu();
        SoundManager.Instance.Play(SoundType.ButtonClick);
    }

}
