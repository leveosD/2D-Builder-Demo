using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    public class BuildingsTilemap : MonoBehaviour
    {
        private Tilemap _tilemap;
        
        private void Start()
        {
            _tilemap = GetComponent<Tilemap>();
            //DataManager.Load(_tilemap);
        }
    }
}