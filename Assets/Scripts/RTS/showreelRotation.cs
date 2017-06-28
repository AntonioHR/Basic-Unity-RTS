using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showreelRotation : MonoBehaviour {

	Transform t;

	// Use this for initialization
	void Start () {
		t = GetComponent<Transform>();	
	}
	
	// Update is called once per frame
	void Update () {
		t.Rotate (new Vector3 (0f, 0.8f, 0f));
	}
}
