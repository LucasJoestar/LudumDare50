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

        [SerializeField, Enhanced, Range(0f, 100000f)] public float Score = 100f;
        #endregion
    }
}
