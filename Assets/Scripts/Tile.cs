using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour,IDamageable
{
	public static string TILE_FOLDER = "Tiles/";
    private Animator animator;
	public TileType tileType;
	public TileData data;
	public int hp;

	void Start()
    {
		animator = GetComponent<Animator>();
		data = Resources.Load<TileData>(TILE_FOLDER+tileType.ToString());
		hp = data.hp;
		//if(data != null)
		//	Debug.Log("Found");
		//else
		//	Debug.Log("Not Found");
	}

	public void Stop()
    {
        Debug.Log("Stopping Movement");
        if(animator != null)
            animator.enabled = false;
    }

	private void OnCollisionEnter(Collision collision)
	{
		var damageble = collision.collider.GetComponent<Ball>();
		if (damageble != null)
			damageble.TakeDamage(data.damage);
		
	}

	public void TakeDamage(int damage)
	{
		//hp -= damage;
		//if (hp < 0)
		//	Destroy(gameObject);
	}
}
