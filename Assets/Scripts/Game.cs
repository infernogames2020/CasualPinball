using FuriousPlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public static int totalLevels = 10;
	public int currentLevel;
	public List<GameObject> flaps;

	private void Start()
	{
		name = "Game";
	}

	private void OnEnable()
	{
		ActionManager.SubscribeToEvent(GameEvents.LOAD_NEXT, LoadNextLevel);
		ActionManager.SubscribeToEvent(GameEvents.RELOAD_LEVEL, Reload);
		ActionManager.SubscribeToEvent(GameEvents.STOP_PLATFORMS, StopMovement);

	}
	private void OnDisable()
	{
		ActionManager.UnsubscribeToEvent(GameEvents.LOAD_NEXT, LoadNextLevel);
		ActionManager.UnsubscribeToEvent(GameEvents.RELOAD_LEVEL, Reload);
		ActionManager.UnsubscribeToEvent(GameEvents.STOP_PLATFORMS, StopMovement);
	}

	public void Reload()
	{
		SceneManager.LoadScene("Level" + currentLevel);
	}

	public void StopMovement()
	{
		for(int i = 0; i < flaps.Count;i++)
			flaps[i].SendMessage("Stop");
	}

	public void LoadNextLevel()
	{
		if (currentLevel == totalLevels)
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
}
