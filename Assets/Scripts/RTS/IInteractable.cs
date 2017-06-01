using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS
{
    public interface IUnit : ISelectable, IHighlightable, IHittable, ITargetReceiver, IInteractive
    {
    }

    public interface IBuilding: ITargetable, IInteractive, IHittable
    {

    }
    public interface IGround : ITargetable, IInteractive
    {

    }

    public interface ISelectable : IDeathNotifier
    {
        bool Selectable { get; }

        void Select();
        void Deselect();
    }

    public interface IHighlightable : IDeathNotifier
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

    public interface ITargetable: IDeathNotifier
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

    public interface IDeathNotifier
    {
        event Action OnDestroyed;
    }
}
