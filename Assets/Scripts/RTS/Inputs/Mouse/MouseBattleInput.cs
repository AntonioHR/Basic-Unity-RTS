using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS.Util;
using RTS.World.Squads;

namespace RTS.Inputs.Mouse
{
    public class MouseBattleInput : MonoBehaviour
    {
        [System.Serializable]
        public class Settings
        {
            
            public MouseTargeter.Settings SelectionSettings;
            
            public MouseTargeter.Settings TargetSettings;
        }

        public PlayerCommandsController controller;
        [Space()]

        public Settings settings;

        MouseTargeter selectionHandler;
        MouseTargeter targetingHandler;


        //Funções chamadas pela Unity
        void Update()
        {
            selectionHandler.Update();
            targetingHandler.Update();
        }
        void Start()
        {
            selectionHandler = new MouseTargeter(settings.SelectionSettings);
            selectionHandler.OnClicked += OnSelectionClicked;
            selectionHandler.OnHover += OnSelectionHover;
            selectionHandler.OnMultiSelect += OnMultiSelectionClicked;
            selectionHandler.OnMultiSelectHover += OnMultiSelectionHover;

            targetingHandler = new MouseTargeter(settings.TargetSettings);
            targetingHandler.OnClicked += OnTargetClicked;
        }

        private void OnTargetClicked(MouseTargeter.TargetingArguments args)
        {
            ITargetable target = args.Collider.GetComponentInOwner<ITargetable>();
            controller.TryTarget(target, args.Position);
        }

        void OnMultiSelectionClicked(MouseTargeter.TargetingArguments[] args)
        {
            var targets = new List<ISelectionUnit>();
            foreach (var item in args)
            {
                var target = item.Collider.GetComponentInOwner<ISelectionUnit>();
                if (target != null)
                    targets.Add(target);
            }
            controller.TrySelect(targets);
        }
        void OnMultiSelectionHover(MouseTargeter.TargetingArguments[] args)
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

        void OnSelectionClicked(MouseTargeter.TargetingArguments args)
        {
            var target = args.Collider.GetComponentInOwner<ISelectionUnit>();
            controller.TrySelect(target);
        }

        void OnSelectionHover(MouseTargeter.TargetingArguments args)
        {
            var target = args.Collider.GetComponentInOwner<IHighlightable>();
            controller.TryHighlight(target);
        }
    }
}