using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace RTS
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour, IUnit
    {
        [System.Serializable]
        public class Settings
        {
            public float range;
            public float attackRangeTolerance = .1f;
            public Material idleMaterial;
            public Material selectedMaterial;
        }
        public Settings settings;

        public Transform selectionIndicator;
        public MeshRenderer meshRenderer;

        NavMeshAgent navMeshAgent;
        Animator animator;

        IHittable hitTarget;




        public bool Selectable { get { return true; } }
        public bool Highlightable { get { return true; } }
        public bool CanTarget { get { return true; } }
        public bool Targetable { get { return true; } }
        public bool Hittable { get { return true; } }
        public GameObject Owner { get { return gameObject; } }

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            meshRenderer = meshRenderer != null? meshRenderer: GetComponent<MeshRenderer>();
            meshRenderer.material = settings.idleMaterial;
        }
        void Update()
        {

        }



        public void Deselect()
        {
            selectionIndicator.gameObject.SetActive(false);
        }
        public void Select()
        {
            selectionIndicator.gameObject.SetActive(true);
        }
        
        public void HighlightOn()
        {
            meshRenderer.material = settings.selectedMaterial;
        }
        public void HighlightOff()
        {
            meshRenderer.material = settings.idleMaterial; 
        }

        public void Target(ITargetReceiver targetReceiver)
        {
            throw new System.NotImplementedException();
        }

        public void SetTarget(ITargetable target, Vector3 position)
        {
            if (target != null)
            {
                navMeshAgent.stoppingDistance = 0;
                navMeshAgent.destination = position;
                var hittable = target as IHittable;
                if(hittable != null)
                {
                    
                    navMeshAgent.stoppingDistance = settings.range;
                }
            }
        }


        public void Hit(int damage)
        {
            throw new System.NotImplementedException();
        }

    }
}