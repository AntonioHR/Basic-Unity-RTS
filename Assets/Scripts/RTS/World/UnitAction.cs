using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.World
{
    public enum ActionMode { Attack, Move, Idle }
    public class UnitAction
    {
        public bool IsValid { get; private set; }
        public ActionMode Mode { get; private set; }
        public IHittable Target { get; private set; }
        public Vector3? position { get; private set; }


        private UnitAction(ActionMode mode, IHittable target, Vector3? position)
        {
            this.Mode = mode;
            this.Target = target;
            this.IsValid = Mode != ActionMode.Attack || target != null;
            this.IsValid &= Mode == ActionMode.Idle || position != null;
            if(Target != null)
                Target.OnDestroyed += Target_OnDestroyed;
            this.position = position;
        }

        void Target_OnDestroyed()
        {
            Target = null;
            IsValid = false;
        }

        public static UnitAction AttackAction(IHittable target)
        {
            return new UnitAction(ActionMode.Attack, target, target.position);
        }
        public static UnitAction AttackAction(IHittable target, Vector3 position)
        {
            return new UnitAction(ActionMode.Attack, target, position);
        }

        public static UnitAction MoveAction(Vector3 position)
        {
            return new UnitAction(ActionMode.Move, null, position);
        }

        internal static UnitAction IdleAction()
        {
            return new UnitAction(ActionMode.Idle, null, null);
        }
    }
}
