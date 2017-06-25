using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.World
{
    class Highlighter : MonoBehaviour, IHighlightable
    {
        public Material idle;
        public Material highlight;

        public MeshRenderer meshRenderer;
        public event System.Action OnDestroyed;

        public bool Highlightable { get { return this.isActiveAndEnabled; } }

        public bool Destroyed
        {
            get;
            private set;
        }




        void Start()
        {
            meshRenderer = meshRenderer != null ? meshRenderer : GetComponentInChildren<MeshRenderer>();
            meshRenderer.material = idle;
        }
        void OnDestroy()
        {
            Destroyed = true;
            if (OnDestroyed != null)
                OnDestroyed();
        }


        public void HighlightOn()
        {
            if(meshRenderer != null)
            meshRenderer.material = highlight;
        }
        public void HighlightOff()
        {
            if (meshRenderer != null)
            meshRenderer.material = idle;
        }

    }
}