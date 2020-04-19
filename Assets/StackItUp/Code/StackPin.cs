using FuriousPlay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPin : MonoBehaviour
{
	public static GameObject SelectedTile;

	public List<GameObject> tiles;
	public GameObject pin;

	private Stack<GameObject> stack;
	private Bounds baseBounds;
	private Bounds pinBounds;
	private Vector3 entryPoint;
	private Vector3 startPoint;
	private Vector3 nextPosition;

	private void Start()
	{
		stack = new Stack<GameObject>();
		//ActionManager.SubscribeToEvent(GameEvents.POP_TILE, PopTile);
		//ActionManager.SubscribeToEvent(GameEvents.PUSH_TILE, PushTile);
		baseBounds = gameObject.GetComponent<MeshFilter>().sharedMesh.bounds;
		pinBounds  = pin.GetComponent<MeshFilter>().sharedMesh.bounds;
		startPoint = transform.InverseTransformPoint(transform.position + (Vector3.up * baseBounds.extents.y));
		entryPoint = transform.InverseTransformPoint(transform.position + (Vector3.up * pinBounds.size.y));

		foreach (GameObject tile in tiles)
		{
			stack.Push(tile);
		}
		Reposition();
	}

	private void OnDestroy()
	{
		//ActionManager.UnsubscribeToEvent(GameEvents.POP_TILE, PopTile);
		//ActionManager.SubscribeToEvent(GameEvents.PUSH_TILE, PushTile);
	}

	public void PopTile()
	{
		try
		{
			var tile = stack.Pop();
			tile.GetComponent<StackTile>().PopTile(this);
			SelectedTile = tile;
		}
		catch(Exception exception)
		{
			Debug.LogError("No More tiles in " + gameObject.name);
		}
	}

	public void PushTile(GameObject tile)
	{
		stack.Push(tile);
		tile.transform.parent = transform;
		tile.GetComponent<StackTile>().PushTile(this);
		SelectedTile = null;
		//tile.transform.localPosition = CalculateNextPosition(tile);
	}

	public void Reposition()
	{
		int count = stack.Count - 1;
		
		foreach (GameObject tile in stack)
		{
			float tileHeight = tile.GetComponent<MeshFilter>().sharedMesh.bounds.extents.y;
			float height = (2 * count * tileHeight) + tileHeight;
			tile.transform.localPosition = startPoint + (height * Vector3.up);
			count--;
		}
	}

	#region Math

	public Vector3 CalculateNextPosition(GameObject tile)
	{
		float tileHeight = tile.GetComponent<MeshFilter>().sharedMesh.bounds.extents.y;
		float height = (2 * (stack.Count - 1)* tileHeight) + tileHeight;
		nextPosition = startPoint + (height * Vector3.up);
		return nextPosition;
	}

	public Vector3 GetEntryPoint()
	{
		return entryPoint;
	}

	#endregion

}
