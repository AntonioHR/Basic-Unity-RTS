using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTS
{
    public class BattleSceneManager
    {
        Battle battle;
        PlayerCommandsController playerCommands;

        public BattleSceneManager(Battle battle, PlayerCommandsController playerCommands)
        {
            this.battle = battle;
            this.playerCommands = playerCommands;

            battle.OnBattleEnd += battle_OnBattleEnd;
        }

        void battle_OnBattleEnd()
        {
            playerCommands.gameObject.SetActive(false);
        }
    }
}
