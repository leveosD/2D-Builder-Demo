using System;

[Serializable]
public class Field
{
    public int width;
    public int height;
    public int offsetX;
    public int offsetY;
}

[Serializable]
public class BuildingSaveData
{
    public string name;
    public int x;
    public int y;
}

[Serializable]
public class FieldSaveData
{
    public BuildingSaveData[] buildings;
}