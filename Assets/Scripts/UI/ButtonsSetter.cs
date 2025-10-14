using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ButtonsSetter : MonoBehaviour
{
    private List<Sprite> _buildingsTextures;
    [SerializeField] private GameObject buttonPrefab;

    [SerializeField] private Tilemap tilemap;
    
    void Start()
    {
        _buildingsTextures = Resources.LoadAll<Sprite>("Images").ToList();
        foreach (var texture in _buildingsTextures)
        {
            var go = Instantiate(buttonPrefab, this.transform);
            go.GetComponentsInChildren<Image>()[1].sprite = texture;
            var button = go.GetComponent<BuildingsButton>();
            button.buildingName = texture.name;
        }
    }
}
