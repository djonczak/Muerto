using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform myTarget;
    public Vector3 mapMin;
    public Vector3 mapMax;

    void Update()
    {
        if (myTarget != null)
        {
            Vector3 targPos = myTarget.position;

            transform.position = targPos;

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, mapMin.x, mapMax.x), Mathf.Clamp(transform.position.y, mapMin.y, mapMax.y), Mathf.Clamp(transform.position.z, mapMin.z, mapMax.z));
        
        }
    }
}

