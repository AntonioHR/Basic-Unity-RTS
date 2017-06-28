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

    class ThreeKindsAIStrategy : AIStrategy
    {
        class State
        {
            public bool animosity; //if false, it's defensive. If true, it's aggressive
            public State(bool animosity)
            {
                this.animosity = animosity;
            }
        }
        public int viewRadius = 10;
        private Dictionary<Squad, State> states;
        private Dictionary<Squad, Dictionary<IHittable, int>> numberOfSquaddiesOnEnemy; // given a squad and an enemy, this should return how many squaddies are already engaging that enemy
        private Dictionary<Squad, Dictionary<Unit, IHittable>> LockedEnemies; // com quem cada unidade de cada esquadrão está lutando
            
        public override void Start()
        {
            states = new Dictionary<Squad, State>();
            numberOfSquaddiesOnEnemy = new Dictionary<Squad, Dictionary<IHittable, int>>();
            LockedEnemies = new Dictionary<Squad, Dictionary<Unit, IHittable>>();
        }

        private HashSet<IHittable> FindSquadEnemies(Squad squad)
        {
            HashSet<IHittable> finalEnemiesList = new HashSet<IHittable>();

            foreach (var unit in squad.Units)
            {
                Collider[] hitColliders = Physics.OverlapSphere(unit.position, viewRadius);
                int i = 0;
                while (i < hitColliders.Length)
                {
                    IHittable[] seenEnemies = hitColliders[i].gameObject.GetComponents<IHittable>();
                    int j = 0;
                    while (j < seenEnemies.Length)
                    {
                        finalEnemiesList.Add(seenEnemies[j]);
                        j++;
                    }
                    i++;
                }
            }
            return finalEnemiesList;
        }

        private IHittable defineEnemy(Unit.ClassType squaddieType, HashSet<IHittable> enemiesList, Dictionary<IHittable, int>numberOfSiblingsOnEnemy, Unit squaddie)
        {
            IHittable currentTarget = null;
            switch (squaddieType)
            {
                case Unit.ClassType.Infantry:
                    float smallestDistance = float.PositiveInfinity;
                    foreach (var enemy in enemiesList)
                    {
                        if (numberOfSiblingsOnEnemy[enemy] < 3) //só atacar se tiver poucos "irmãos" atacando
                        {
                            float currentDistance = Vector3.Distance(enemy.position, squaddie.position);
                            if (currentDistance < smallestDistance)
                            {
                                smallestDistance = currentDistance;
                                currentTarget = enemy;
                            }
                        }
                    }
                    break;
                default:
                    currentTarget = null;
                    break;
            }
            return currentTarget;
        }

        public override void Step(Squad squad)
        {
            bool animosity;
            try
            {
                animosity = states[squad].animosity;
            }
            catch (KeyNotFoundException)
            {
                animosity = true; //this is for testing purposes, usually it should be 'false' here
                states.Add(squad, new State(animosity));
            }
            if (animosity == false && squad.TargetInfo != null && squad.TargetInfo.Position != null)
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
                    
                    Dictionary<Unit, IHittable> localLockedEnemies;
                    try
                    {
                        localLockedEnemies = LockedEnemies[squad];
                    }
                    catch
                    {
                        localLockedEnemies = new Dictionary<Unit, IHittable>();
                        LockedEnemies.Add(squad, localLockedEnemies);
                    }
                    foreach (var squaddie in squad.Units)
                    {
                        if (!localLockedEnemies.ContainsKey(squaddie))
                        {
                            localLockedEnemies.Add(squaddie, null);
                        }
                        //else if (localLockedEnemies[squaddie] != null && localLockedEnemies[squaddie].Destroyed)
                        //{
                        //    localLockedEnemies[squaddie] = null;
                        //}
                    }

                            Dictionary<IHittable, int> numberOfSiblingsOnEnemy;
                    try
                    {
                        numberOfSiblingsOnEnemy = numberOfSquaddiesOnEnemy[squad];
                    }
                    catch (KeyNotFoundException)
                    {
                        numberOfSiblingsOnEnemy = new Dictionary<IHittable, int>();
                        numberOfSquaddiesOnEnemy.Add(squad, numberOfSiblingsOnEnemy);
                    }

                    foreach (var enemy in enemiesList)
                    {
                        if (!numberOfSiblingsOnEnemy.ContainsKey(enemy))
                        {
                            numberOfSquaddiesOnEnemy[squad].Add(enemy, 0);
                        }
                    }

                    foreach (var squaddie in squad.Units)
                    {
                        if (localLockedEnemies[squaddie]==null) //then we have to search for an enemy
                        {
                            IHittable currentTarget = defineEnemy(squaddie.Type, enemiesList, numberOfSiblingsOnEnemy, squaddie);
                            if (currentTarget != null)
                            {
                                squaddie.CurrentAction = UnitAction.AttackAction(currentTarget);
                                numberOfSiblingsOnEnemy[currentTarget]++; // já foi checado antes se todos os inimigos tinham uma entrada no dicionário
                            }
                            else
                            {
                                //seguir os outros caras e ficar pronto pra iniciar combate.
                                //o comportamento a seguir é temporário!
                                if (squad.TargetInfo != null && squad.TargetInfo.Position != null)
                                {
                                    squaddie.CurrentAction = UnitAction.MoveAction(squad.TargetInfo.Position);
                                }
                            }
                            localLockedEnemies[squaddie] = currentTarget;
                        }
                        else
                        {
                            squaddie.CurrentAction = UnitAction.AttackAction(localLockedEnemies[squaddie]);
                        }
                    }
                }
            }

           
        }
    }
}
