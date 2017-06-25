using RTS.World.Squads;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RTS
{
    public class PlayerCommandsController : MonoBehaviour
    {
        public PlayerCommandsController Current { get; private set; }
        
        List<Squad> selection;
        List<IHighlightable> highlight;

        public int HighlightCount { get { return highlight.Count; } }


        void Awake()
        {
            Debug.Assert(this == FindObjectOfType<PlayerCommandsController>());
            Current = this;
        }
        void Start()
        {
            selection = new List<Squad>();
            highlight = new List<IHighlightable>();
        }

        public void TrySelect(ISelectionUnit select)
        {
            var list = new List<ISelectionUnit>();
            if(select != null)
                list.Add(select);
            TrySelect(list);
        }
        public void TrySelect(List<ISelectionUnit> units)
        {
            var selectable = units.FindAll(x=>x.Selectable);
            var selectableGroups = units.Select(x => x.Squad).Distinct();

            var unselectedGroups = selection.Where(x => !selectableGroups.Contains(x));
            foreach (var group in unselectedGroups)
            {
                group.Deselect();
            }
            foreach (var newSelect in selectableGroups)
            {
                newSelect.Select();
            }
            selection = selectableGroups.ToList();
        }

        public void TryMergeSelection()
        {
            Squad.Create();
            var result = Squad.Create();
            foreach (var group in selection)
            {
                group.MergeInto(result);
            }

            selection = new List<Squad>();
            selection.Add(result);
            result.Select();
        }


        public void TryHighlight(IHighlightable unit)
        {
            var list = new List<IHighlightable>();
            if (unit != null)
                list.Add(unit);
            TryHighlight(list);
        }
        public void TryHighlight(List<IHighlightable> units)
        {
            var highlightable = units.FindAll(x => x.Highlightable);
            foreach (var s in highlight)
            {
                if (!highlightable.Contains(s))
                    s.HighlightOff();
            }
            foreach (var s in highlightable)
            {
                if (!highlight.Contains(s))
                    s.HighlightOn();
            }
            highlight = highlightable;
        }

        public void TryTarget(ITargetable targetable, Vector3 point)
        {
            foreach (var squad in selection)
            {
                squad.setTarget(new TargetInformation(targetable as IHittable, point));
            }
        }
    }
}