// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50
{
	public class Ingredient : MonoBehaviour
    {
        #region Global Members
        [Section("Ingredient")]

        [SerializeField, Enhanced, Range(0f, 1f)] private float height = 1.0f;
        public float Height => height; 
        #endregion
    }
}
