using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAnim : MonoBehaviour {

	public Animator animator;
	public bool dead, walk, damage, selected = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("q")) 
		{
			selected = !selected;
		}
		if (Input.GetKeyDown ("w")) 
		{
			dead = !dead;
		}
		if (Input.GetKeyDown ("e")) 
		{
			walk = !walk;
		}
		if (Input.GetKeyDown ("r")) 
		{
			damage = !damage;
		}
	}

	void LateUpdate() {
		animator.SetBool("Selected", selected);
		animator.SetBool("Dead", dead);
		animator.SetBool("Walk", walk);
		animator.SetBool("TookDamage", selected);
	}
}
