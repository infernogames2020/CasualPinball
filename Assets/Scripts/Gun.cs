using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

[System.Serializable]
public class Node
{
	//public Vector3 direction;
	public Vector3 target;
	public Vector3 origin;
}

public class Gun : MonoBehaviour
{
	public List<Node> PathBuffer = new List<Node>();
	[SerializeField]
	Transform initialTarget;
	[SerializeField]
	LineRenderer tracer;
	[SerializeField]
	float ballRadius;
	[SerializeField]
	GameObject ball;
	[SerializeField]
	float ballSpeed;
	[SerializeField]
	float ballRotationSpeed;
	[SerializeField]
	int count;
	[SerializeField]
	float aimSpeed;
	[SerializeField]
	int impactCount;

	[SerializeField]
	Gradient lineGradient;

	Vector3 worldPoint;
	Vector3 direction;
	Vector3 ballPosition;
	RaycastHit hit;
	Ray shootRay;
	Ray screenRay;
	Vector3 reflectedDirection;
	bool shoot;
	bool readyToShoot;
	Touch _inputManager;
	Touch inputManager { get { if (_inputManager == null) _inputManager = new Touch(); return _inputManager; } }

	void Start()
	{
		PathBuffer = new List<Node>();
		readyToShoot = true;
		inputManager.Player.Aim.performed += OnAim;
		inputManager.Player.Shoot.performed += OnShoot;
		inputManager.Player.Drag.performed += OnDrag;
		screenRay = new Ray();
		shootRay = new Ray();
		previousDirection = Vector3.zero;
		tracer.colorGradient = lineGradient;
	}

	public void OnValueChanged(string input)
	{
		float result;
		if (float.TryParse(input, out result))
		{
			aimSpeed = result;
		}
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
		inputManager.Player.Aim.performed -= OnAim;
		inputManager.Player.Shoot.performed -= OnShoot;
		inputManager.Player.Drag.performed -= OnDrag;
	}

	private void OnShoot(InputAction.CallbackContext context)
	{
		if (EventSystem.current.IsPointerOverGameObject() || shoot)
			return;

		if (!context.ReadValueAsButton())
		{
			if (readyToShoot)
			{
				shoot = true;
				readyToShoot = false;
			}
		}
	}

	Vector3 startDragPosition;
	Vector3 currentDragPosition;
	Vector3 previousDirection;
	Vector3 currentDirection;
	private void OnAim(InputAction.CallbackContext context)
	{
		if (EventSystem.current.IsPointerOverGameObject() || shoot)
			return;

		startDragPosition   = context.ReadValue<Vector2>();
		startDragPosition.z = startDragPosition.y;
		startDragPosition.y = ball.transform.position.y;
	}

	private void OnDrag(InputAction.CallbackContext context)
	{
		if (EventSystem.current.IsPointerOverGameObject() || shoot)
			return;

		currentDragPosition = context.ReadValue<Vector2>();
		currentDragPosition.z = currentDragPosition.y;
		currentDragPosition.y = ball.transform.position.y;
		direction = startDragPosition - currentDragPosition;

		if (previousDirection.magnitude <= 0)
			previousDirection = direction;
	}

	void FixedUpdate()
	{
		if (readyToShoot)
		{
			if (previousDirection.magnitude > 0)
			{
				currentDirection = Vector3.MoveTowards(previousDirection, direction, aimSpeed * Time.fixedDeltaTime);
				shootRay.origin = ball.transform.position;
				shootRay.direction = currentDirection.normalized;
				previousDirection = currentDirection;
				initialTarget.position = ball.transform.position + currentDirection.normalized * 100;
			}
			Physics.Linecast(ball.transform.position, initialTarget.position, out hit);

			if (hit.collider == null)
			{
				if (PathBuffer.Count > 0)
				{
					PathBuffer[0].target = initialTarget.position;
					PathBuffer[0].origin = ball.transform.position;
					count = 1;
					if (PathBuffer.Count > 1)
					{
						PathBuffer[1].target = ball.transform.position;
						PathBuffer[1].origin = initialTarget.position;
					}
					else
					{
						var node = new Node();
						node.target = ball.transform.position;
						node.origin = initialTarget.position;
						PathBuffer.Add(node);
					}
					count = 2;
				}
				else
				{
					var node = new Node();
					node.target = initialTarget.position;
					node.origin = ball.transform.position;
					PathBuffer.Add(node);

					node = new Node();
					node.target = ball.transform.position;
					node.origin = initialTarget.position;
					PathBuffer.Add(node);
					count = 2;
				}
			}
			else
			{
				if (PathBuffer.Count > 0)
				{
					PathBuffer[0].target = hit.point;
					PathBuffer[0].origin = ball.transform.position;
				}
				else
				{
					var node = new Node();
					node.target = hit.point;
					node.origin = ball.transform.position;
					PathBuffer.Add(node);
				}
				count = 1;
				RecursiveRaycast(currentDirection.normalized, ball.transform.position, count);
			}
			DrawTracer(count);
		}

		if (shoot)
		{
			Debug.Log("PathBuffer Count : " + PathBuffer.Count);
			ball.GetComponent<Ball>().Shot((PathBuffer[0].target - ball.transform.position).normalized,ballSpeed,ballRotationSpeed);
			tracer.enabled = false;
			shoot = false;
		}
	}

	void RecursiveRaycast(Vector3 direction, Vector3 origin, int nodeAdded)
	{
		if (count > impactCount)
			return;

		shootRay = new Ray(origin, direction.normalized);
		Physics.Raycast(shootRay, out hit);

		if (hit.collider != null)
		{
			//Debug.DrawRay(hit.point, hit.normal, Color.red);
			//Debug.DrawLine(origin, hit.point, Color.green);

			PathBuffer[nodeAdded - 1].target = hit.point;

			reflectedDirection = Vector3.Reflect(direction.normalized, hit.normal);

			if (count < PathBuffer.Count)
			{
				PathBuffer[count].origin = hit.point;
			}
			else
			{
				var node = new Node();
				node.target = hit.point;
				PathBuffer.Add(node);
			}
			count++;

			//Debug.DrawRay(hit.point, reflectedDirection, Color.green);
			//if (hit.collider.tag.Equals("Hole") || hit.collider.tag.Equals("Wall"))
			//	return;

			RecursiveRaycast(reflectedDirection, hit.point, count);
		}
		
	}

	void DrawTracer(int count)
	{
		//tracer.positionCount = Mathf.Min(3,count);
		tracer.positionCount = count;
		if (PathBuffer.Count > 0)
		{
			for (int i = 0; i < count; i++)
			{
				tracer.SetPosition(i, PathBuffer[i].origin);
			}
		}
	}
}
