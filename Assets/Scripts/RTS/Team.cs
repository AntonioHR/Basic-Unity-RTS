using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS
{
    public class Team: MonoBehaviour
    {
        [System.Serializable]
        public class Settings
        {
            public int StartMorale;
        }



        public Settings settings;

        public event Action<int, Team> OnMoraleChanged;

        private int morale;



        public int Morale
        {
            get
            {
                return morale;
            }
            set
            {
                var delta = morale - value;
                this.morale = value;

                if (OnMoraleChanged != null)
                    OnMoraleChanged(delta, this);
            }
        }



        void Start()
        {
            Morale = settings.StartMorale;
        }
    }
}
