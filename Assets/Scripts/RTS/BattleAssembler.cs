using RTS.Inputs.Mouse;
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
        }


        public Settings settings;
        public Team playerTeam;
        public Team opposingTeam;

        PlayerCommandsController playerCommands;
        MouseBattleInput mouseInput;
        Battle battleManager;
        BattleSceneManager sceneManager;



        void Awake()
        {
            var playerCommandsObj = new GameObject("PlayerCommandsController");
            playerCommandsObj.transform.parent = this.transform;
            playerCommands = playerCommandsObj.AddComponent<PlayerCommandsController>();


            var mouseInputObj = new GameObject("MouseInputManager");
            mouseInputObj.transform.parent = this.transform;
            mouseInput = mouseInputObj.AddComponent<MouseBattleInput>();
            mouseInput.settings = settings.mouseInput;
            mouseInput.controller = playerCommands;

            battleManager = new Battle(playerTeam, opposingTeam);

            sceneManager = new BattleSceneManager(battleManager, playerCommands);
        }
    }
}