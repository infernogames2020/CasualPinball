using FuriousPlay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ViewEntry
{
	public string Id;
	public View viewObject;
}

public enum MessageBoxStatus
{
	OK,
	CANCEL,
	CLOSE
}

public class ViewController : MonoBehaviour
{
	[SerializeField]
	List<ViewEntry> viewList;

	private Dictionary<string, ViewEntry> lookupMap;
	private void Awake()
	{
		ActionManager.SubscribeToEvent(UIEvents.RESULT, ShowResult);
		ActionManager.SubscribeToEvent(UIEvents.SETTINGS, ShowResult);
	}

	private void Start()
	{
		lookupMap = new Dictionary<string, ViewEntry>();

		foreach (ViewEntry viewEntry in viewList)
		{
			if (!lookupMap.ContainsKey(viewEntry.Id))
				lookupMap.Add(viewEntry.Id, viewEntry);
		}
	}
	private void OnDestroy()
	{
		ActionManager.SubscribeToEvent(UIEvents.RESULT, ShowResult);
		ActionManager.SubscribeToEvent(UIEvents.RESULT, ShowResult);
	}

	private void ShowResult(Hashtable paramaters)
	{
		string eventId = paramaters["event"].ToString();
		lookupMap[eventId].viewObject.Init(paramaters);
		lookupMap[eventId].viewObject.Show();
	}

	private void ShowSettings(Hashtable paramaters)
	{
		string eventId = paramaters["event"].ToString();
		lookupMap[eventId].viewObject.Init(paramaters);
		lookupMap[eventId].viewObject.Show();
	}
}
