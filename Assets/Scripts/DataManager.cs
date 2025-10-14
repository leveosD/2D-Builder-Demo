using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataManager
{
    private static Vector3Int _leftTop;
    private static Vector3Int _fieldSize;
    private static string _fileName = Application.persistentDataPath + "/save.json";
    
    static DataManager()
    {
        Field field = JsonUtility.FromJson<Field>(Resources.Load<TextAsset>("Configs/FieldConf").text);
        _leftTop = new Vector3Int(-field.width / 2 + field.offsetX, field.height / 2 + field.offsetY);
        _fieldSize = new Vector3Int(field.width, field.height);
    }
    
    public static void Save(Tilemap tilemap)
    {
        TileBase tile;
        List<BuildingSaveData> buildingSaveDatas = new List<BuildingSaveData>();
        for (int i = 0; i < _fieldSize.x; i++)
        {
            for (int j = 0; j < _fieldSize.y; j++)
            {
                var fieldPosition = _leftTop + new Vector3Int(i, -j);
                tile = tilemap.GetTile(fieldPosition);
                if (tile)
                {
                    var split = tile.name.Split('_');
                    if (split[1] == "0")
                    {
                        var coords = new BuildingSaveData()
                            { name = split[0], x = fieldPosition.x, y = fieldPosition.y };
                        buildingSaveDatas.Add(coords);
                    }
                }
            }
        }
        FieldSaveData fieldSaveData = new FieldSaveData();
        fieldSaveData.buildings = buildingSaveDatas.ToArray();

        var json = JsonUtility.ToJson(fieldSaveData, true);
        
        File.WriteAllText(_fileName, json);
    }

    public static BuildingSaveData[] Load()
    {
        if (!File.Exists(_fileName))
        {
            return null;
        }
        string data = File.ReadAllText(_fileName);
        var fieldSaveData = JsonUtility.FromJson<FieldSaveData>(data);

        return fieldSaveData.buildings;
    }
}