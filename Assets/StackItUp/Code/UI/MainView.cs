using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FuriousPlay;

public class MainView : View
{
	public TextMeshProUGUI moves;
	public TextMeshProUGUI levelText;
	public GameObject settings;
	public GameObject removeAds;
	public GameObject replay;
	public GameObject skipLevel;

	public void Start()
	{
		ActionManager.SubscribeToEvent(GameEvents.STACK_LOAD_COMPLETE, OnLevelLoadComplete);
	}
	
	public void OnLevelLoadComplete(Hashtable parameters)
	{
		levelText.text = "Level " + (parameters.ContainsKey("level") ? parameters["level"].ToString() : "1");
	}

	public override void Hide()
	{
	}

	public override void Init(Hashtable data)
	{

	}

	public override void Show()
	{

	}


	public void SettingsClicked()
	{
		ActionManager.TriggerEvent(UIEvents.SETTINGS, new Hashtable() {
			{ "event", UIEvents.SETTINGS},
			{ "heptic", SaveManager.SaveData.heptic}
		});
	}
	public void RemoveAds()
	{

	}

	public void Replay()
	{
		Handheld.Vibrate();
		ActionManager.TriggerEvent(GameEvents.RELOAD_LEVEL);
	}

	public void SkipLevel()
	{
		ActionManager.TriggerEvent(GameEvents.SKIP_LEVEL);
	}
}
