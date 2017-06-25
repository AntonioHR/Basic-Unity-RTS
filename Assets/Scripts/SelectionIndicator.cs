using RTS.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionIndicator : MonoBehaviour {
    public Unit unit;

	void Start () {
        unit.OnSelected += Unit_OnSelected;
        unit.OnDeselected += Unit_OnDeselected;
        gameObject.SetActive(false);
	}

    private void Unit_OnDeselected()
    {
        gameObject.SetActive(false);
    }

    private void Unit_OnSelected()
    {
        gameObject.SetActive(true);
    }
    
    void Update () {
		
	}
}
