using FuriousPlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class SettingsView : View
{
	public GameObject resumeButton;
	public GameObject removeAdsButton;
	public GameObject hepticToggle;
	public GameObject soundToggle;
	public SpriteAtlas iconAtlas;
	public GameObject hepticToggleSprite;
	public GameObject soundToggleSprite;

	public void Awake()
	{
		hepticToggle.GetComponent<Toggle>().isOn = SaveManager.SaveData.heptic;
		soundToggle.GetComponent<Toggle>().isOn = SaveManager.SaveData.sound;
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
		soundToggle.GetComponent<Toggle>().isOn = data.ContainsKey("sound") ? (bool)data["sound"] : false;
	}

	public override void Show()
	{
		gameObject.SetActive(true);
	}

	public void OnHepticValueChanged(bool value)
	{
		if (value)
			Handheld.Vibrate();
		string spriteName = value ? "UI_Icon_ToggleOn" : "UI_Icon_ToggleOff";
		hepticToggleSprite.GetComponent<Image>().sprite = iconAtlas.GetSprite(spriteName);
		ActionManager.TriggerEvent(GameEvents.SAVE_SETTINGS, new Hashtable() { {"heptic",value} });
	}

	public void OnSoundValueChanged(bool value)
	{
		if (value)
			Handheld.Vibrate();
		string spriteName = value ? "UI_Icon_ToggleOn" : "UI_Icon_ToggleOff";
		soundToggleSprite.GetComponent<Image>().sprite = iconAtlas.GetSprite(spriteName);
		ActionManager.TriggerEvent(GameEvents.SAVE_SETTINGS, new Hashtable() { { "sound", value } });
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
