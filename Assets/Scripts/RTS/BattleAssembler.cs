using RTS.Inputs.Mouse;
using RTS.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTS
{
    public class BattleAssembler : MonoBehaviour
    {
        [System.Serializable]
        public class Settings
        {
            public MouseBattleInput.Settings mouseInput;
            public BattleSceneManager.Settings sceneManager;
        }


        public Settings settings;
        public Team playerTeam;
        public Team opposingTeam;
        public BattleUI battleUI;

        PlayerCommandsController playerCommands;
        MouseBattleInput mouseInput;
        Battle battleManager;
        BattleSceneManager sceneManager;



        void Awake()
        {
            var playerCommandsObj = new GameObject("PlayerCommandsController");
            playerCommandsObj.transform.parent = this.transform;
            playerCommands = playerCommandsObj.AddComponent<PlayerCommandsController>();

            battleManager = new Battle(playerTeam, opposingTeam);

            var mouseInputObj = new GameObject("MouseInputManager");
            mouseInputObj.transform.parent = playerCommands.transform;
            mouseInput = mouseInputObj.AddComponent<MouseBattleInput>();
            mouseInput.settings = settings.mouseInput;
            mouseInput.controller = playerCommands;

            sceneManager = InstancingUtils.CreateWithPreemptiveExecution<BattleSceneManager>((manager) =>
            {
                manager.battle = battleManager;
                manager.battleUI = battleUI;
                manager.playerCommands = playerCommands;
                manager.settings = settings.sceneManager;
            });
            sceneManager.transform.parent = transform;
        }
    }
}