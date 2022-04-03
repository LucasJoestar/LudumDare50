// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
    [CreateAssetMenu(fileName = "ScoreUIFeedbackAttributes", menuName = "LudumDare50/ScoreUIFeedbackAttributes")]
	public class ScoreUIFeedbackAttributes : ScriptableObject {
        #region Global Members
        [Section("Score UI Feedback Attributes")]

        [SerializeField, Enhanced, Range(0f, 1f)] private float variable = 1f;
        #endregion
    }
}
