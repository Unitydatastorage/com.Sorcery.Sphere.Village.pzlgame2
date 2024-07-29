using System.Collections.Generic;
using DG.Tweening;
using SSVPG.UI;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SSVPG
{
    public partial class SSVPGGridCalculator : MonoBehaviour
    {
        private bool SSVPGIsGameOver;
        private int SSVPGScore;
        private int SSVPGLevelIndex;
        private int _SSVPGGoal;
        [SerializeField] private AudioClip SSVPGWinClip;
        [SerializeField] private int SSVPGSizeY;
        [SerializeField] private AudioClip SSVPGDestroyClip;
        [SerializeField] private AudioClip SSVPGLoseClip;
        [SerializeField] private SSVPGtile SSVPGTilesPrefab;
        [SerializeField] private AudioClip SSVPGScoreClip;
        [SerializeField] private AudioSource SSVPGSFXSource;
        [SerializeField] private AudioSource SSVPGMainSource;
        [SerializeField] private SSVPGUIGameplay SSVPGuiUiForGamePlay;
        [SerializeField] private AudioClip SSVPGEndMoveClip;
        [SerializeField] private AudioClip SSVPGBadMoveClip;
        [SerializeField] private List<SSVPGToken> SSVPGTokensPrefab;
        [SerializeField] private TMP_Text SSVPGScoreTxt;
        [SerializeField] private int SSVPGSIZEx;
        [SerializeField] private TMP_Text SSVPGLevelIndexText;
        
        private Dictionary<Vector2Int, SSVPGtile> SSVPGTilesInGrid = new Dictionary<Vector2Int, SSVPGtile>();
        private SSVPGtile SSVPGTileCur;
        private float SSVPGTime = 90f;
        private List<SSVPGToken> SSVPGTokensInSession = new List<SSVPGToken>();

        private void Start()
        {
            SSVPGLevelIndex = PlayerPrefs.GetInt("SSVPGCurLvl", 0) + 1;
            SSVPGLevelIndexText.text = $"Level {SSVPGLevelIndex}";
            Random.InitState(SSVPGLevelIndex);
            
            _SSVPGGoal = Random.Range(150, 301);
            _SSVPGGoal = Mathf.RoundToInt(_SSVPGGoal / 10f) * 10;
            SSVPGTextResolve();

            SSVPGInitialField();
            SSVPGSpawnUpperTokens();

            return;

            void SSVPGInitialField()
            {
                var SSVPGpos = transform.position;

                for (var SSVPGX = 0; SSVPGX < SSVPGSIZEx; SSVPGX++)
                {
                    for (var SSVPGY = 0; SSVPGY < SSVPGSizeY; SSVPGY++)
                    {
                        var SSVPGtile = Instantiate(SSVPGTilesPrefab, transform);
                        SSVPGtile.transform.position = new Vector3(SSVPGpos.x + SSVPGX, SSVPGpos.y + SSVPGY, 0);
                        SSVPGtile.SSVPGPOSITION = new Vector2Int(SSVPGX, SSVPGY);
                        SSVPGTilesInGrid.Add(SSVPGtile.SSVPGPOSITION, SSVPGtile);
                        SSVPGtile.SSVPGONTileDown += SSVPGStartMove;
                        SSVPGtile.SSVPGONTileEnter += SSVPGEnter;
                        SSVPGtile.SSVPGONTileUP += SSVPGCancelMove;
                    }
                }

                while (SSVPGTokensInSession.Count < 4)
                {
                    var SSVPGRand = SSVPGTokensPrefab[Random.Range(0, SSVPGTokensPrefab.Count)];
                    if (SSVPGTokensInSession.Contains(SSVPGRand))
                        continue;

                    SSVPGTokensInSession.Add(SSVPGRand);
                }

                return;

                void SSVPGEnter(SSVPGtile SSVPGtiLE)
                {
                    if (SSVPGIsGameOver)
                        return;

                    if (SSVPGHelpers.SSVPGStop) return;
                    var SSVPGCurTile = SSVPGTileCur;
                    if (SSVPGCurTile == null)
                        return;
                    if (SSVPGCurTile == SSVPGtiLE)
                        return;
                    if (SSVPGHelpers.SSVPGIgnoreInput)
                    {
                        SSVPGTileCur = null;
                        return;
                    }

                    var SSVPGDir = ((Vector2)(SSVPGCurTile.SSVPGPOSITION - SSVPGtiLE.SSVPGPOSITION)).normalized;
                    if (Mathf.Abs(SSVPGDir.x) != 1f && Mathf.Abs(SSVPGDir.y) != 1f)
                        return;
                    var SSVPGseq = DOTween.Sequence();
                    (SSVPGCurTile.ImpImpTokenItem.SSVPGPOS, SSVPGtiLE.ImpImpTokenItem.SSVPGPOS) =
                        (SSVPGtiLE.ImpImpTokenItem.SSVPGPOS, SSVPGCurTile.ImpImpTokenItem.SSVPGPOS);
                    (SSVPGCurTile.ImpImpTokenItem, SSVPGtiLE.ImpImpTokenItem) =
                        (SSVPGtiLE.ImpImpTokenItem, SSVPGCurTile.ImpImpTokenItem);
                    var SSVPGCheck = SSVPGChecker();
                    SSVPGseq.Append(SSVPGCurTile.ImpImpTokenItem.SSVPGMovement())
                        .Join(SSVPGtiLE.ImpImpTokenItem.SSVPGMovement());
                    if (SSVPGCheck)
                        SSVPGseq.AppendInterval(0.2f);
                    else
                    {
                        SSVPGseq.AppendInterval(0.1f)
                            .AppendCallback(() =>
                            {
                                SSVPGSFXSource.SSVPGPitching(SSVPGBadMoveClip, true);
                                (SSVPGCurTile.ImpImpTokenItem.SSVPGPOS, SSVPGtiLE.ImpImpTokenItem.SSVPGPOS) =
                                    (SSVPGtiLE.ImpImpTokenItem.SSVPGPOS, SSVPGCurTile.ImpImpTokenItem.SSVPGPOS);
                                (SSVPGCurTile.ImpImpTokenItem, SSVPGtiLE.ImpImpTokenItem) =
                                    (SSVPGtiLE.ImpImpTokenItem, SSVPGCurTile.ImpImpTokenItem);
                                SSVPGCurTile.ImpImpTokenItem.SSVPGMovement();
                                SSVPGtiLE.ImpImpTokenItem.SSVPGMovement();
                            });
                    }

                    SSVPGTileCur = null;
                }

                void SSVPGCancelMove() => SSVPGTileCur = null;
            }
        }
        
        private void SSVPGSpawnUpperTokens()
        {
            SSVPGHelpers.SSVPGIgnoreInput = true;
            var SSVPGHasSpawn = false;
            for (var x = 0; x < SSVPGSIZEx; x++)
            {
                var SSVPGtileFather = SSVPGTilesInGrid[new Vector2Int(x, SSVPGSizeY - 1)];
                if (SSVPGtileFather.ImpImpTokenItem != null)
                    continue;

                SSVPGHasSpawn = true;
                var SSVPGrand = Random.Range(0, SSVPGTokensInSession.Count);
                var SSVPGtile = Instantiate(SSVPGTokensInSession[SSVPGrand], transform);
                var SSVPGindex = SSVPGTokensPrefab.FindIndex(x => x == SSVPGTokensInSession[SSVPGrand]);
                SSVPGtile.SSVPGID = SSVPGindex;
                SSVPGtile.transform.localPosition = SSVPGtileFather.transform.localPosition;
                SSVPGtile.SSVPGPOS = SSVPGtileFather.SSVPGPOSITION;
                SSVPGtileFather.ImpImpTokenItem = SSVPGtile;

                SSVPGtile.SSVPGShake();
            }

            if (!SSVPGHasSpawn)
            {
                SSVPGChecker();
                return;
            }

            SSVPGCalculateMove();
        }

        private void SSVPGTextResolve()
        {
            SSVPGScoreTxt.text = $"Score: {SSVPGScore}/{_SSVPGGoal}\nTime: {SSVPGTime.SSVPGTimeToString()}s";
        }
    }
}