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

        MouseClickTargetingHandler selectionHandler;

        //Funções chamadas pela Unity
        void Update()
        {
            selectionHandler.Update();
        }
        void Start()
        {
            selectionHandler = new MouseClickTargetingHandler(this, SelectionSettings);
            selectionHandler.OnClicked += OnSelectionClicked;
            selectionHandler.OnMultiSelect += OnMultiSelectionClicked;
            selectionHandler.OnMultiSelectHover += OnMultiSelectionClicked;
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