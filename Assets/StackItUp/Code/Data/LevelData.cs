using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PinConfig 
{
	public Color color;
	public int colorIndex;
	public int sizeIndex;
	public int pinIndex;
}

[CreateAssetMenu(fileName = "Stack", menuName = "Furious/ScriptableObjects/LevelData", order = 1)]

public class LevelData : ScriptableObject
{
	public StackData stack;
	public List<PinConfig> pinConfig = new List<PinConfig>();
}
