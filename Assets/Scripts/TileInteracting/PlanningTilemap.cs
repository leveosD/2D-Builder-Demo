using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlanningTilemap : MonoBehaviour
{
    private Tilemap _tilemap;
    private TilemapCollider2D _collider2D;

    private Color _green = new Color(77/255f, 1, 73/255f, 196/255f);
    private Color _red = new Color(1, 44/255f, 86/255f, 196/255f);

    private void Start()
    {
        _tilemap = GetComponent<Tilemap>();
        _collider2D = GetComponent<TilemapCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Buildings"))
        {
            _tilemap.color = _red;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Buildings"))
        {
            _tilemap.color = _green;
        }
    }
}
