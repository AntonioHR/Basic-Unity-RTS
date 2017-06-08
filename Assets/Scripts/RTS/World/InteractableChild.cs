using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.World
{
    public class InteractableChild: MonoBehaviour,  IInteractive
    {
        public GameObject owner;
        public event Action OnDestroyed;

        public GameObject Owner
        {
            get { return owner; }
        }

        void OnDestroy()
        {
            if(OnDestroyed != null)
            {
                OnDestroyed();
            }
        }

    }
}
