using RTS.World.Squads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.AI
{
    public abstract class AIStrategy: ScriptableObject
    {
        public abstract void Step(Squad squad);
    }
}
