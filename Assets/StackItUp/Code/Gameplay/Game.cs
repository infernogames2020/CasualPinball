using FuriousPlay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public int currentLevel;
	public List<Color> colors;
	public Globals data;
	public List<StackData> stacks;
	public List<StackPin> stackPins;
	public List<GameObject> pooledGameObjects;
	public Transform poolParent;

	private int stacksToWin;

	private void Awake()
	{
		ActionManager.SubscribeToEvent(GameEvents.CHECK_COMPLETE, CheckStackCompletion);
	}

	private void OnDestroy()
	{
		ActionManager.UnsubscribeToEvent(GameEvents.CHECK_COMPLETE, CheckStackCompletion);
	}

	private void Start()
	{
		name = "Game";
		data = Resources.Load<Globals>("Globals");
		Initialize();
		LoadStack(stacks[0], 2, 3);
	}

	private void Initialize()
	{
		int index = 1;
		foreach(StackPin stack in stackPins)
		{
			stack.Init();
			stack.pinIndex = index;
			index++;
		}
	}

	private void OnEnable()
	{
		ActionManager.SubscribeToEvent(GameEvents.LOAD_NEXT, LoadNextLevel);
		ActionManager.SubscribeToEvent(GameEvents.RELOAD_LEVEL, Reload);
		//ActionManager.SubscribeToEvent(GameEvents.STOP_PLATFORMS, StopMovement);

	}

	private void OnDisable()
	{
		ActionManager.UnsubscribeToEvent(GameEvents.LOAD_NEXT, LoadNextLevel);
		ActionManager.UnsubscribeToEvent(GameEvents.RELOAD_LEVEL, Reload);
		//ActionManager.UnsubscribeToEvent(GameEvents.STOP_PLATFORMS, StopMovement);
	}

	public void Reload()
	{
		//SceneManager.LoadScene("Level" + currentLevel);
		SceneManager.LoadScene("Game");

	}

	public void StopMovement()
	{
		//for(int i = 0; i < tiles.Count;i++)
		//	tiles[i].SendMessage("Stop");
	}

	public void LoadNextLevel()
	{
		if (currentLevel == data.totalLevels)
		{
			SceneManager.LoadScene("Level1");
		}
		else
		{
			SceneManager.LoadScene("Level" + (currentLevel+1));
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.tag.Equals("Ball"))
		{
			Reload();
		}
	}
	
	public void LoadStack(StackData stack,int stacksToFill, int maxStackPins)
	{
		stacksToWin = stacksToFill;
		if (stacksToFill > maxStackPins)
		{
			Debug.LogError("Pins to fill is higher than max pins");
			return;
		}

		stackPins.Shuffle();
		colors.Shuffle();

		Color[] randomColors = new Color[stacksToFill];

		for (int i = 0;i < stacksToFill; i++)
		{
			randomColors[i] = colors[i];
			//randomStackpins[i] = stackPins[i];
		}

		for(int j = 0;j < stacksToFill;j++)
		{
			int colorCode = j;
			Color color = randomColors[j];
			stack.meshes.Shuffle();
			for (int i = 0; i < stack.meshes.Count;i++)
			{
				GameObject tile = pooledGameObjects[0];
				pooledGameObjects.RemoveAt(0);
				var stackTile = tile.GetComponent<StackTile>();
				stackTile.SetData(stack);
				stackTile.colorCode = colorCode;
				stackTile.index = stack.meshes[i].index;
				stackTile.SetMesh(stack.meshes[i].mesh);
				stackTile.SetMaterials(stack.materials.ToArray());
				stackTile.SetMaterialColor(color);
				stackPins.Shuffle();
				StackPin stackPin = stackPins[0];
				stackTile.pinIndex = stackPin.pinIndex;
				stackPin.PushTile(stackTile.gameObject);
			}
		}

		ActionManager.TriggerEvent(GameEvents.STACK_LOAD_COMPLETE);
	}

	public void CheckStackCompletion()
	{
		int count = stacksToWin;
		foreach(StackPin stack in stackPins)
		{
			if(stack.CheckComplete())
			{
				count--;
			}
		}

		if(count <= 0)
		{
			Debug.LogError("You win");
			Win();
		}
	}

	public void Win()
	{
		foreach (StackPin stack in stackPins)
		{
			stack.RestoreToPool(pooledGameObjects);
			stack.Celebrate();
		}
		StartCoroutine(OnCelebrationComplete());
	}

	IEnumerator OnCelebrationComplete()
	{
		yield return new WaitForSeconds(1.5f);
		ActionManager.TriggerEvent(UIEvents.RESULT,new Hashtable() {
			{ "event", UIEvents.RESULT},
			{ "smiley", "UI_Icon_SmileyHappy"},
			{ "message", "Level is Completed"},
			{ "emotion", "Awesome!"},
			{ "callback",(Action<MessageBoxStatus>)ResultCallback}
		});
	}

	public void ResultCallback(MessageBoxStatus status)
	{
		if(status == MessageBoxStatus.OK)
			LoadStack(stacks[0], 2, 3);
	}

	private T[] ShuffleArray<T>(T[] array)
	{
		System.Random r = new System.Random();
		for (int i = array.Length; i > 0; i--)
		{
			int j = r.Next(i);
			T k = array[j];
			array[j] = array[i - 1];
			array[i - 1] = k;
		}

		return array;
	}

}

public static class IListExtensions
{
	/// <summary>
	/// Shuffles the element order of the specified list.
	/// </summary>
	public static void Shuffle<T>(this IList<T> ts)
	{
		var count = ts.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i)
		{
			var r = UnityEngine.Random.Range(i, count);
			var tmp = ts[i];
			ts[i] = ts[r];
			ts[r] = tmp;
		}
	}
}
