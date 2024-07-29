using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace SSVPG
{
    public class SSVPGToken : MonoBehaviour
    {
        [SerializeField] private List<Sprite> SSVPGIcons;
        public Vector2Int SSVPGPOS { get; set; }
        private bool SSVPGIsDestroy;
        private int SSVPGidItem;

        public int SSVPGID
        {
            get => SSVPGidItem;
            set
            {
                SSVPGidItem = value;
                GetComponent<SpriteRenderer>().sprite = SSVPGIcons[SSVPGidItem];
            }
        }

        public void SSVPGDESTROY()
        {
            if (SSVPGIsDestroy)
                return;
            SSVPGIsDestroy = true;
            SSVPGImmDestroy();
        }

        private void SSVPGImmDestroy()
        {
            var SSVPGseq = DOTween.Sequence();
            SSVPGseq.Append(transform.DOScale(1.2f, 0.1f).SetEase(Ease.InOutBounce))
                .Append(transform.DOScale(0f, 0.15f).SetEase(Ease.InOutBounce))
                .OnComplete(() => { Destroy(gameObject); });
        }
    }
}