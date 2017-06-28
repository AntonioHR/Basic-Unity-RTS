using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SpawnBuildingAnimation : MonoBehaviour {
    public UnitSpawner spawner;
    Animator animator;

	void Start () {
        animator = GetComponent<Animator>();
        spawner.OnSpawn += spawner_OnSpawn;
	}

    void spawner_OnSpawn(GameObject obj)
    {
        animator.SetTrigger("Spawn");
    }
}
