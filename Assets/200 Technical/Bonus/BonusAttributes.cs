// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
    [CreateAssetMenu(fileName = "BonusAttributes", menuName = "LudumDare50/BonusAttributes")]
	public class BonusAttributes : ScriptableObject {
        #region Global Members
        [Section("BonusAttributes")]

        [SerializeField, Enhanced, Range(0f, 1f)] private float variable = 1f;
        #endregion
    }
}
