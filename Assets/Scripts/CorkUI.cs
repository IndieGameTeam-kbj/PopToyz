using UnityEngine;
using UnityEngine.UI;

public class CorkUI : MonoBehaviour
{
    [SerializeField] private Image[] _corkImages;

    public void SetCorkCount(int currentCorkCount)
    {
        for (int i = 0; i < _corkImages.Length; i++)
        {
            _corkImages[i].gameObject.SetActive(i < currentCorkCount);
        }
    }
}