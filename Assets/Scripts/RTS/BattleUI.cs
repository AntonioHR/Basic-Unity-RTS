using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTS
{
    public class BattleUI : MonoBehaviour
    {
        [SerializeField]
        private Transform winIndicator;
        [SerializeField]
        private Transform loseIndicator;
        [SerializeField]
        private Transform battleEndUI;
        [SerializeField]
        private Button newBattleButton;
        [SerializeField]
        private Button exitButton;
        [SerializeField]
        private Button joinButton;

        public void Start()
        {
            battleEndUI.gameObject.SetActive(false);
            winIndicator.gameObject.SetActive(false);
            loseIndicator.gameObject.SetActive(false);
            var battleSceneManager = FindObjectOfType<BattleSceneManager>();
            newBattleButton.onClick.AddListener(() => battleSceneManager.StartNewBattle());
            exitButton.onClick.AddListener(() => battleSceneManager.ExitBattle());
            var playerCommandsController = FindObjectOfType<PlayerCommandsController>();
            joinButton.onClick.AddListener(() => playerCommandsController.TryMergeSelection());
        }

        public void ShowWin()
        {
            ShowEnd();
            winIndicator.gameObject.SetActive(true);
        }

        public void ShowLose()
        {
            ShowEnd();
            loseIndicator.gameObject.SetActive(true);
        }

        private void ShowEnd()
        {
            battleEndUI.gameObject.SetActive(true);
        }
    }
}