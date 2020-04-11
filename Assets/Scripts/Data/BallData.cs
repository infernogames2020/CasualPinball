using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	[System.Serializable]
	public enum BallType
	{
		RUBBER,
		WOOD,
		GLASS,
		ICE,
		FIRE,
		METAL
	}

	[CreateAssetMenu(fileName = "BallData", menuName = "Furious/ScriptableObjects/Ball Data", order = 2)]
	public class BallData : ScriptableObject
	{
		public BallType type;
		public int hp;
		public float speed;
		public float rotationSpeed;
		public float deceleration;
		public int damage;
	}
