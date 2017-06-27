using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.World
{
    public enum ActionMode { Empty, Attack, Move}
    public class ActionInfo
    {
        public ActionMode Mode { get; private set; }
        public IHittable Target { get; private set; }
        public Vector3 position { get; private set; }


        private ActionInfo(ActionMode mode, IHittable target, Vector3 position)
        {
            this.Mode = mode;
            this.Target = target;
            this.position = position;
        }

        public static ActionInfo AttackAction(IHittable target)
        {
            return new ActionInfo(ActionMode.Attack, target, target.position);
        }
        public static ActionInfo AttackAction(IHittable target, Vector3 position)
        {
            return new ActionInfo(ActionMode.Attack, target, position);
        }
        
        public static ActionInfo MoveAction(Vector3 position)
        {
            return new ActionInfo(ActionMode.Move, null, position);
        }

        public static ActionInfo EmptyAction()
        {
            return new ActionInfo(ActionMode.Empty, null, new Vector3());
        }
    }
}
