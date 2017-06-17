using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.World.Buildings
{
    [CreateAssetMenu()]
    public class BuildingDescriptor: ScriptableObject
    {
        public Building.Settings settings;
    }
}
