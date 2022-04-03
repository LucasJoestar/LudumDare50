// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using EnhancedEditor;
using UnityEngine;
using UnityEngine.InputSystem;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
    [CreateAssetMenu(fileName = "GameSettings", menuName = "LudumDare50/GameSettings")]
    public class GameSettings : ScriptableObject {
        #region Global Members
        [Section("Game Settings")]

        [Enhanced, Required] public InputActionMap Inputs = null;
        [Enhanced, Range(.01f, 5f)] public float Unit = 1f;
        [Enhanced, Range(.01f, 5f)] public float IntroDelay = 1f;
        #endregion

        #region Inputs
        [Enhanced, BeginFoldout("Inputs"), Section("Inputs")]

        public string SkipInput = "Shortcuts/Skip";
        public string PauseInput = "Shortcuts/Pause";
        public string RestartInput = "Shortcuts/Restart";
        [Enhanced, EndFoldout] public string MenuInput = "Shortcuts/Menu";
        #endregion
    }
}
