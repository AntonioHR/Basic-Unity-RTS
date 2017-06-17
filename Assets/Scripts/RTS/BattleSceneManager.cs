using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RTS
{
    public class BattleSceneManager: MonoBehaviour
    {
        [System.Serializable]
        public class Settings
        {
            public string battleSceneName;
            public string menuSceneName;
        }

        public Settings settings;

        public Battle battle;
        public BattleUI battleUI;
        public PlayerCommandsController playerCommands;

        void Awake()
        {
            Debug.Assert(battle != null);
            Debug.Assert(battleUI != null);
            Debug.Assert(playerCommands != null);

            battle.OnBattleEnd += battle_OnBattleEnd;
        }

        void battle_OnBattleEnd(Team winner)
        {
            playerCommands.gameObject.SetActive(false);

            if (winner == battle.PlayerTeam)
                battleUI.ShowWin();
            else
                battleUI.ShowLose();
        }

        public void StartNewBattle()
        {
            SceneManager.LoadScene(settings.battleSceneName);
        }
        public void ExitBattle()
        {
            SceneManager.LoadScene(settings.menuSceneName);
        }
    }
}
