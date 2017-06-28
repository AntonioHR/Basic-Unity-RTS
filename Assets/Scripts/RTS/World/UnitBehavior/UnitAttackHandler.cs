using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.World.UnitBehavior
{
    public class UnitAttackHandler
    {
        enum State { Free, Attacking }

        [Serializable]
        public class Settings
        {
            public float startRange = 1f;
            public float stayRange = 1.5f;
        }

        Settings settings;

        State state;
        Unit unit;
        UnitAnimationHandler animationHandler;
        IHittable target;
        

        public bool IsAttacking { get { return state == State.Attacking; } }

        public UnitAttackHandler(Unit unit, UnitAnimationHandler animationHandler, Settings settings)
        {
            this.unit = unit;
            this.animationHandler = animationHandler;
            this.settings = settings;
        }   


        public void StartAttacking(IHittable target)
        {
            this.target = target;
            this.state = State.Attacking;
            animationHandler.SetAttacking(true);
            animationHandler.OnHitFrame += ExecuteAttack;

        }
        public void StopAttacking()
        {
            this.target = null;
            this.state = State.Free;
            animationHandler.SetAttacking(false);
            animationHandler.OnHitFrame -= ExecuteAttack;
        }

        public bool IsInRange(Vector3 targetPos)
        {
            var distance = Mathf.Abs(Vector3.Distance(targetPos, unit.transform.position));
            var maxDistance = unit.Range +
                (state == State.Attacking ? settings.stayRange : settings.startRange);
            return distance < maxDistance;
        }
        void ExecuteAttack()
        {
            animationHandler.OnHitFrame -= ExecuteAttack;
            target.OnHit((int)unit.AttackDamage);
            StopAttacking();
        }
    }
}
