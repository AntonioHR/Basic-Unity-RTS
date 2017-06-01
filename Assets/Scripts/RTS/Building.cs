using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class Building : MonoBehaviour, IBuilding
    {
        public bool Targetable  {  get { return true; } }
        public GameObject Owner { get { return gameObject; } }

        public void Target(ITargetReceiver targetReceiver)
        {
            throw new System.NotImplementedException();
        }

        [System.Serializable]
        public class Settings
        {
            public float MaxHealth = 10;
        }
        public Settings settings;




    }
}