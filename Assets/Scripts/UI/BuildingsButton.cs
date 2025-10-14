using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingsButton : MonoBehaviour
{
    public string buildingName;

    private Button _button;

    public static event Action<string> SpawnBuilding;

    private void Start()
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
        SpawnBuilding?.Invoke(buildingName);
    }
}
