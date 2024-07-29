using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace SSVPG.UI
{
    public class SSVPGSettings : MonoBehaviour
    {
        [SerializeField] private AudioSource SSVPGSFXSource;
        [SerializeField] private Slider SSVPGMusicSlider;
        [SerializeField] private Button SSVPGCloseSettings;
        [SerializeField] private AudioSource SSVPGBackgroundMusic;
        [SerializeField] private Slider SSVPGSFXSlider;
        [SerializeField] private AudioClip SSVPGCLICKClip;
        [SerializeField] private CanvasGroup SSVPGContent;
        [SerializeField] private Button SSVPGOpenSettings;
        public void SSVPGCLICK() => SSVPGSFXSource.SSVPGPitching(SSVPGCLICKClip);

        private void Start()
        {
            Application.targetFrameRate = 120;

            SSVPGHide();
            SSVPGOpenSettings.onClick.AddListener(SSVPGShow);
            SSVPGCloseSettings.onClick.AddListener(SSVPGHide);

            var ssvpgSfx = PlayerPrefs.GetFloat("SSVPGSFX", 1f);
            SSVPGSFXSource.volume = ssvpgSfx;
            SSVPGSFXSlider.value = ssvpgSfx;
            SSVPGSFXSlider.onValueChanged.AddListener(SSVPGChangeSfx);

            var ssvpgBg = PlayerPrefs.GetFloat("SSVPGBG", 1f);
            SSVPGBackgroundMusic.volume = ssvpgBg;
            SSVPGMusicSlider.value = ssvpgBg;
            SSVPGMusicSlider.onValueChanged.AddListener(SSVPGChangeBgMsc);

            return;
            
            void SSVPGHide()
            {
                DOTween.timeScale = 1f;
                SSVPGHelpers.SSVPGStop = false;
                SSVPGContent.SSVPGReforce(false);
            }

            void SSVPGChangeBgMsc(float ssvpgDef)
            {
                PlayerPrefs.SetFloat("SSVPGBG", ssvpgDef);
                SSVPGBackgroundMusic.volume = ssvpgDef;
            }
            
            void SSVPGShow()
            {
                SSVPGSFXSource.Stop();
                DOTween.timeScale = 0f;
                SSVPGHelpers.SSVPGStop = true;
                SSVPGContent.SSVPGReforce(true);
            }
            
            void SSVPGChangeSfx(float ssvpgDef)
            {
                PlayerPrefs.SetFloat("SSVPGSFX", ssvpgDef);
                SSVPGSFXSource.volume = ssvpgDef;
            }
        }
    }
}