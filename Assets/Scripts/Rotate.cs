using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed;
    Vector3 currentRotation;

    // Update is called once per frame
    void Update()
    {
        Vector3 previousRotation = transform.localEulerAngles;
        Vector3 newRotation = previousRotation + (Vector3.up * rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(newRotation);
    }
}
