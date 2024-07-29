using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SSVPG.UI
{
    public class SSVPGStartHUD : MonoBehaviour
    {
        [SerializeField] private Button SSVPGOpenLevel;
        [SerializeField] private CanvasGroup SSVPGPolicyPanel;
        [SerializeField] private Button SSVPGCloseApplication;
        [SerializeField] private Button SSVPGCloseWelcome;
        [SerializeField] private CanvasGroup SSVPGMenuPanel;
        [SerializeField] private CanvasGroup SSVPGPolicyPanel1;
        [SerializeField] private Button SSVPGPolicyPanel1Btn;
        [SerializeField] private Button SSVPGPolicyPanel1BtnBack;
        [SerializeField] private CanvasGroup SSVPGLevelPanel;
        [SerializeField] private CanvasGroup SSVPGWelcomePanel;
        [SerializeField] private Button SSVPGPolicyBTN;
        [SerializeField] private List<Button> SSVPGCloseAll;

        private void Awake()
        {
            foreach (var SSVPGClocseBtn in this.SSVPGCloseAll)
                SSVPGClocseBtn.onClick.AddListener(SSVPGCloseAll);

            SSVPGPolicyBTN.onClick.AddListener(() => SSVPGPolicyPanel.SSVPGReforce(true));
            SSVPGOpenLevel.onClick.AddListener(() => SSVPGLevelPanel.SSVPGReforce(true));

            SSVPGCloseApplication.onClick.AddListener(Application.Quit);

            SSVPGCloseAll();

            if (!PlayerPrefs.HasKey("SSVPG"))
            {
                SSVPGWelcomePanel.SSVPGReforce(true);
                SSVPGMenuPanel.SSVPGReforce(false);
            }

            SSVPGPolicyPanel1Btn.onClick.AddListener(() => SSVPGPolicyPanel1.SSVPGReforce(true));
            SSVPGPolicyPanel1BtnBack.onClick.AddListener(() =>
            {
                SSVPGPolicyPanel1.SSVPGReforce(false);
                SSVPGWelcomePanel.SSVPGReforce(true);
            });
            SSVPGCloseWelcome.onClick.AddListener(() =>
            {
                SSVPGCloseAll();
                PlayerPrefs.SetString("SSVPG", "SSVPGAccess");
            });

            return;

            void SSVPGCloseAll()
            {
                SSVPGMenuPanel.SSVPGReforce(true);
                SSVPGWelcomePanel.SSVPGReforce(false);
                SSVPGPolicyPanel.SSVPGReforce(false);
                SSVPGLevelPanel.SSVPGReforce(false);
                SSVPGPolicyPanel1.SSVPGReforce(false);
            }
        }
    }
}