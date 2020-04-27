using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PinConfig 
{
	//public Color color;
	public int pinIndex;
	public List<TileInfo> tiles;
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
	public List<Color> colors;
	public int pins;
	public List<PinConfig> pinConfig = new List<PinConfig>();
}
