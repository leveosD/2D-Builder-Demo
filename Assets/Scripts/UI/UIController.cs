using DG.Tweening;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private RectTransform buildingsPanel;
    private bool _isOpened;
    
    public void OnPlaceButtonClicked()
    {
        if (!_isOpened)
        {
//            buildingsPanel.offsetMax = new Vector2(0, 70);
            buildingsPanel.DOLocalMoveY(45f, 0.3f)
                .SetEase(Ease.OutSine);
        }
        else
        {
  //          buildingsPanel.offsetMax = new Vector2(0, 0);
              buildingsPanel.DOLocalMoveY(-40f, 0.3f)
                  .SetEase(Ease.InSine);
        }

        _isOpened = !_isOpened;
    }
    
    public void OnDeleteButtonClicked()
    {
//        buildingsPanel.offsetMax = new Vector2(0, 0);
        buildingsPanel.DOLocalMoveY(-40f, 0.3f)
            .SetEase(Ease.OutSine);
        _isOpened = false;
    }
}
