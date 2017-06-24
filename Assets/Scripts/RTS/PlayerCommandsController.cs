using RTS.World.Groups;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RTS
{
    public class PlayerCommandsController : MonoBehaviour
    {
        public PlayerCommandsController Current { get; private set; }
        
        List<SelectionGroup> selection;
        List<IHighlightable> highlight;


        //public int SelectionCount { get { return selection.Count; } }
        public int HighlightCount { get { return highlight.Count; } }


        void Awake()
        {
            Debug.Assert(this == FindObjectOfType<PlayerCommandsController>());
            Current = this;
        }
        void Start()
        {
            selection = new List<SelectionGroup>();
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
            var selectableGroups = units.Select(x => x.Group).Distinct();

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
            var result = new SelectionGroup();
            foreach (var group in selection)
            {
                group.MergeInto(result);
            }

            selection = new List<SelectionGroup>();
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
            foreach (var group in selection)
            {
                var targetReceivers = group.Units.Select(x => x.Owner.GetComponent<ITargetReceiver>())
                    .Where(x=>x != null).ToList();
                foreach (var targetReceiver in targetReceivers)
                {
                    Debug.LogFormat("Setting target {0} -> {1}", targetReceiver, targetable);
                    targetReceiver.SetTarget(targetable, point);
                }
                
            }
        }
    }
}