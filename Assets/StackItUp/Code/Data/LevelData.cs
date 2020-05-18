using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PinConfig 
{
	//public Color color;
	public int pinIndex;
	public List<TileInfo> tiles;

    public void AddTile(TileInfo tile)
    {
        if (tiles == null)
            tiles = new List<TileInfo>();
        tiles.Add(tile);
    }

    public void RemoveTile(TileInfo _tile)
    {
        //for(int j = tiles.Count; j <= 0; j--)
        //{
        //    if (tiles[j].colorIndex == _tile.colorIndex && tiles[j].size == _tile.size)
        //        tiles.RemoveAt(j);
        //}

        tiles.Remove(_tile);
    }
}

[System.Serializable]
public class TileInfo
{
	//public int stackIndex;
	public int colorIndex;
	public int size;
}

[CreateAssetMenu(fileName = "Stack", menuName = "Furious/ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
	public StackData stack;
	public int stackCount;
	public List<int> sequence;
	public List<Material> colors;
	public int pins;
	public List<PinConfig> pinConfig = new List<PinConfig>();
}
