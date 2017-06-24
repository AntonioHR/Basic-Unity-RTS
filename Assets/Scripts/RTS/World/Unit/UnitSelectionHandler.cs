using RTS.World.Groups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.World.Units
{
    public class UnitSelectionHandler : SelectionUnitBase
    {
        private Unit unit;

        public override GameObject Owner
        {
            get
            {
                return unit.Owner;
            }
        }

        public event Action OnSelected;
        public event Action OnDeselected;

        public UnitSelectionHandler(Unit unit) : base(new SelectionGroup())
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

        internal override void OnGroupChanged(SelectionGroup oldGroup)
        {

        }
    }
}
