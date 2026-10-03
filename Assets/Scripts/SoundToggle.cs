using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    [SerializeField] private Image _soundImage;
    [SerializeField] private Sprite _soundOnSprite;
    [SerializeField] private Sprite _soundOffSprite;

    private bool _isMuted;

    public void ToggleSound()
    {
        _isMuted = !_isMuted;
        SaveManager.Instance.SetSoundMuted(_isMuted);
        _soundImage.sprite = _isMuted ? _soundOffSprite : _soundOnSprite;
        SoundManager.Instance.Play(SoundType.ButtonClick);
    }

    private void OnEnable()
    {
        _isMuted = SaveManager.Instance.IsMuted;
        _soundImage.sprite = _isMuted ? _soundOffSprite : _soundOnSprite;
    }

}
