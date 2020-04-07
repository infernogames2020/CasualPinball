using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
	[SerializeField]
	float rotationSpeed;
	[SerializeField]
    float ballSpeed;
    [SerializeField]
    bool shot;
    [SerializeField]
    float force;
	[SerializeField]
	float ballLife;

    Vector3 currentDirection;
	Vector3 rotationAxis;
	Quaternion deltaRotation;
    Vector3 currentTarget;
	Vector3 nextTarget;
	Vector3 nextDirection;
    List<Node> pathBufferReference;
    int nodeCount;
    int nodeCleared;
    Transform cachedTransform;
    Rigidbody cachedRigidbody;
	SphereCollider cachedCollider;
    Vector3 tempVector;
	Ray shootRay;
	RaycastHit hit;
	

    private void Start()
    {
        cachedTransform = transform;
        cachedRigidbody = GetComponent<Rigidbody>();
        cachedCollider = GetComponent<SphereCollider>();

	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag.Equals("Wall") || collision.collider.tag.Equals("Flap"))
		{
			var contact = collision.GetContact(0);
			nextDirection = Vector3.Reflect(currentDirection.normalized, contact.normal);
			rotationAxis = Vector3.Cross(nextDirection, Vector3.up);
			deltaRotation = Quaternion.Euler(rotationSpeed * Time.fixedDeltaTime * rotationAxis);
			currentDirection = nextDirection;
		}

		if (collision.collider.tag.Equals("Hole"))
		{
			Game.Instance.LoadNextLevel();
		}
	}

	public void Shot(Vector3 direction,float ballSpeed,float rotationSpeed)
	{
		this.ballSpeed     = ballSpeed;
		this.rotationSpeed = rotationSpeed;
		currentDirection   = direction;
		rotationAxis  = Vector3.Cross(currentDirection, Vector3.up);
		deltaRotation = Quaternion.Euler(rotationSpeed * Time.fixedDeltaTime * rotationAxis);
		Game.Instance.StopMovement();
		shot = true;
	}


	public void Shot(List<Node> path,int count)
    {
        if (path.Count < 0)
            return;
        shot = true;
        nodeCleared = 0;
        pathBufferReference = path;
        nodeCount = count;
        currentTarget = pathBufferReference[nodeCleared].target;
    }

	void FixedUpdate()
	{
		if (shot)
		{
			//tempVector = Vector3.MoveTowards(cachedTransform.position, currentTarget, ballSpeed * Time.fixedDeltaTime);
			tempVector = cachedTransform.position + (ballSpeed * Time.fixedDeltaTime * currentDirection);
			cachedRigidbody.MovePosition(tempVector);
			cachedRigidbody.MoveRotation(cachedRigidbody.rotation * deltaRotation);

		}
	}
  
}
