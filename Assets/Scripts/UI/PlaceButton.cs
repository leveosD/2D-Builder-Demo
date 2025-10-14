using UnityEngine;
using UnityEngine.UI;

public class PlaceButton : MonoBehaviour
{
    [SerializeField] private RectTransform buildingsPanel;
    private Button _button;
    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        buildingsPanel.offsetMax = new Vector2(0, -70);
    }
}
