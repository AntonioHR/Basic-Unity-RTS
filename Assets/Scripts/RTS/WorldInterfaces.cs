using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS
{
    public interface ISelectionUnit : IDestructionNotifier
    {
        World.Groups.SelectionGroup Group { get; }
        bool Selectable { get; }
        GameObject Owner { get; }
    }


    public interface IHighlightable : IDestructionNotifier
    {
        bool Highlightable { get; }

        void HighlightOn();
        void HighlightOff();
    }

    public interface IHittable: ITargetable
    {
        bool Hittable { get; }

        void Hit(int damage);

    }

    public interface ITargetable: IDestructionNotifier
    {
        bool Targetable { get; }

        Vector3 position { get; }

        void TargetBy(ITargetReceiver targetReceiver);
    }

    public interface IInteractive
    {
        GameObject Owner { get; }
    }

    public interface ITargetReceiver
    {
        bool CanTarget { get; }

        void SetTarget(ITargetable target, Vector3 position);
    }

    public interface IDestructionNotifier
    {
        event Action OnDestroyed;
    }

    public interface IHealth
    {
        float Health { get; }

        float MaxHealth { get; }

        event Action<float> OnHealthChanged;
    }
}
