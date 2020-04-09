using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour,IDamageable
{
	public static string BALL_FOLDER = "Balls/";

	[SerializeField]
	BallType ballType;
	[SerializeField]
	float rotationSpeed;
	[SerializeField]
    float ballSpeed;
    [SerializeField]
    bool shot;
    [SerializeField]
    float force;
	[SerializeField]
	float hp;
	[SerializeField]
	BallData data;
	[SerializeField]
	List<ParticleSystem> impactParticles;

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
	int impacts;


	private void Start()
    {
        cachedTransform = transform;
        cachedRigidbody = GetComponent<Rigidbody>();
        cachedCollider = GetComponent<SphereCollider>();

	    data = Resources.Load<BallData>(BALL_FOLDER + ballType.ToString());
		hp = data.hp;
		ballSpeed = data.speed;
		rotationSpeed = data.rotationSpeed;
	}

	public ParticleSystem GetImpactParticle()
	{
		impacts++;
		if (impacts % 2 == 0)
		{
			return impactParticles[0];
		}
		return impactParticles[1];
	}
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag.Equals("Wall") || collision.collider.tag.Equals("Flap"))
		{
			var contact = collision.GetContact(0);
			nextDirection = Vector3.Reflect(currentDirection.normalized, contact.normal);
			rotationAxis = Vector3.Cross(nextDirection, Vector3.up);
			//deltaRotation = Quaternion.Euler(rotationSpeed * Time.fixedDeltaTime * rotationAxis);
			deltaRotation = Quaternion.AngleAxis(rotationSpeed * Time.fixedDeltaTime, -rotationAxis);
			currentDirection = nextDirection;
			ballSpeed -= (ballSpeed * data.deceleration);
			var particleSystem = GetImpactParticle();
			particleSystem.transform.position = contact.point;
			particleSystem.transform.LookAt(cachedTransform.position);
			particleSystem.Play();
		}

		if (collision.collider.tag.Equals("Hole"))
		{
			Game.Instance.LoadNextLevel();
		}

		var damageble = collision.collider.GetComponent<IDamageable>();
		if (damageble != null)
			damageble.TakeDamage(data.damage);

	}

	public void Shot(Vector3 direction,float ballSpeed,float rotationSpeed)
	{
		//this.ballSpeed     = ballSpeed;
		//this.rotationSpeed = rotationSpeed;
		currentDirection   = direction;
		rotationAxis  = Vector3.Cross(currentDirection.normalized, Vector3.up);
		deltaRotation = Quaternion.AngleAxis(rotationSpeed * Time.fixedDeltaTime, -rotationAxis); //Quaternion.Euler();
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

			if (ballSpeed < 10)
				Game.Instance.Reload();
		}
	}

	public void TakeDamage(int damage)
	{
		hp -= damage;
		if (hp < 0)
			Game.Instance.Reload();
	}

}
