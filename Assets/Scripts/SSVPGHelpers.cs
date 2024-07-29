using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SSVPG
{
    public static class SSVPGHelpers
    {
        public static bool SSVPGStop { get; set; }
        public static bool SSVPGIgnoreInput { get; set; }
        public static void SSVPGShake(this SSVPGToken SSVPGToken)
            => SSVPGToken.transform.DOShakeScale(0.1f, Vector3.up).SetEase(Ease.Linear);

        public static Tween SSVPGMovement(this SSVPGToken SSVPGtoken)
            => SSVPGtoken.transform.DOLocalMove(new Vector3(SSVPGtoken.SSVPGPOS.x, SSVPGtoken.SSVPGPOS.y, 0), 0.1f)
                .SetEase(Ease.Linear);

        public static string SSVPGTimeToString(this float SSVPGtime)
        {
            var SSVPGValue = Mathf.CeilToInt(SSVPGtime);
            return SSVPGValue.ToString();
        }
        
        public static void SSVPGReforce(this CanvasGroup SSVPG, bool SSVPGVal)
        {
            SSVPG.interactable = SSVPGVal;
            SSVPG.blocksRaycasts = SSVPGVal;
            SSVPG.alpha = SSVPGVal ? 1 : 0;
        }
        
        public static void SSVPGPlaySingle(this AudioSource SSVPGSoundsource, AudioClip SSVPGclip)
        {
            SSVPGSoundsource.loop = false;
            SSVPGSoundsource.Stop();
            SSVPGSoundsource.clip = SSVPGclip;
            SSVPGSoundsource.Play();
        }
        
        public static void SSVPGPitching(this AudioSource SSVPGSoundsource, AudioClip SSVPGclip, bool SSVPGval = false)
        {
            if (!SSVPGval)
            {
                SSVPGSoundsource.pitch = 1f;
                SSVPGSoundsource.PlayOneShot(SSVPGclip);
                return;
            }

            SSVPGSoundsource.pitch = Random.Range(0.9f, 1.1f);
            SSVPGSoundsource.PlayOneShot(SSVPGclip);
        }
    }

    public class SSVPGInfo
    {
        public SSVPGtile SSVPGEmpty;
        public SSVPGtile SSVPGMovableGeTile;
    }
}