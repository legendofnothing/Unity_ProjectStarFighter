using System.Collections.Generic;
using SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu {
    public class LevelSelector : MonoBehaviour {
        public List<MissionBrief> MissionBriefs = new();
        public TextMeshProUGUI missionName;
        public TextMeshProUGUI missionBrief;
        public Button playButton;
        
        private int _currentIndex;

        private void Start() {
            missionName.text = "SELECT A MISSION";
            playButton.interactable = false;
        }

        public void SelectMission(int index) {
            if (!playButton.interactable) playButton.interactable = true;
            missionName.text = MissionBriefs[index].name;
            missionBrief.text = MissionBriefs[index].text;
        }
    }
}