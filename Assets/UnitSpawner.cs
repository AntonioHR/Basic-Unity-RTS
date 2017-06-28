using RTS.Util;
using RTS.World;
using RTS.World.UnitBehavior;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class UnitSpawner : MonoBehaviour {
    public UnitClassData classData;
    Building building;

    public float interval = .5f;
    public Transform spawnPoint;

    public event Action<GameObject> OnSpawn;

	void Awake () {
        building = GetComponent<Building>();
	}
    void OnEnable()
    {
        StartCoroutine(SpawnCoroutine());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }


    void OnDrawGizmosSelected()
    {
        if(spawnPoint == null)
            return;

        Gizmos.color = Color.red;
        Vector3 size = Vector3.one;
        Gizmos.DrawCube(spawnPoint.position + Vector3.up * (size.y *.5f), Vector3.one);
    }

    IEnumerator SpawnCoroutine()
    {
        while (enabled)
        {
            yield return new WaitForSeconds(interval);
            SpawnUnit();
        }
        yield return null;
    }

    public GameObject SpawnUnit()
    {
        var result = classData.CreateUnitInstance(building.Team, spawnPoint.position, spawnPoint.rotation);
        if (OnSpawn != null)
            OnSpawn(result);
        return result;
    }
}
