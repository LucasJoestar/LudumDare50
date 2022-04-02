// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50
{
    [CreateAssetMenu(fileName = "PlayerIKAttributes", menuName = "LudumDare50/PlayerIKAttributes")]
	public class PlayerIKAttributes : ScriptableObject
    {
        #region Global Members
        [Section("PlayerIKAttributes")]

        [SerializeField, Enhanced, Range(0f, 1f)] private float variable = 1f;
        #endregion
    }
}
