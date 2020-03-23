using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
	public Vector3 direction;
	public Vector3 target;
}

public class Gun : MonoBehaviour
{
	public static Vector3 DummyTarget = new Vector3(1000, 1000, 1000);
	public List<Node> PathBuffer = new List<Node>();
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

	void Start()
	{
		PathBuffer = new List<Node>();
		readyToShoot = true;
	}

	void Update()
	{
		worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		worldPoint.y = ball.transform.position.y;
		direction = worldPoint - ball.transform.position;

		if (readyToShoot)
		{
			if (Input.GetMouseButtonDown(1))
			{
				shoot = true;
				readyToShoot = false;
			}
		}
	}

	void FixedUpdate()
	{
		if (readyToShoot)
		{
			if (PathBuffer.Count > 0)
			{
				PathBuffer[0].direction = direction;
				PathBuffer[0].target = DummyTarget;
			}
			else
			{
				var node = new Node();
				node.direction = direction;
				node.target = DummyTarget;
				PathBuffer.Add(node);
			}
			count = 1;
			RecursiveRaycast(direction.normalized, ball.transform.position,count);
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

			if (hit.collider.tag.Equals("Hole") || hit.collider.tag.Equals("Wall"))
				return;

			reflectedDirection = Vector3.Reflect(direction.normalized, hit.normal);
			if (count < PathBuffer.Count)
			{
				PathBuffer[count].direction = reflectedDirection;
			}
			else
			{
				var node = new Node();
				node.direction = reflectedDirection;
				node.target = hit.point;
				PathBuffer.Add(node);
			}

			count++;
			Debug.DrawRay(hit.point, reflectedDirection, Color.green);
			RecursiveRaycast(reflectedDirection, hit.point,count);
		}
	}
}
