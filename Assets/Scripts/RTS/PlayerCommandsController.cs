using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class PlayerCommandsController : MonoBehaviour
    {
        List<Unit> selection;

        void Start()
        {
            selection = new List<Unit>();
        }

        void Update()
        {

        }
        public void TrySelect(Unit unit)
        {
            var list = new List<Unit>();
            if(unit != null)
                list.Add(unit);
            TrySelect(list);
        }
        public void TrySelect(List<Unit> units)
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
        public int SelectionCount { get { return selection.Count; } }
    }
}