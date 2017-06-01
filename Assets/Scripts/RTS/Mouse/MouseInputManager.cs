using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS.Util;

namespace RTS.Mouse
{
    public class MouseInputManager : MonoBehaviour
    {
        public PlayerCommandsController controller;
        [Space()]
        [SerializeField]
        MouseClickTargetingHandler.Settings SelectionSettings;
        [SerializeField]
        MouseClickTargetingHandler.Settings TargetSettings;

        MouseClickTargetingHandler selectionHandler;
        MouseClickTargetingHandler targetingHandler;


        //Funções chamadas pela Unity
        void Update()
        {
            selectionHandler.Update();
            targetingHandler.Update();
        }
        void Start()
        {
            selectionHandler = new MouseClickTargetingHandler(SelectionSettings);
            selectionHandler.OnClicked += OnSelectionClicked;
            selectionHandler.OnHover += OnSelectionHover;
            selectionHandler.OnMultiSelect += OnMultiSelectionClicked;
            selectionHandler.OnMultiSelectHover += OnMultiSelectionHover;

            targetingHandler = new MouseClickTargetingHandler(TargetSettings);
            targetingHandler.OnClicked += OnTargetClicked;
        }

        private void OnTargetClicked(MouseClickTargetingHandler.TargetingArguments args)
        {
            ITargetable target = args.Collider.GetComponentInOwner<ITargetable>();
            controller.TryTarget(target, args.Position);
        }

        void OnMultiSelectionClicked(MouseClickTargetingHandler.TargetingArguments[] args)
        {
            var targets = new List<ISelectable>();
            foreach (var item in args)
            {
                var target = item.Collider.GetComponentInOwner<ISelectable>();
                if (target != null)
                    targets.Add(target);
            }
            controller.TrySelect(targets);
        }
        void OnMultiSelectionHover(MouseClickTargetingHandler.TargetingArguments[] args)
        {
            var targets = new List<IHighlightable>();
            foreach (var item in args)
            {
                var target = item.Collider.GetComponentInOwner<IHighlightable>();
                if (target != null)
                    targets.Add(target);
            }
            controller.TryHighlight(targets);
        }

        void OnSelectionClicked(MouseClickTargetingHandler.TargetingArguments args)
        {
            var target = args.Collider.GetComponentInOwner<ISelectable>();
            controller.TrySelect(target);
        }

        void OnSelectionHover(MouseClickTargetingHandler.TargetingArguments args)
        {
            var target = args.Collider.GetComponentInOwner<IHighlightable>();
            controller.TryHighlight(target);
        }
    }
}