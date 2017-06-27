using RTS.World.Squads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.World.UnitBehavior
{
    public class UnitSquadHandler : SquadElement
    {
        private Unit unit;


        public override GameObject Owner
        {
            get
            {
                return unit.Owner;
            }
        }
        public override Unit Unit { get { return unit; } }
        public override Team Team{ get { return unit.Team; } }

        public event Action OnSelected;
        public event Action OnDeselected;

        public UnitSquadHandler(Unit unit)
        {
            this.unit = unit;
            Squad.Create(unit.Team).AddUnit(this);
        }


        internal override void OnGroupDeselected()
        {
            if (OnDeselected != null)
                OnDeselected();
        }

        internal override void OnGroupSelected()
        {
            if (OnSelected != null)
                OnSelected();
        }

        internal override void OnGroupChanged(Squad oldGroup)
        {

        }
    }
}
