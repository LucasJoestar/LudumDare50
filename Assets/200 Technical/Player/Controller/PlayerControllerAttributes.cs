// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50
{
    [CreateAssetMenu(fileName = "PlayerControllerAttributes", menuName = "LudumDare50/PlayerControllerAttributes")]
	public class PlayerControllerAttributes : ScriptableObject
    {
        #region Global Members
        [Section("PlayerControllerAttributes")]

        [SerializeField, Enhanced, Range(0f, 1f)] private float variable = 1f;
        #endregion
    }
}
