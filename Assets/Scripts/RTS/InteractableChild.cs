using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS
{
    public class InteractableChild: MonoBehaviour,  IInteractive
    {
        public GameObject owner;

        public GameObject Owner
        {
            get { return owner; }
        }
    }
}
