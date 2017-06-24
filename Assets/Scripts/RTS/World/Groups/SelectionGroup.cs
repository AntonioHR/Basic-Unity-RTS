using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.World.Groups
{
    public class SelectionGroup
    {
        List<SelectionUnitBase> units;

        public IEnumerable<SelectionUnitBase> Units { get { return units.AsEnumerable(); } }

        public SelectionGroup()
        {
            units = new List<SelectionUnitBase>();
        }

        public void AddUnit(SelectionUnitBase unit)
        {
            units.Add(unit);
            unit.Group = this;
        }

        public void RemoveUnit(SelectionUnitBase unit)
        {
            UnityEngine.Debug.Assert(unit.Group == this);
            units.Remove(unit);
            unit.Group = null;
        }

        public SelectionGroup MergeInto(SelectionGroup group)
        {
            for (int i = units.Count-1; i >= 0; i--)
            {
                var unit = units[i];
                this.RemoveUnit(unit);
                group.AddUnit(unit);
            }
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
    }

    public abstract class SelectionUnitBase: ISelectionUnit
    {
        private SelectionGroup group;
        public SelectionGroup Group
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

        public event Action OnDestroyed;
        
        public SelectionUnitBase(SelectionGroup group)
        {
            group.AddUnit(this);
        }
            

        internal abstract void OnGroupChanged(SelectionGroup oldGroup);
        internal abstract void OnGroupSelected();
        internal abstract void OnGroupDeselected();
    }
}
