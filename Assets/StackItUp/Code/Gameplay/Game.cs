using FuriousPlay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public int currentLevel;
	public PinSetup activeSetup;
	public List<Color> colors;
	public Globals data;
	public List<StackData> stacks;
	public List<StackPin> stackPins;
	public List<GameObject> pooledGameObjects;
	public List<PinSetup> pinsetUps;
	public Transform poolParent;
	public int moves;
	private int stacksToWin;

	private void Awake()
	{
		ActionManager.SubscribeToEvent(GameEvents.CHECK_COMPLETE, CheckStackCompletion);
		ActionManager.SubscribeToEvent(GameEvents.RELOAD_LEVEL, Reload);
		ActionManager.SubscribeToEvent(GameEvents.SKIP_LEVEL, SkipLevel);
	}

	private void OnDestroy()
	{
		ActionManager.UnsubscribeToEvent(GameEvents.CHECK_COMPLETE, CheckStackCompletion);
		ActionManager.UnsubscribeToEvent(GameEvents.RELOAD_LEVEL, Reload);
		ActionManager.UnsubscribeToEvent(GameEvents.SKIP_LEVEL, SkipLevel);
	}

	private void Start()
	{
		name = "Game";
		data = Resources.Load<Globals>("Globals");
		currentLevel = SaveManager.SaveData.currentLevel;
		LoadLevel(currentLevel);
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

	public void LoadLevel(int level)
	{
		if (activeSetup != null)
			activeSetup.gameObject.SetActive(false);

		currentLevel = level;
		LevelData data = Resources.Load<LevelData>("Levels/" + level.ToString());
		if (data == null)
		{
			Debug.LogError("No More Levels Available");
			return;
		}

		int pinSetupIndex = data.pins;
		activeSetup = pinsetUps[pinSetupIndex];
		activeSetup.gameObject.SetActive(true);
		stackPins = activeSetup.stackPins;
		Camera.main.orthographicSize = activeSetup.cameraSize;
		Initialize();
		foreach(PinConfig config in data.pinConfig)
		{
			LoadStack(data.stack, config,data.colors);
		}

		stacksToWin = data.stackCount;
		ActionManager.TriggerEvent(GameEvents.STACK_LOAD_COMPLETE,new Hashtable() {
			{"level",currentLevel} 
		});
	}

	public void Reload()
	{
		foreach (StackPin stack in stackPins)
		{
			stack.RestoreToPool(pooledGameObjects,poolParent);
			stack.Reset();
		}
		LoadLevel(currentLevel);
	}

	public void SkipLevel()
	{
		ActionManager.TriggerEvent(GameEvents.SAVE_GAME, new Hashtable() {
			{"level",currentLevel+1}
		});
		foreach (StackPin stack in stackPins)
		{
			stack.RestoreToPool(pooledGameObjects, poolParent);
			stack.Reset();
		}
		LoadLevel(currentLevel+1);
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
	
	public void LoadStackRandom(StackData stack,int stacksToFill, int maxStackPins)
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

		ActionManager.TriggerEvent(GameEvents.STACK_LOAD_COMPLETE, new Hashtable() {
			{"level",currentLevel}
		});
		//ActionManager.TriggerEvent(GameEvents.STACK_LOAD_COMPLETE);
	}

	public void LoadStack(StackData stack,PinConfig config,List<Color> colors)
	{
		for (int j = 0; j < config.tiles.Count; j++)
		{
			var tileInfo = config.tiles[j];
			{
				GameObject tile = pooledGameObjects[0];
				pooledGameObjects.RemoveAt(0);
				var stackTile = tile.GetComponent<StackTile>();
				stackTile.SetData(stack);
				stackTile.colorCode = tileInfo.colorIndex;
				stackTile.index = tileInfo.size;
				stackTile.SetMesh(stack.meshes[tileInfo.size - 1].mesh);
				stackTile.SetMaterials(stack.materials.ToArray());
				stackTile.SetMaterialColor(colors[tileInfo.colorIndex]);
				StackPin stackPin = stackPins[config.pinIndex];
				stackTile.pinIndex = stackPin.pinIndex;
				stackPin.PushTile(stackTile.gameObject);
			}
		}

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
		ActionManager.TriggerEvent(GameEvents.SAVE_GAME, new Hashtable() {
			{"level",currentLevel+1},
			{"score",10 }
		});
		foreach (StackPin stack in stackPins)
		{
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

		foreach (StackPin stack in stackPins)
		{
			stack.RestoreToPool(pooledGameObjects, poolParent);
		}
		Handheld.Vibrate();
	}

	public void ResultCallback(MessageBoxStatus status)
	{
		if(activeSetup != null)
			activeSetup.gameObject.SetActive(false);

		if (status == MessageBoxStatus.OK)
		{
			LoadLevel(currentLevel + 1);
		}
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
