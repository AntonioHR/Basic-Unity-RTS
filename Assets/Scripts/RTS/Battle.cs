using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS
{
    public class Battle
    {
        public Team PlayerTeam { get; private set; }
        public Team OpposingTeam { get; private set; }

        public event Action OnBattleEnd;



        public Battle(Team playerTeam, Team opposingTeam)
        {
            this.PlayerTeam = playerTeam;
            this.OpposingTeam = opposingTeam;
        }
    }
}
