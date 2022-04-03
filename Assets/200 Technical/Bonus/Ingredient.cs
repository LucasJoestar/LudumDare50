// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
	public class Ingredient : Bonus {
        #region Global Members
        [Section("Ingredient")]

        [SerializeField, Enhanced, Range(0f, 1f)] public float Height = 1f;

        public override bool DestroyOnCollect => false;
        #endregion

        #region Behaviour
        public override void Collect(PlayerController player) {
            base.Collect(player);

            player.Collect(this);
        }

        public void OnCollect(int order) {
            // Collect it.
            gameObject.layer = PlayerController.PlayerMask;
            group.gameObject.layer = PlayerController.PlayerMask;
            group.sortingOrder = order;

            isCollected = true;
        }
        #endregion
    }
}
