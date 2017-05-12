using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.Mouse
{
    class MouseClickTargetingHandler
    {
        //Settings Class
        [Serializable]
        public class Settings
        {
            public LayerMask TargetLayers;
            public int Button;
            public bool AllowDragging;
            public float dragStartDistance;
            //public float clickTime;
        }
        //The arguments to be passed on to events
        public class TargetingArguments
        {
            public TargetingArguments(Collider collider, Vector3 position)
            {
                Collider = collider;
                Position = position;
            }
            public Collider Collider { get; private set; }
            public Vector3 Position { get; private set; }
        }

        //Settings
        Settings settings;
        MouseInputManager manager;


        //State
        //float mouseDownTime;
        Vector3 mouseDownPosition;
        Vector3 mouseDownRealWorldPosition;
        Collider mouseDownTarget;



        //Events
        public event Action<TargetingArguments> OnClicked;
        public event Action<TargetingArguments> OnClickHold;
        public event Action<TargetingArguments> OnHover;

        public event Action<TargetingArguments[]> OnMultiSelectHover;
        public event Action<TargetingArguments[]> OnMultiSelect;

        //Constructor
        public MouseClickTargetingHandler(MouseInputManager manager, Settings settings)
        {
            this.settings = settings;
            this.manager = manager;
        }



        public void Update()
        {
            var hit = RaycastHelper.CastClickRay(settings.TargetLayers);
            var mousePos = Input.mousePosition;

            //Do we allow draggin? And Is The target the same from the click start?  And has the mouse not moved much?
            bool isDrag = settings.AllowDragging &&
                (hit.collider == null || hit.collider != mouseDownTarget) &&
                Vector2.Distance(mousePos, mouseDownPosition) > settings.dragStartDistance;

            //Click Start
            if(Input.GetMouseButtonDown(settings.Button))
            {
                //mouseDownTime = Time.time;
                mouseDownPosition = Input.mousePosition;
                mouseDownRealWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseDownTarget = hit.collider;
            } 
            //Dragging
            else if(Input.GetMouseButton(settings.Button))
            {
                if (!isDrag)
                {
                    if(OnClickHold != null)
                        OnClickHold(new TargetingArguments(hit.collider, hit.point));
                }
                else
                {
                    RaycastHit[] allHits = RaycastHelper.BoxSelect(settings.TargetLayers, mouseDownPosition);
                    if (OnMultiSelectHover != null)
                        OnMultiSelectHover(allHits.Select(h => new TargetingArguments(h.collider, h.point)).ToArray());
                }
            }
            //Released
            else if(Input.GetMouseButtonUp(settings.Button))
            {
                if (!isDrag)
                {
                    if (OnClicked != null)
                        OnClicked(new TargetingArguments(hit.collider, hit.point));
                }
                else
                {
                    RaycastHit[] allHits = RaycastHelper.BoxSelect(settings.TargetLayers, mouseDownPosition);
                    if (OnMultiSelect != null)
                        OnMultiSelect(allHits.Select(h => new TargetingArguments(h.collider, h.point)).ToArray());
                }
            } else
            //Just Hovering
            {
                if (OnHover!= null)
                    OnHover(new TargetingArguments(hit.collider, hit.point));
            }
        }
    }
}
