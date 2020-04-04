using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
	Touch _inputManager;
	Touch inputManager { get { if (_inputManager == null) _inputManager = new Touch(); return _inputManager; } }

	// Start is called before the first frame update
	void Start()
    {
		inputManager.Player.Aim.performed += OnAim;
		inputManager.Player.Shoot.performed += OnShoot;
		inputManager.Player.Drag.performed += OnDrag;
	}

	private void OnEnable()
	{
		Invoke("EnableInput", 5);
	}
	void EnableInput()
	{
		inputManager.Enable();
	}
	private void OnDisable()
	{
		inputManager.Disable();
	}
	private void OnDestroy()
	{
		inputManager.Player.Aim.performed -= OnAim;
		inputManager.Player.Shoot.performed -= OnShoot;
		inputManager.Player.Drag.performed -= OnDrag;
	}

	private void OnDrag(InputAction.CallbackContext obj)
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			Debug.LogError("[OnDrag] Pointer Over UI Element");
			return;
		}
		Debug.LogError("On Drag");
	}

	private void OnShoot(InputAction.CallbackContext obj)
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			Debug.LogError("[OnShoot] Pointer Over UI Element");
			return;
		}

		Debug.LogError("On Shoot");
	}

	private void OnAim(InputAction.CallbackContext obj)
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			Debug.LogError("[OnAim] Pointer Over UI Element");
			return;
		}

		Debug.LogError("On Aim");
	}

	
}
