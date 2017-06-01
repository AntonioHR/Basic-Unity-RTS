using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class Ground : MonoBehaviour, IGround
    {
        public GameObject Owner { get { return gameObject; } }
        public bool Targetable { get { return true; } }
        


        void Start()
        {

        }
        
        void Update()
        {

        }



        public void Target(ITargetReceiver targetReceiver)
        {
        }

    }
}