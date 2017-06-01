using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fixate : MonoBehaviour {
    public bool fixatePosition;
    public bool fixateRotation;

    Vector3 position;
    Quaternion rotation;

	void Start () {
        position = transform.position;
        rotation = transform.rotation;
	}
	
	void Update () {
        if (fixatePosition)
            transform.position = position;
        if (fixateRotation)
            transform.rotation = rotation;

	}
}
