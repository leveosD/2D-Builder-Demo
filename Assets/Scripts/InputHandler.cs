using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class InputHandler : MonoBehaviour
{
    private MyInputSystem _inputSystem;
    private PlaceMode _placeMode;
    private DeleteMode _deleteMode;
    private IPlayMode _playMode;
    
    [SerializeField] private Tilemap buildingsTilemap;
    [SerializeField] private Tilemap planningTilemap;
    
    public void Awake()
    {
        _inputSystem = new MyInputSystem();

        _placeMode = new PlaceMode(buildingsTilemap, planningTilemap, DataManager.Load());
        _deleteMode = new DeleteMode(buildingsTilemap, planningTilemap);
    }

    private void OnEnable()
    {
        BuildingsButton.SpawnBuilding += OnPlaceButtonClicked;
        DeleteButton.DeleteButtonClick += OnDeleteButtonClicked;
        _inputSystem.Gameplay.Cancel.performed += Cancel;
        _inputSystem.Gameplay.Submit.performed += Submit;
    }
    
    private void OnDisable()
    {
        BuildingsButton.SpawnBuilding -= OnPlaceButtonClicked;
        DeleteButton.DeleteButtonClick -= OnDeleteButtonClicked;
        _inputSystem.Gameplay.Cancel.performed -= Cancel;
        _inputSystem.Gameplay.Submit.performed -= Submit;
    }

    private void OnPlaceButtonClicked(string bName)
    {
        _playMode?.Cancel();
        _inputSystem.Enable();
        _placeMode.Start(bName);
        _playMode = _placeMode;
    }

    private void OnDeleteButtonClicked()
    {
        _playMode?.Cancel();
        _inputSystem.Enable();
        _deleteMode.Start();
        _playMode = _deleteMode;
    }

    private void Submit(InputAction.CallbackContext obj)
    {
        if (_playMode.Submit())
        {
            _inputSystem.Disable();
            DataManager.Save(buildingsTilemap);
        }
    }
    
    private void Cancel(InputAction.CallbackContext obj)
    {
        _inputSystem.Disable();
        _playMode.Cancel();
    }
    
}