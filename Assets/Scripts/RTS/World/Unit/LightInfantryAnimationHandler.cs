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
			animator.speed = 1f;
        }

		internal void SetSelected(bool value)
		{
			animator.SetBool("Selected", value);
			animator.speed = 1f;
		}

		internal void SetWalking(bool value, float speed)
		{
			animator.SetBool("Walking", value);
			if (value != false) {
				if (speed > 1.75) {
					animator.speed = speed / 3.5f;
				} else {
					animator.speed = 1.0f;
				}
			}
			animator.SetFloat("Speed", speed);
		}
    }
}
