// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
	public class Bonus : MonoBehaviour {
        #region Global Members
        [Section("Bonus")]

        [SerializeField, Enhanced, Required] public SpriteRenderer feedback = null;
        [SerializeField, Enhanced, Required] public new Collider2D collider = null;
        [SerializeField, Enhanced, Range(0f, 100000f)] public float Score = 100f;
        #endregion

        #region Collect
        public virtual void Collect(PlayerController player) {
            collider.enabled = false;

            // Play feedback here.
        }
        #endregion
    }
}
