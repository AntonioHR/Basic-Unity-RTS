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
        public override bool Destroyed { get { return Unit.Destroyed; } }

        public event Action OnSelected;
        public event Action OnDeselected;

        public UnitSquadHandler(Unit unit) : base(Squad.Create())
        {
            this.unit = unit;
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
