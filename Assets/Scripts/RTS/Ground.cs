using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class Ground : MonoBehaviour, IGround
    {
        public GameObject Owner { get { return gameObject; } }
        public bool Targetable { get { return true; } }
        public Vector3 position { get { return transform.position; } }
        public event System.Action OnDestroyed;
        


        void Start()
        {

        }
        
        void Update()
        {

        }
        void OnDestroy()
        {
            if (OnDestroyed != null)
                OnDestroyed();
        }


        public void TargetBy(ITargetReceiver targetReceiver)
        {
        }



    }
}