using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [SerializeField]
    float verticalVelocity;

    private void OnTriggerEnter(Collider other)
    {
        //other.attachedRigidbody.velocity = (verticalVelocity * Vector3.up);
    }
}
