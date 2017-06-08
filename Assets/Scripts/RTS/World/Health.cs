using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS.World
{
    [Serializable]
    public class Health
    {
        public int StartValue;
        public int MaxValue;
        int value;
        public Action<int> OnHealthChange;

        public int Value
        {
            get
            {
                return value;
            }

            set
            {
                var delta = value - this.value;
                this.value = value;
                if (OnHealthChange != null)
                    OnHealthChange(delta);
            }
        }
    }
}
