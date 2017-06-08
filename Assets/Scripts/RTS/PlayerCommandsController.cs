using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class PlayerCommandsController : MonoBehaviour
    {
        public PlayerCommandsController Current { get; private set; }

        List<ISelectable> selection;
        List<IHighlightable> highlight;


        public int SelectionCount { get { return selection.Count; } }
        public int HighlightCount { get { return highlight.Count; } }


        void Awake()
        {
            Debug.Assert(this == FindObjectOfType<PlayerCommandsController>());
            Current = this;
        }
        void Start()
        {
            selection = new List<ISelectable>();
            highlight = new List<IHighlightable>();
        }

        public void TrySelect(ISelectable unit)
        {
            var list = new List<ISelectable>();
            if(unit != null)
                list.Add(unit);
            TrySelect(list);
        }
        public void TrySelect(List<ISelectable> units)
        {
            var selectable = units.FindAll(x=>x.Selectable);
            foreach (var s in selection)
            {
                if (!selectable.Contains(s))
                    s.Deselect();
            }
            foreach (var s in selectable)
            {
                if (!selection.Contains(s))
                    s.Select();
            }
            selection = selectable;
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
            List<ITargetReceiver> targetReceivers = new List<ITargetReceiver>();
            foreach (var item in selection)
            {
                var convert = item as ITargetReceiver;
                if (convert != null)
                    targetReceivers.Add(convert);
            }
            Debug.LogFormat("Targeting {0}", targetable);
            targetReceivers.ForEach(x => x.SetTarget(targetable, point));
        }
    }
}