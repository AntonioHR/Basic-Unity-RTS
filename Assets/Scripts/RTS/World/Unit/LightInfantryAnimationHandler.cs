using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.World.Units
{
    [RequireComponent(typeof(Animator))]
	public class LightInfantryAnimationHandler : MonoBehaviour {
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
			animator.SetBool("Attack", value);
        }

		internal void SetSelected(bool value)
		{
			animator.SetBool("Selected", value);
		}

		internal void SetWalking(bool value)
		{
			animator.SetBool("Walking", value);
		}
    }
}
