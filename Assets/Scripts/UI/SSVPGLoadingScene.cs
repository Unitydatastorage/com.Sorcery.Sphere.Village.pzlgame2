using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SSVPG.UI
{
    public class SSVPGLoadingScene : MonoBehaviour
    {
        private void Start()
        {
            var ssvpgSeq = DOTween.Sequence();
            ssvpgSeq.AppendInterval(Random.Range(1f,3f))
                .OnComplete(() => SceneManager.LoadScene(1));
        }
    }
}