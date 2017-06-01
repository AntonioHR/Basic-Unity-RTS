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

    public interface IBuilding: ITargetable, IInteractive
    {

    }
    public interface IGround : ITargetable, IInteractive
    {

    }


    public interface IInteractive
    {
        GameObject Owner { get; }
    }

    public interface ISelectable
    {
        bool Selectable { get; }

        void Select();
        void Deselect();
    }

    public interface IHighlightable
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

    public interface ITargetable
    {
        bool Targetable { get; }

        void Target(ITargetReceiver targetReceiver);
    }

    public interface ITargetReceiver
    {
        bool CanTarget { get; }

        void SetTarget(ITargetable target, Vector3 position);
    }
}
