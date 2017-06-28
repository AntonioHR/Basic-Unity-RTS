using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.World.UnitBehavior
{
    [CreateAssetMenu(menuName = "RTS/New Class")]
    public class UnitClassData: ScriptableObject
    {
        //public RuntimeAnimatorController animationController;
        public GameObject model;
        //public string modelObjectName;
        public Unit.Settings settings;
    }
}
