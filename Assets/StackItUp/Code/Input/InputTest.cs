using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
	StackItUp _inputManager;
	StackItUp inputManager { get { if (_inputManager == null) _inputManager = new StackItUp(); return _inputManager; } }

	private Vector3 screenPosition;
	private Vector3 worldPosition;
	private RaycastHit hit;
	private GameObject hitGameObject;
	private bool fireRay;

	void Start()
    {
		//inputManager.Player.Click.performed += OnClick;
		inputManager.Player.Drop.performed  += OnDrop;
		inputManager.Player.Drag.performed  += OnDrag;
	}

	private void OnEnable()
	{
		inputManager.Enable();
	}

	private void OnDisable()
	{
		inputManager.Disable();
	}
	private void OnDestroy()
	{
		//inputManager.Player.Click.performed -= OnClick;
		inputManager.Player.Drop.performed  -= OnDrop;
		inputManager.Player.Drag.performed  -= OnDrag;
	}

	private void OnDrag(InputAction.CallbackContext inputContext)
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			//Debug.LogError("[OnDrag] Pointer Over UI Element");
			return;
		}
		screenPosition = inputContext.ReadValue<Vector2>();
		//Debug.LogError("On Drag : " + screenPosition.ToString());
	}

	private void OnDrop(InputAction.CallbackContext inputContext)
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			//Debug.LogError("[OnShoot] Pointer Over UI Element");
			return;
		}
		//Debug.LogError("On Drop : " + inputContext.ReadValue<UnityEngine.InputSystem.TouchPhase>());
	}

	private void OnClick(InputAction.CallbackContext inputContext)
	{
		if (EventSystem.current.IsPointerOverGameObject())
		{
			//Debug.LogError("[OnAim] Pointer Over UI Element");
			return;
		}
		var value = inputContext.ReadValueAsButton();
		//Debug.LogError(" [OnClick] Value : " + value);
		fireRay = true;
	}


	private void FixedUpdate()
	{
		if(fireRay)
		{
			var ray = Camera.main.ScreenPointToRay(screenPosition);
			if(Physics.Raycast(ray, out hit))
			{
				if(hit.collider != null)
				{
					var gameobject = hit.collider.gameObject;
					if (gameobject.layer.Equals(LayerMask.NameToLayer("Stack")))
					{
						//Debug.LogError("Hit Stack : " + gameobject.name);
						var stackPin = gameobject.GetComponent<StackPin>();
						if (stackPin == null)
							stackPin = gameobject.transform.parent.GetComponent<StackPin>();

						if(stackPin != null)
						{
							if(StackPin.SelectedTile != null)
							{
								stackPin.PushTile(StackPin.SelectedTile);
							}
							else
							{
								stackPin.PopTile();
							}
						}
						
					}
				}
			}
			fireRay = false;
		}
	}

}
