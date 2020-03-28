using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    [SerializeField]
    float ballSpeed;
    [SerializeField]
    bool shot;
    [SerializeField]
    float force;
    Vector3 currentDirection;
    Vector3 currentTarget;
    List<Node> pathBufferReference;
    int nodeCount;
    int nodeCleared;
    Transform cachedTransform;

    private void Start()
    {
        cachedTransform = transform;
    }

    private void OnTriggerEnter (Collider collider)
    {
		if (collider.tag.Equals("Wall"))
		{
			Game.Instance.Reload();
		}

		if (collider.tag.Equals("Hole"))
		{
			Game.Instance.LoadNextLevel();
		}
	}

    
    public void Shot(List<Node> path,int count)
    {
        if (path.Count < 0)
            return;

        shot = true;
        nodeCleared = 0;
        pathBufferReference = path;
        nodeCount = count;
        //currentDirection = pathBufferReference[nodeCleared].direction;
        currentTarget = pathBufferReference[nodeCleared].target;
    }

    void Update()
    {
        if (shot)
        {
            //FuriousPlay.Mechanics.MovePosition(transform, currentTarget, ballSpeed);
            //FuriousPlay.Mechanics.MoveDirection(cachedTransform, currentDirection, ballSpeed);
            
            cachedTransform.position = Vector3.MoveTowards(cachedTransform.position, currentTarget, ballSpeed * Time.deltaTime);

            if (Vector3.Distance(cachedTransform.position, currentTarget) < 0.001f)
            {
                nodeCleared++;
                if (nodeCleared < nodeCount)
                {
                    currentTarget = pathBufferReference[nodeCleared].target;
                    //currentDirection = pathBufferReference[nodeCleared].direction;
                }
            }

            for (int i = 0; i < nodeCount - 1; i++)
            {
                Debug.DrawLine(pathBufferReference[i].target, pathBufferReference[i + 1].target, Color.green);
            }
        }
    }

  
}
