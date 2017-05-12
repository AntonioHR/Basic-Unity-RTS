using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Unit : MonoBehaviour {
    public Material idle;
    public Material selected;

    MeshRenderer meshRenderer;


    public bool Selectable
    {
        get
        {
            return true;
        }
    }

	void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = idle;
	}
	void Update () {
		
	}



    public void Deselect()
    {
        meshRenderer.material = idle;
    }
    public void Select()
    {
        meshRenderer.material = selected;
    }



}
