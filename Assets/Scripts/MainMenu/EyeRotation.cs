using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeRotation : MonoBehaviour
{
    public float rotationAmount;

    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotationAmount * Time.fixedDeltaTime);
    }
}
