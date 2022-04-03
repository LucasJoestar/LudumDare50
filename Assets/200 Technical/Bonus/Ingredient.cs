// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
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

            float duration = collectSequence.Duration();
            player.Collect(this, duration);
        }
        #endregion
    }
}
