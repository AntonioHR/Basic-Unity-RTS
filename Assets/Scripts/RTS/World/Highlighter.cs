using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS.World
{
    public class Highlighter : MonoBehaviour, IHighlightable
    {
        public Material idle;
        public Material highlight;

        public MeshRenderer meshRenderer;
        public event System.Action OnDestroyed;

        public bool Highlightable { get { return this.isActiveAndEnabled; } }



        void Start()
        {
            meshRenderer = meshRenderer != null ? meshRenderer : GetComponentInChildren<MeshRenderer>();
            meshRenderer.material = idle;
        }
        void OnDestroy()
        {
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