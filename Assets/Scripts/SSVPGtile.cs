using System;
using UnityEngine;

namespace SSVPG
{
    public class SSVPGtile : MonoBehaviour
    {
        public Vector2Int SSVPGPOSITION { get; set; }

        public SSVPGToken ImpImpTokenItem { get; set; }
        

        #region SSVPG_Events

        private void OnMouseUp()
        {
            SSVPGONTileUP?.Invoke();
        }

        private void OnMouseEnter()
        {
            SSVPGONTileEnter?.Invoke(this);
        }
        
        private void OnMouseDown()
        {
            if (SSVPGHelpers.SSVPGIgnoreInput)
                return;
            
            SSVPGONTileDown?.Invoke(this);
        }

        public event Action<SSVPGtile> SSVPGONTileDown;
        public event Action<SSVPGtile> SSVPGONTileEnter;
        public event Action SSVPGONTileUP;
        
        #endregion
    }
}