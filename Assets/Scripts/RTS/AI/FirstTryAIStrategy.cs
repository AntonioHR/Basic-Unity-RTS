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

    class FirstTryAIStrategy : AIStrategy
    {
        class State
        {
            public bool animosity; //if false, it's defensive. If true, it's aggressive
            public State (bool animosity)
            {
                this.animosity = animosity;
            }
        }
        Dictionary<Squad, State> states;

        public override void Start()
        {
            states = new Dictionary<Squad, State>();
        }

        private HashSet<IHittable> FindSquadEnemies(Squad squad)
        {
            HashSet<IHittable> finalEnemiesList= new HashSet<IHittable>();

            foreach (var unit in squad.Units)
            {
                Collider[] hitColliders = Physics.OverlapSphere(unit.position, 5);
                int i = 0;
                while (i < hitColliders.Length)
                {
                    IHittable[] seenEnemies = hitColliders[i].gameObject.GetComponents<IHittable>();
                    int j = 0;
                    while(j<seenEnemies.Length)
                    {
                        finalEnemiesList.Add(seenEnemies[j]);
                        j++;
                    }
                    i++;
                }  
            }
            return finalEnemiesList;
        }
        
        public override void Step(Squad squad)
        {            
            bool animosity;
            try
            {
                animosity=states[squad].animosity;
            }
            catch (KeyNotFoundException)
            {
                animosity = true; //this is for testing purposes, usually it should be 'false' here
                states.Add(squad, new State(animosity)); 
            }
            if (animosity==false && squad.TargetInfo != null && squad.TargetInfo.Position != null)
            {// se nós estamos em modo defensivo (animosity==false) e não há alvo-posição, não há nada a fazer
                foreach (var squaddie in squad.Units)
                {
                    squaddie.CurrentAction = UnitAction.MoveAction(squad.TargetInfo.Position);
                }
            }

            if (animosity == true)
            {
                HashSet<IHittable> enemiesList = this.FindSquadEnemies(squad);
                if (enemiesList.Count == 0)
                {
                    if (squad.TargetInfo != null && squad.TargetInfo.Position != null)
                    {
                        foreach (var squaddie in squad.Units)
                        {
                            squaddie.CurrentAction = UnitAction.MoveAction(squad.TargetInfo.Position);
                        }
                    }
                    else
                    {
                        foreach (var squaddie in squad.Units)
                        {
                            squaddie.CurrentAction = UnitAction.IdleAction();
                        }
                    }
                }
                else
                {
                    foreach (var squaddie in squad.Units)
                    {
                        float smallestDistance = float.PositiveInfinity;
                        IHittable currentTarget = null;
                        foreach (var enemy in enemiesList)
                        {
                            float currentDistance = Vector3.Distance(enemy.position, squaddie.position);
                            if (currentDistance < smallestDistance)
                            {
                                smallestDistance = currentDistance;
                                currentTarget = enemy;
                            }
                        }
                        squaddie.CurrentAction = UnitAction.AttackAction(currentTarget);
                    }
                }
            }
        }
    }
}
