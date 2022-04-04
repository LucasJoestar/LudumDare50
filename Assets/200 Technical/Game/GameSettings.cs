// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
    [Serializable]
    public class GameSteps {
        [UnityEngine.Range(0, 999)] public int MinIngredient = 0;
        [UnityEngine.Range(1f, 99f)] public float PlayerScale = 1f;
        [UnityEngine.Range(3f, 99f)] public float CameraSize = 3f;

        [Space(10f)]

        [Enhanced, MinMax(-100f, 100f)] public Vector2 HorizontalBounds = Vector2.one;
        [Enhanced, MinMax(-100f, 100f)] public Vector2 VerticalBounds = Vector2.one;

        [Space(10f)]

        [Enhanced, MinMax(0f, 100f)] public Vector2 BonusSpawnInterval = Vector2.one;
        [Enhanced, MinMax(0f, 100f)] public Vector2 PatternSpawnInterval = Vector2.one;
    }

    [CreateAssetMenu(fileName = "GameSettings", menuName = "LudumDare50/GameSettings")]
    public class GameSettings : ScriptableObject {
        #region Global Members
        [Section("Game Settings")]

        [Enhanced, Required] public InputActionMap Inputs = null;
        [Enhanced, Range(.01f, 5f)] public float Unit = 1f;
        [Enhanced, Range(.01f, 5f)] public float IntroDelay = 1f;

        [Space(10f)]

        public GameSteps[] Steps = new GameSteps[] { };
        #endregion

        #region Inputs
        [Enhanced, BeginFoldout("Inputs"), Section("Inputs")]

        public string SkipInput = "Shortcuts/Skip";
        public string PauseInput = "Shortcuts/Pause";
        public string RestartInput = "Shortcuts/Restart";
        [Enhanced, EndFoldout] public string MenuInput = "Shortcuts/Menu";
        #endregion

        #region Gameplay
        [BeginFoldout("Gameplay"), Section("Gameplay")]

        [Enhanced, Range(0f, 5f)] public float CameraSizeDelay = 0f;
        [Enhanced, Range(0f, 5f)] public float CameraSizeDuration = 1f;
        [Enhanced, EndFoldout] public Ease CameraSizeEase = Ease.OutSine;
        #endregion
    }
}
