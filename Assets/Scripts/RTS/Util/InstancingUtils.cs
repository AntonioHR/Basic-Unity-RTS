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
        public static T AddWithPreemptiveExecution<T>(this GameObject owner, Action<T> action) where T : MonoBehaviour
        {
            owner.SetActive(false);
            var component = owner.AddComponent<T>();
            action(component);
            owner.SetActive(true);
            return component;
        }
    }
}
