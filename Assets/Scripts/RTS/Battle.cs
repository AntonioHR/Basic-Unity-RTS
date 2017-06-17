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

        public event Action<Team> OnBattleEnd;



        public Battle(Team playerTeam, Team opposingTeam)
        {
            this.PlayerTeam = playerTeam;
            this.PlayerTeam.OnMoraleChanged += (change, team) => { if (team.Morale <= 0) OnPlayerLose(); };
            this.OpposingTeam = opposingTeam;
            this.OpposingTeam.OnMoraleChanged += (change, team) => { if (team.Morale <= 0) OnPlayerWin(); };
        }

        private void OnPlayerWin()
        {
            OnBattleEnd(PlayerTeam);
        }
        private void OnPlayerLose()
        {
            OnBattleEnd(OpposingTeam);
        }
    }
}
