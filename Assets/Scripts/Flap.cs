using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flap : MonoBehaviour
{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Stop()
    {
        Debug.Log("Stopping Movement");
        if(animator != null)
            animator.enabled = false;
    }

}
