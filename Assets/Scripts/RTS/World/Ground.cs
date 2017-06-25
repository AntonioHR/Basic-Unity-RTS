using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.World
{
    public class Ground : MonoBehaviour, ITargetable, IInteractive
    {
        public GameObject Owner { get { return gameObject; } }
        public bool Targetable { get { return true; } }
        public Vector3 position { get { return transform.position; } }
        public event System.Action OnDestroyed;
        public bool Destroyed
        {
            get;
            private set;
        }




        void Start()
        {

        }
        
        void Update()
        {

        }
        void OnDestroy()
        {
            Destroyed = true;
            if (OnDestroyed != null)
                OnDestroyed();
        }



    }
}