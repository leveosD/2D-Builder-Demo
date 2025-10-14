using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class Building
{
    public Size size;
    public TileBase[] tiles;
}

public class PlaceMode : BaseMode
{
    private string _currentBuildings;
    private static Dictionary<string, Building> _loadedBuildings;
    
    private static Vector3Int _leftTop;
    private static Vector3Int _rightBotton;

    public PlaceMode(Tilemap buildingsTilemap, Tilemap planningTilemap, BuildingSaveData[] buildingSaveDatas) : base(buildingsTilemap, planningTilemap)
    {
        _loadedBuildings = new Dictionary<string, Building>();
        var buildingsConfigs = Resources.LoadAll<TextAsset>("Configs/Buildings");
        foreach (var config in buildingsConfigs)
        {
            var bSize = JsonUtility.FromJson<Size>(config.text);
            TileBase[] loadedTiles = Resources.LoadAll<TileBase>($"Tiles/{bSize.name}");
            Building building = new Building() { size = bSize, tiles = loadedTiles };
            _loadedBuildings.Add(bSize.name, building);
        }
        
        foreach (var buildingSaveData in buildingSaveDatas)
        {
            var building = _loadedBuildings[buildingSaveData.name];
            var size = building.size;
            
            PlaceBuildings(_buildingsTilemap, building.tiles, size, new Vector3Int(buildingSaveData.x, buildingSaveData.y));
        }
        
        Field field = JsonUtility.FromJson<Field>(Resources.Load<TextAsset>("Configs/FieldConf").text);
        _leftTop = new Vector3Int(-field.width / 2 + field.offsetX, field.height / 2 + field.offsetY);
        var fieldSize = new Vector3Int(field.width, -field.height);
        _rightBotton = _leftTop + fieldSize;
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
                ClearTilemapInPositions(_planningTilemap, _positions);
                
                var size = _loadedBuildings[_currentBuildings].size;
                _tilemapPosition = newPosition;
                
                _positions = PlaceBuildings(_planningTilemap, _loadedBuildings[_currentBuildings].tiles, size, _tilemapPosition);
            }

            await UniTask.Yield();
        }
    }
    
    public void Start(string bName)
    {
        _currentBuildings = bName;
        
        Tick(_cancellationToken).Forget();
    }

    public override bool Submit()
    {
        if (_positions == null)
            return false;
        foreach (var p in _positions)
        {
            if (_buildingsTilemap.GetTile(p))
            {
                return false;
            }
            if (p.x > _rightBotton.x || p.x < _leftTop.x ||
                p.y < _rightBotton.y || p.y > _leftTop.y)
            {
                return false;
            }
        }

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        _cancellationToken = _cancellationTokenSource.Token;

        _buildingsTilemap.SetTiles(_positions, _loadedBuildings[_currentBuildings].tiles);
        ClearTilemapInPositions(_planningTilemap, _positions);
        _positions = null;

        return true;
    }

    private Vector3Int[] PlaceBuildings(Tilemap tilemap, TileBase[] tiles, Size size, Vector3Int offset)
    {
        Vector3Int[] positions = new Vector3Int[size.x * size.y];
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
                positions[size.x * i + j] = new Vector3Int(j, -i) + offset;
            }
        }

        tilemap.SetTiles(positions, tiles);
        return positions;
    }
}