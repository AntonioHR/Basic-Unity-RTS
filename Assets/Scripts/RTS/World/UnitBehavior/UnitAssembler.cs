using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using RTS.Util;

namespace RTS.World.UnitBehavior
{
    public class UnitAssembler: MonoBehaviour
    {
        public UnitClassData classData;

        [Space]
        public Transform ModelOrigin;
        public SelectionIndicator SelectionIndicator;
        public Transform CanvasHolder;

        [Space]
        public NavMeshAgent agentNM;

        [Space]
        public Animator animator;
        public UnitAnimationHandler animationHandler;

        [Space]
        public Team team;

        public void Awake()
        {
            VerifyReferences();

            animator.runtimeAnimatorController = classData.animationController;

            var model = GameObject.Instantiate(classData.model, ModelOrigin);
            model.name = classData.modelObjectName;
            VerifyModel(model);
            foreach (var child in model.GetComponentsInChildren<ChildOfInteractiveGameObject>())
            {
                child.owner = gameObject;
            }


            VerifyModel(model);

            Unit unit = gameObject.AddWithPreemptiveExecution<Unit>((u) =>
                {
                    u.settings = classData.settings;
                    u.animationHandler = animationHandler;
                    u.StartTeam = team;
                });

            this.SelectionIndicator.unit = unit;

            Destroy(this);
        }

        private void VerifyModel(GameObject model)
        {
            Debug.Assert(model.GetComponentInChildren<Collider>() != null);
            Debug.Assert(model.GetComponentInChildren<ChildOfInteractiveGameObject>() != null);
        }

        private void VerifyReferences()
        {
            Debug.Assert(ModelOrigin != null);
            Debug.Assert(SelectionIndicator != null);
            Debug.Assert(CanvasHolder != null);

            Debug.Assert(agentNM != null);

            Debug.Assert(animator != null);
            Debug.Assert(animationHandler != null);

            Debug.Assert(classData != null);
        }
    }

}
