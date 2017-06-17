using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.Util
{
    public class InstancingUtils
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
    }
}
