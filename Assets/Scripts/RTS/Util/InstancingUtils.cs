using RTS.World.UnitBehavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.Util
{
    public static class InstancingUtils
    {
        public static T CreateWithPreemptiveExecution<T>(Action<T> action) where T : MonoBehaviour
        {
            GameObject obj = new GameObject();
            obj.SetActive(false);
            var component = obj.AddComponent<T>();
            action(component);
            obj.SetActive(true);
            return component;
        }
        public static GameObject InstantiateWithPreemptiveExecution(this GameObject prefab, Action<GameObject> action)
        {
            var temp = prefab.activeSelf;
            prefab.SetActive(false);
            GameObject obj = GameObject.Instantiate(prefab);
            prefab.SetActive(temp);

            obj.SetActive(false);
            action(obj);
            obj.SetActive(true);
            return obj;
        }
        public static T AddWithPreemptiveExecution<T>(this GameObject owner, Action<T> action) where T : MonoBehaviour
        {
            owner.SetActive(false);
            var component = owner.AddComponent<T>();
            action(component);
            owner.SetActive(true);
            return component;
        }
        public static GameObject CreateUnitInstance(this UnitClassData classData, Team team, Vector3 position, Quaternion rotation)
        {
            var unitAssemblerObj = UnitAssembler.UnitPrefab.InstantiateWithPreemptiveExecution((obj) =>
            {
                var assembler = obj.GetComponent<UnitAssembler>();
                assembler.classData = classData;
                assembler.team = team;
                obj.transform.position = position;
                obj.transform.rotation = rotation;
            });

            return unitAssemblerObj;
        }
    }
}
