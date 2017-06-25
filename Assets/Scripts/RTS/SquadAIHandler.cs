using RTS.AI;
using RTS.World.Squads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS
{
    public class SquadAIHandler : MonoBehaviour
    {
        public AIStrategy strategy;

        void Update ()
        {
            foreach (var squad in Squad.AllSquads)
            {
                strategy.Step(squad);
            }
        }
    }
}
