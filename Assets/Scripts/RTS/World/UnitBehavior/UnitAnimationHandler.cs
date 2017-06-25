using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.World.UnitBehavior
{
    [RequireComponent(typeof(Animator))]
    public class UnitAnimationHandler : MonoBehaviour {
        Animator animator;

        public event System.Action OnHitFrame;

        void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void HitFrame()
        {
            if (OnHitFrame != null)
                OnHitFrame();
        }

        internal void SetAttacking(bool value)
        {
            animator.SetBool("WantsToAttack", value);
        }
    }
}
