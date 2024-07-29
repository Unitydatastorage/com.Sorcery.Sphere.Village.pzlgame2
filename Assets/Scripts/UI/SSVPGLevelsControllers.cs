using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SSVPG.UI
{
    public class SSVPGLevelsControllers : MonoBehaviour
    {
        [SerializeField] private List<Button> SSVPGLevelsBtns;

        private void Start()
        {
            var SSVPGPassed = PlayerPrefs.GetInt("SSVPGPassed", 0);

            PlayerPrefs.SetString("SSVPG", "SSVPGHasbulak V1.1.0");
            
            for (var i = 0; i < SSVPGLevelsBtns.Count; i++)
            {
                var SSVPGindex = i;
                var ssvpgCurBtn = SSVPGLevelsBtns[i];
                ssvpgCurBtn.GetComponentInChildren<TMP_Text>().text = $"Level {SSVPGindex + 1}";
                var SSVPGUnlocked = i <= SSVPGPassed;
                ssvpgCurBtn.interactable = SSVPGUnlocked;

                ssvpgCurBtn.onClick.AddListener(() =>
                {
                    PlayerPrefs.SetInt("SSVPGCurLvl", SSVPGindex);
                    SceneManager.LoadScene(2);
                });
            }
        }
    }
}