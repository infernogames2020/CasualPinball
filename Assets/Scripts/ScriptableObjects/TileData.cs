using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TileType
{
	CONCRETE,
	WOOD,
	GLASS,
	ICE,
	FIRE,
	PORTAL
}

[CreateAssetMenu(fileName ="TileData",menuName = "Furious/ScriptableObjects/Tile Data",order = 1)]
public class TileData : ScriptableObject
{
	public TileType type;
	public int hp;
	public int damage;
}
