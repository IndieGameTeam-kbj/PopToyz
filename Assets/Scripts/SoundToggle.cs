using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    [SerializeField] private Image _soundImage;
    [SerializeField] private Sprite _soundOnSprite;
    [SerializeField] private Sprite _soundOffSprite;

    private bool _isMuted;

    private void OnEnable()
    {
        _isMuted = AudioListener.volume == 0f;

        _soundImage.sprite = _isMuted
            ? _soundOffSprite
            : _soundOnSprite;
    }

    public void ToggleSound()
    {
        _isMuted = !_isMuted;

        AudioListener.volume = _isMuted ? 0f : 1f;

        _soundImage.sprite = _isMuted
            ? _soundOffSprite
            : _soundOnSprite;
    }
}