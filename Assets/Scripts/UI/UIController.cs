using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private RectTransform buildingsPanel;
    private bool _isOpened;
    
    public void OnPlaceButtonClicked()
    {
        if (!_isOpened)
        {
            buildingsPanel.offsetMax = new Vector2(0, 70);
        }
        else
        {
            buildingsPanel.offsetMax = new Vector2(0, 0);
        }

        _isOpened = !_isOpened;
    }
    
    public void OnDeleteButtonClicked()
    {
        buildingsPanel.offsetMax = new Vector2(0, 0);
        _isOpened = false;
    }
}
