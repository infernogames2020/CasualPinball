using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
	public static Game Instance;
	public int totalLevels;
	public int currentLevel;

	public List<GameObject> flaps;

	void Start()
	{
		name = "Game";
		Instance = this;
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
}
