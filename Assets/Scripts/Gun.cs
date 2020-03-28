using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
	public static Vector3 DummyTarget = new Vector3(1000, 1000, 1000);
	public List<Node> PathBuffer = new List<Node>();
	[SerializeField]
	LineRenderer tracer;
	[SerializeField]
	float ballRadius;
	[SerializeField]
	GameObject ball;
	[SerializeField]
	float ballSpeed;
	[SerializeField]
	int count;

	Vector3 worldPoint;
	Vector3 direction;
	Vector3 ballPosition;
	RaycastHit hit;
	Ray ray;
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
	}

	private void OnShoot(InputAction.CallbackContext context)
	{
#if UNITY_EDITOR
		if (context.ReadValueAsButton())
#elif UNITY_ANDROID
		if (!context.ReadValueAsButton())
#endif
		{
			if (readyToShoot)
			{
				shoot = true;
				readyToShoot = false;
			}
		}
	}

	private void OnAim(InputAction.CallbackContext context)
	{
		Vector2 inputPosition = context.ReadValue<Vector2>();
		//Debug.Log("OnAim " + inputPosition);
		worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
	}

	void Update()
	{
		//worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		worldPoint.y = ball.transform.position.y;
		direction = worldPoint - ball.transform.position;

		//if (readyToShoot)
		//{
		//	if (Input.GetMouseButtonDown(1))
		//	{
		//		shoot = true;
		//		readyToShoot = false;
		//	}
		//}
	}

	void FixedUpdate()
	{
		if (readyToShoot)
		{
			if (PathBuffer.Count > 0)
			{
				//PathBuffer[0].direction = direction;
				PathBuffer[0].target = DummyTarget;
				PathBuffer[0].origin = ball.transform.position;

			}
			else
			{
				var node = new Node();
				//node.direction = direction;
				node.origin = ball.transform.position;
				node.target = DummyTarget;
				PathBuffer.Add(node);
			}
			count = 1;
			RecursiveRaycast(direction.normalized, ball.transform.position,count);
			DrawTracer(count);
		}

		if(shoot)
		{
			ball.GetComponent<Ball>().Shot(PathBuffer,count);
			shoot = false; 
		}
	}

	void RecursiveRaycast(Vector3 direction, Vector3 origin,int nodeAdded)
	{
		ray = new Ray(origin, direction.normalized);
		Physics.Raycast (ray, out hit);
		
		if (hit.collider != null)
		{
			Debug.DrawRay(hit.point, hit.normal, Color.red);
			Debug.DrawLine(origin, hit.point, Color.green);

			PathBuffer[nodeAdded - 1].target = hit.point;

			reflectedDirection = Vector3.Reflect(direction.normalized, hit.normal);
			if (count < PathBuffer.Count)
			{
				//PathBuffer[count].direction = reflectedDirection;
				PathBuffer[count].origin = hit.point;

			}
			else
			{
				var node = new Node();
				//node.direction = reflectedDirection;
				node.target = hit.point;
				PathBuffer.Add(node);
			}
			count++;
			Debug.DrawRay(hit.point, reflectedDirection, Color.green);
			
			if (hit.collider.tag.Equals("Hole") || hit.collider.tag.Equals("Wall"))
				return;

			RecursiveRaycast(reflectedDirection, hit.point,count);
		}
	}

	void DrawTracer(int count)
	{
		tracer.positionCount = count;
		if (PathBuffer.Count > 0)
		{
			for(int i = 0;i < count; i++)
			{
				tracer.SetPosition(i, PathBuffer[i].origin);
			}
		}
	}
}
