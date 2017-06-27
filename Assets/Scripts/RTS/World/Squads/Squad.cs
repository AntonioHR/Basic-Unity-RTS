using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.World.Squads
{
    public class Squad
    {
        List<SquadElement> units;

        public Team Team { get; private set; }

        public TargetInformation TargetInfo { get; private set; }

        public IEnumerable<Unit> Units { get { return units.AsEnumerable().Select(x => x.Unit); } }

        private Squad(Team Team)
        {
            this.Team = Team;
            units = new List<SquadElement>();
        }

        public void AddUnit(SquadElement unit)
        {
            Debug.Assert(unit.Team == Team);
            units.Add(unit);
            unit.OnUnitDestroyed += RemoveUnit;
            unit.Squad = this;
        }

        public void RemoveUnit(SquadElement unit)
        {
            UnityEngine.Debug.Assert(unit.Squad == this);
            units.Remove(unit);
            unit.OnUnitDestroyed -= RemoveUnit;
            unit.Squad = null;
        }

        public Squad MergeInto(Squad group)
        {
            Debug.Assert(group.Team == Team);
            for (int i = units.Count-1; i >= 0; i--)
            {
                var unit = units[i];
                this.RemoveUnit(unit);
                group.AddUnit(unit);
            }
            Manager.Instance.RemoveSquad(this);
            return group;
        }

        public void Select() {
            foreach (var unit in units)
            {
                unit.OnGroupSelected();
            }
        }

        public void Deselect()
        {
            foreach (var unit in units)
            {
                unit.OnGroupDeselected();
            }
        }

        internal void setTarget(TargetInformation info)
        {
            this.TargetInfo = info;
        }

        public static Squad Create(Team team)
        {
            return Manager.Instance.CreateSquad(team);
        }
        public static IEnumerable<Squad> AllSquads
        {
            get
            {
                return Manager.Instance.Squads;
            }
        }

        public class Manager
        {
            private static Manager singleton;
            public static Manager Instance
            {
                get
                {
                    if (singleton == null)
                        singleton = new Manager();
                    return singleton;
                }
            }


            List<Squad> squads;


            private Manager()
            {
                squads = new List<Squad>();
            }
            
            internal IEnumerable<Squad> Squads
            {
                get { return squads.AsEnumerable(); }
            }

            internal Squad CreateSquad(Team team)
            {
                var squad = new Squad(team);
                squads.Add(squad);
                return squad;
            }
            internal void RemoveSquad(Squad squad)
            {
                squads.Remove(squad);
            }
        }

        internal bool CanTarget(TargetInformation targetInformation)
        {
            return targetInformation.IsValid && (targetInformation.Target == null || targetInformation.Target.Team != Team);
        }
    }

    public abstract class SquadElement: ISelectionUnit
    {
        private Squad group;
        public Squad Squad
        {
            get { return group; }
            internal set
            {
                var oldGroup = group;
                group = value;
                OnGroupChanged(oldGroup);
            }
        }

        public virtual bool Selectable
        {
            get { return true; }
        }

        public abstract GameObject Owner { get; }
        public abstract Unit Unit { get; }
        public abstract Team Team { get; }


        public event Action OnDestroyed;
        public event Action<SquadElement> OnUnitDestroyed;

        public SquadElement()
        {
        }
            

        internal abstract void OnGroupChanged(Squad oldGroup);
        internal abstract void OnGroupSelected();
        internal abstract void OnGroupDeselected();
    }
    
    public class TargetInformation
    {
        public IHittable Target { get; private set; }
        public Vector3 Position { get; private set; }
        public bool IsValid { get; private set; }
        public TargetInformation(IHittable target, Vector3 position)
        {
            this.Target = target;
            this.Position = position;
            IsValid = true;
            if(Target != null)
                Target.OnDestroyed += Target_OnDestroyed;
        }

        void Target_OnDestroyed()
        {
            Target = null;
            IsValid = false;
        }
    }
}
