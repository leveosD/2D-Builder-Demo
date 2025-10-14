using System;
using UnityEngine;
using UnityEngine.UI;

public class DeleteButton : MonoBehaviour
{
    private Button _button;
    public static event Action DeleteButtonClick;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClicked);
    }
    
    private void OnEnable()
    {
        _button?.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonClicked);

    }

    private void OnButtonClicked()
    {
        DeleteButtonClick?.Invoke();
    }
}
