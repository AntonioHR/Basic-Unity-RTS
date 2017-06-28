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
        public static GameObject UnitPrefab { get; set; }


        public Transform ModelOrigin;
        public SelectionIndicator SelectionIndicator;
        public Transform CanvasHolder;
        public NavMeshAgent agentNM;
        public Animator animator;
        public UnitAnimationHandler animationHandler;

        [Space]
        public Team team;
        public UnitClassData classData;

        public void Awake()
        {
            VerifyReferences();

            Unit unit = AssembleUnit();

            this.SelectionIndicator.unit = unit;

            GameObject model = AssembleModel(unit);

            VerifyModel(model);

            animator.runtimeAnimatorController = classData.animationController;

            Destroy(this);
        }

        private Unit AssembleUnit()
        {
            Unit unit = gameObject.AddWithPreemptiveExecution<Unit>((u) =>
            {
                u.settings = classData.settings;
                u.animationHandler = animationHandler;
                u.StartTeam = team;
            });
            return unit;
        }

        private GameObject AssembleModel(Unit unit)
        {
            var model = GameObject.Instantiate(classData.model, ModelOrigin);
            model.name = classData.modelObjectName;
            foreach (var child in model.GetComponentsInChildren<ChildOfInteractiveGameObject>())
            {
                child.owner = gameObject;
            }
            foreach (var highlighter in model.GetComponentsInChildren<UnitMeshHighlight>())
            {
                highlighter.unit = unit;
            }

            return model;
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
