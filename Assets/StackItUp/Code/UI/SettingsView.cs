using FuriousPlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsView : View
{
	public GameObject resumeButton;
	public GameObject removeAdsButton;
	public GameObject hepticToggle;

	public void Awake()
	{
		hepticToggle.GetComponent<Toggle>().isOn = SaveManager.SaveData.heptic;
	}

	public override void Hide()
	{
		gameObject.SetActive(false);
	}

	public override void Init(Hashtable data)
	{
		//emotion.text = data.ContainsKey("emotion") ? data["emotion"].ToString() : "";
		//message.text = data.ContainsKey("message") ? data["message"].ToString() : "";

		hepticToggle.GetComponent<Toggle>().isOn = data.ContainsKey("heptic") ? (bool)data["heptic"] : false;
	}

	public override void Show()
	{
		gameObject.SetActive(true);
	}

	public void OnHepticValueChanged(bool value)
	{
		ActionManager.TriggerEvent(GameEvents.SAVE_SETTINGS, new Hashtable() { {"heptic",value} });
	}

	public void NoADS()
	{
		ActionManager.TriggerEvent(GameEvents.SAVE_SETTINGS, new Hashtable() { { "noads",true} });
	}

	public void Resume()
	{
		Hide();
	}
}
