using UnityEngine.Tilemaps;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeleteMode : BaseMode
{
    private Texture2D _cursor;
    private Dictionary<string, Size> _sizes;

    public DeleteMode(Tilemap buildingsTilemap, Tilemap planningTilemap) : base(buildingsTilemap, planningTilemap)
    {
        _cursor = Resources.Load<Texture2D>("DeleteButton");
        _sizes = new Dictionary<string, Size>();
        var jsons = Resources.LoadAll<TextAsset>($"Configs");
        foreach (var json in jsons)
        {
            _sizes.Add(json.name, JsonUtility.FromJson<Size>(json.text));
        }
    }
    
    private async UniTask Tick(CancellationToken token)
    {
        while (true)
        {
            token.ThrowIfCancellationRequested();

            var mousePosition = Mouse.current.position;
            var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition.ReadValue());
            Vector3Int newPosition = Vector3Int.RoundToInt(worldPosition);
            newPosition.z = 0;
            
            if (newPosition.x != _tilemapPosition.x || newPosition.y != _tilemapPosition.y)
            {
                _tilemapPosition = newPosition;
                if (_buildingsTilemap.GetTile(_tilemapPosition))
                {
                    Cursor.SetCursor(_cursor, Vector3.zero, CursorMode.Auto);
                }
                else
                {
                    Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
                }
            }

            await UniTask.Yield();
        }
    }
    
    public void Start()
    {
        Tick(_cancellationToken).Forget();
    }

    public override bool Submit()
    {
        var target = _buildingsTilemap.GetTile(_tilemapPosition);
        if (!target)
            return false;
        var split = target.name.Split('_');
        string buildingName = split[0];
        int number = int.Parse(split[1]);
        if (!_sizes.TryGetValue(buildingName, out var currentSize))
            return false;

        int deltaX = number % currentSize.x;
        int deltaY = number / currentSize.x;

        Vector3Int delta = new Vector3Int(deltaX, -deltaY);

        Vector3Int startPoint = _tilemapPosition - delta;
        
        for(int i = 0; i < currentSize.x; i++)
        {
            for (int j = 0; j < currentSize.y; j++)
            {
                var pos = startPoint + new Vector3Int(i, -j, 0);
                _buildingsTilemap.SetTile(pos, null);
            }
        }
        
        Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
        
        /*_cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;*/
        
        return false;
    }

    public override void Cancel()
    {
        Cursor.SetCursor(null, Vector3.zero, CursorMode.Auto);
        
        base.Cancel();
    }
}