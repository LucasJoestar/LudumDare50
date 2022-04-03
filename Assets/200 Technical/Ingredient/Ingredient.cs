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

        [SerializeField, Enhanced, Required] public SpriteRenderer Sprite = null;
        [SerializeField, Enhanced, Range(0f, 1f)] public float Height = 1.0f;
        #endregion
    }
}
