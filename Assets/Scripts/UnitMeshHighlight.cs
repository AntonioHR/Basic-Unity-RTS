using RTS.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMeshHighlight : MonoBehaviour {
    public Material idle;
    public Material highlight;

    [Space]
    public MeshRenderer MeshRenderer;

    [Space]
    public Unit unit;

	void Start () {
        unit.OnHighlightOn += unit_OnHighlightOn;
        unit.OnHighlightOff += unit_OnHighlightOff;
        MeshRenderer.material = idle;
	}

    void unit_OnHighlightOff()
    {
        MeshRenderer.material = idle;
    }

    void unit_OnHighlightOn()
    {
        MeshRenderer.material = highlight;
    }
}
