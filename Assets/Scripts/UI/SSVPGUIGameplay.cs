using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SSVPG.UI
{
    public class SSVPGUIGameplay : MonoBehaviour
    {
                
        [SerializeField] private TMP_Text SSVPGWinText;
        [SerializeField] private TMP_Text SSVPGLoseLevelText;
        [SerializeField] private Button SSVPGNextLvlBtn;
        [SerializeField] private List<Button> SSVPGHomeBTN;
        [SerializeField] private Button SSVPGRetryLvlBtn;
        [SerializeField] private TMP_Text SSVPGLoseText;
        [SerializeField] private TMP_Text SSVPGWinLevelText;
        [SerializeField] private CanvasGroup SSVPGHudPanel;
        [SerializeField] private CanvasGroup SSVPGWinPanel;
        [SerializeField] private CanvasGroup SSVPGLosePanel;
        public void SSVPGWinGame(int SSVPGScore, int SSVPGGoal, float SSVPGTime)
        {
            SSVPGWinPanel.SSVPGReforce(true);
            SSVPGWinText.text = $"Score: {SSVPGScore}/{SSVPGGoal}\nTime: {SSVPGTime.SSVPGTimeToString()}s";
            
            var SSVPGCurLevel = PlayerPrefs.GetInt("SSVPGCurLvl", 0);
            SSVPGWinLevelText.text = $"Level {SSVPGCurLevel + 1}";

            SSVPGCurLevel = Mathf.Clamp(SSVPGCurLevel, 0, 99);
            
            var SSVPGPassed = PlayerPrefs.GetInt("SSVPGPassed", 0);

            if (SSVPGCurLevel == SSVPGPassed)
            {
                SSVPGPassed++;
                SSVPGPassed = Mathf.Clamp(SSVPGPassed, 0, 99);

                PlayerPrefs.SetInt("SSVPGPassed", SSVPGPassed);
            }
        }
        
        private void Start()
        {
            foreach (var SSVPG in SSVPGHomeBTN)
                SSVPG.onClick.AddListener(() => SceneManager.LoadScene(1));

            SSVPGRetryLvlBtn.onClick.AddListener(()=> SceneManager.LoadScene(2));
            
            SSVPGNextLvlBtn.onClick.AddListener(() =>
            {
                var SSVPGcur = PlayerPrefs.GetInt("SSVPGCurLvl", 0);

                SSVPGcur++;
                SSVPGcur = Mathf.Clamp(SSVPGcur, 0, 99);
                PlayerPrefs.SetInt("SSVPGCurLvl", SSVPGcur);

                SceneManager.LoadScene(2);
            });
            
            SSVPGShowHud();
        }

        private void SSVPGShowHud()
        {
            SSVPGHudPanel.SSVPGReforce(true);
            SSVPGWinPanel.SSVPGReforce(false);
            SSVPGLosePanel.SSVPGReforce(false);
        }
        
        public void SSVPGLoseGame(int SSVPGScore, int SSVPGGoal, float SSVPGTime)
        {
            SSVPGLosePanel.SSVPGReforce(true);
            SSVPGLoseText.text = $"Score: {SSVPGScore}/{SSVPGGoal}\nTime: {SSVPGTime.SSVPGTimeToString()}s";
            
            var SSVPGCurLevel = PlayerPrefs.GetInt("SSVPGCurLvl", 0);
            
            SSVPGLoseLevelText.text = $"Level {SSVPGCurLevel + 1}";
        }
    }
}