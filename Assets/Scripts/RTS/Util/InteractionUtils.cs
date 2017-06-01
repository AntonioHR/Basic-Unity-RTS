using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.Util
{
    public static class InteractionExtensions
    {
        public static T GetComponentInOwner<T>(this Collider me)
        {
            if (me == null)
                return default(T);
            var inter = me.GetComponent<IInteractive>();
            return inter.GetComponentInOwner<T>();
        }
        public static T GetComponentInOwner<T>(this IInteractive me)
        {
            if (me == null)
                return default(T);
            return me.Owner.GetComponent<T>();
        }
    }
}
