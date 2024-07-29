using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace SSVPG
{
    public partial class SSVPGGridCalculator
    {
        private bool SSVPGChecker()
        {
            var SSVPGHor = SSVPGChekHor(out var SSVPGHorTiles);
            var SSVPGVer = SSVPGCheckVert(out var SSVPGVerTiles);
            var SSVPGTiles = new List<SSVPGToken>();
            SSVPGTiles.AddRange(SSVPGHorTiles);
            SSVPGTiles.AddRange(SSVPGVerTiles);
            foreach (var SSVPGTile in SSVPGTiles)
                SSVPGTile.SSVPGDESTROY();

            if (SSVPGHor || SSVPGVer)
            {
                SSVPGSFXSource.SSVPGPitching(SSVPGDestroyClip, true);

                var SSVPGseq = DOTween.Sequence();
                SSVPGseq.AppendInterval(0.35f)
                    .AppendCallback(() =>
                    {
                        SSVPGScore += SSVPGTiles.Distinct().Count();
                        SSVPGTextResolve();
                        SSVPGSFXSource.SSVPGPitching(SSVPGScoreClip, true);
                    })
                    .AppendCallback(SSVPGCalculateMove);
            }
            else
            {
                SSVPGHelpers.SSVPGIgnoreInput = false;

                if (SSVPGScore >= _SSVPGGoal)
                {
                    SSVPGIsGameOver = true;

                    SSVPGuiUiForGamePlay.SSVPGWinGame(SSVPGScore, _SSVPGGoal, SSVPGTime);
                    SSVPGMainSource.SSVPGPlaySingle(SSVPGWinClip);
                }
                else if (SSVPGIsGameOver)
                {
                    SSVPGuiUiForGamePlay.SSVPGLoseGame(SSVPGScore, _SSVPGGoal, SSVPGTime);
                    SSVPGMainSource.SSVPGPlaySingle(SSVPGLoseClip);
                }
            }

            return SSVPGHor || SSVPGVer;

            bool SSVPGChekHor(out List<SSVPGToken> SSVPGmapTiles)
            {
                SSVPGmapTiles = new List<SSVPGToken>();

                for (var y = 0; y < SSVPGSizeY; y++)
                {
                    var SSVPGCur = -1;
                    var SSVPGToDelete = new List<SSVPGToken>();
                    for (var x = 0; x < SSVPGSIZEx; x++)
                    {
                        var SSVPGtile = SSVPGTilesInGrid[new Vector2Int(x, y)].ImpImpTokenItem;
                        if (SSVPGtile == null)
                            continue;

                        if (SSVPGCur == -1)
                        {
                            SSVPGCur = SSVPGtile.SSVPGID;
                            SSVPGToDelete.Add(SSVPGtile);
                            continue;
                        }

                        if (SSVPGCur == SSVPGtile.SSVPGID)
                            SSVPGToDelete.Add(SSVPGtile);
                        else
                        {
                            if (SSVPGToDelete.Count >= 3)
                                SSVPGmapTiles.AddRange(SSVPGToDelete);

                            SSVPGToDelete.Clear();
                            SSVPGCur = SSVPGtile.SSVPGID;
                            SSVPGToDelete.Add(SSVPGtile);
                        }
                    }

                    if (SSVPGToDelete.Count >= 3)
                        SSVPGmapTiles.AddRange(SSVPGToDelete);
                }

                return SSVPGmapTiles.Count > 0;
            }
        }

        private void SSVPGCalculateMove()
        {
            SSVPGHelpers.SSVPGIgnoreInput = true;

            var SSVPGHasMove = false;
            var SSVPGeptyTiles = new Dictionary<int, SSVPGInfo>();
            for (var x = 0; x < SSVPGSIZEx; x++)
            {
                for (var y = 0; y < SSVPGSizeY; y++)
                {
                    var SSVPGFtile = SSVPGTilesInGrid[new Vector2Int(x, y)];

                    if (SSVPGFtile.ImpImpTokenItem != null)
                    {
                        if (SSVPGeptyTiles.TryGetValue(x, out var SSVPGgettile))
                        {
                            SSVPGHasMove = true;
                            SSVPGgettile.SSVPGMovableGeTile = SSVPGFtile;
                            break;
                        }

                        continue;
                    }

                    if (!SSVPGeptyTiles.ContainsKey(x))
                        SSVPGeptyTiles.Add(x, new SSVPGInfo() { SSVPGEmpty = SSVPGFtile, SSVPGMovableGeTile = null });
                }
            }

            var SSVPGSeq = DOTween.Sequence();
            SSVPGSeq.AppendInterval(0f);
            foreach (var SSVPGtileData in SSVPGeptyTiles.Values)
            {
                if (SSVPGtileData.SSVPGMovableGeTile == null)
                    continue;

                var SSVPGtile = SSVPGtileData.SSVPGMovableGeTile.ImpImpTokenItem;
                SSVPGtileData.SSVPGMovableGeTile.ImpImpTokenItem = null;
                SSVPGtileData.SSVPGEmpty.ImpImpTokenItem = SSVPGtile;

                SSVPGtile.SSVPGPOS = SSVPGtileData.SSVPGEmpty.SSVPGPOSITION;

                SSVPGSeq.Join(SSVPGtile.SSVPGMovement().OnComplete(() =>
                {
                    SSVPGSFXSource.SSVPGPitching(SSVPGEndMoveClip, true);
                    SSVPGtile.SSVPGShake();
                }));
            }

            if (!SSVPGHasMove)
                SSVPGSeq.AppendInterval(0.0f)
                    .AppendCallback(SSVPGSpawnUpperTokens);
            else
                SSVPGSeq.AppendInterval(0.0f)
                    .AppendCallback(SSVPGCalculateMove);
        }

        private bool SSVPGCheckVert(out List<SSVPGToken> SSVPGTilesList)
        {
            SSVPGTilesList = new List<SSVPGToken>();
            for (var x = 0; x < SSVPGSIZEx; x++)
            {
                var SSVPGFCur = -1;
                var SSVPGToDelete = new List<SSVPGToken>();

                for (var y = 0; y < SSVPGSizeY; y++)
                {
                    var SSVPGtile = SSVPGTilesInGrid[new Vector2Int(x, y)].ImpImpTokenItem;
                    if (SSVPGtile == null)
                        continue;

                    if (SSVPGFCur == -1)
                    {
                        SSVPGFCur = SSVPGtile.SSVPGID;
                        SSVPGToDelete.Add(SSVPGtile);
                        continue;
                    }

                    if (SSVPGFCur == SSVPGtile.SSVPGID)
                        SSVPGToDelete.Add(SSVPGtile);
                    else
                    {
                        if (SSVPGToDelete.Count >= 3)
                            SSVPGTilesList.AddRange(SSVPGToDelete);

                        SSVPGToDelete.Clear();
                        SSVPGFCur = SSVPGtile.SSVPGID;
                        SSVPGToDelete.Add(SSVPGtile);
                    }
                }

                if (SSVPGToDelete.Count >= 3)
                    SSVPGTilesList.AddRange(SSVPGToDelete);
            }

            return SSVPGTilesList.Count > 0;
        }

        private void SSVPGStartMove(SSVPGtile dpa)
        {
            if (SSVPGIsGameOver)
                return;
            if (SSVPGHelpers.SSVPGStop) return;
            SSVPGTileCur = dpa;
        }

        private void Update()
        {
            if (SSVPGHelpers.SSVPGStop)
                return;

            if (SSVPGIsGameOver)
                return;

            SSVPGTime -= Time.deltaTime;

            if (SSVPGTime <= 0)
            {
                SSVPGIsGameOver = true;
                SSVPGTime = 0;
            }

            SSVPGTextResolve();

            if (SSVPGHelpers.SSVPGIgnoreInput)
                return;

            if (SSVPGScore >= _SSVPGGoal)
            {
                SSVPGuiUiForGamePlay.SSVPGWinGame(SSVPGScore, _SSVPGGoal, SSVPGTime);
                SSVPGMainSource.SSVPGPlaySingle(SSVPGWinClip);
            }
            else if (SSVPGIsGameOver)
            {
                SSVPGuiUiForGamePlay.SSVPGLoseGame(SSVPGScore, _SSVPGGoal, SSVPGTime);
                SSVPGMainSource.SSVPGPlaySingle(SSVPGLoseClip);
            }
        }
    }
}