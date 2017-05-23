using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            selectionHandler.OnMultiSelect += OnMultiSelectionClicked;
            selectionHandler.OnMultiSelectHover += OnMultiSelectionClicked;

            targetingHandler = new MouseClickTargetingHandler(TargetSettings);
            targetingHandler.OnClicked += OnTargetClicked;
        }

        private void OnTargetClicked(MouseClickTargetingHandler.TargetingArguments args)
        {
            Ground ground = args.Collider == null ? null : args.Collider.GetComponent<Ground>();
            controller.TryTarget(ground, args.Position);
        }

        void OnMultiSelectionClicked(MouseClickTargetingHandler.TargetingArguments[] args)
        {
            var units = new List<Unit>();
            foreach (var item in args)
            {
                Unit unit = item.Collider == null ? null : item.Collider.GetComponent<Unit>();
                if (unit != null)
                    units.Add(unit);
            }
            controller.TrySelect(units);
        }

        void OnSelectionClicked(MouseClickTargetingHandler.TargetingArguments args)
        {
            Unit unit = args.Collider == null? null : args.Collider.GetComponent<Unit>();
            controller.TrySelect(unit);
        }
    }
}