// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50
{
	public class PlayerIK : MonoBehaviour
    {
        #region Global Members
        [Section("PlayerIK")]

        [SerializeField, Enhanced, Range(0f, 1f)] private float variable = 1f;
        #endregion
    }
}
