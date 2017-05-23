using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace RTS
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class Unit : MonoBehaviour
    {
        public Material idle;
        public Material selected;

        MeshRenderer meshRenderer;
        NavMeshAgent navMeshAgent;


        public bool Selectable
        {
            get
            {
                return true;
            }
        }

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = idle;
        }
        void Update()
        {

        }



        public void Deselect()
        {
            meshRenderer.material = idle;
        }
        public void Select()
        {
            meshRenderer.material = selected;
        }

        public void Target(Ground target, Vector3 position)
        {
            if (target != null)
            {
                navMeshAgent.destination = position;
            }
        }

    }
}