using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class Size
{
    public string name;
    public int x;
    public int y;
}

public class BaseMode : IPlayMode
{
    protected Tilemap _buildingsTilemap;
    protected Tilemap _planningTilemap;
    protected Vector3Int[] _positions;
    protected Vector3Int _tilemapPosition = Vector3Int.zero;
    protected CancellationTokenSource _cancellationTokenSource;
    protected CancellationToken _cancellationToken;
    private Vector3Int _leftTop;
    private Vector3Int _fieldSize;
    protected BaseMode(Tilemap buildingsTilemap, Tilemap planningTilemap)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
        _buildingsTilemap = buildingsTilemap;
        _planningTilemap = planningTilemap;
    }
    
    public virtual bool Submit()
    {
        return true;
    }
    
    public virtual void Cancel()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;
        
        ClearTilemapInPositions(_planningTilemap, _positions);
    }
    
    protected void ClearTilemapInPositions(Tilemap tilemap, Vector3Int[] positions)
    {   
        if (_positions == null)
            return;
        foreach (var pos in positions)
        {
            tilemap.SetTile(pos, null);
        }
    }
}