using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RTS.World.Squads;
using UnityEngine;
using RTS.World;

namespace RTS.AI
{
    //Tem que ter isso aqui pra ter o menu de criar asset
    [CreateAssetMenu()]

    class DummyAIStrategy: AIStrategy
    {
        //Se quiser adicionar estado, tem que fazer algo assim
        class State
        {
            float fear;
        }
        Dictionary<Squad, State> states;

        public override void Start()
        {
            Debug.Log("potato");
            states = new Dictionary<Squad, State>();
        }
        //



        //Aqui vc coloca sua lógica
        public override void Step(Squad squad)
        {
            if (squad.TargetInfo == null)
            {
                return;
            }

            if (squad.TargetInfo.Target == null)
            {
                foreach (var squaddie in squad.Units)
                {
                    squaddie.CurrentAction = ActionInfo.MoveAction(squad.TargetInfo.Position);
                }
            } else
            { 
                foreach (var squaddie in squad.Units)
                {
                    squaddie.CurrentAction = ActionInfo.AttackAction(squad.TargetInfo.Target);
                }
            }
        }
    }
}
