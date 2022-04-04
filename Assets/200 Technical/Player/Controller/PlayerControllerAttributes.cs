// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
    [CreateAssetMenu(fileName = "PlayerControllerAttributes", menuName = "LudumDare50/PlayerControllerAttributes")]
    public class PlayerControllerAttributes : ScriptableObject {
        [Section("Player Controller Attributes")]

        #region Collision
        [Enhanced, BeginFoldout("Collision"), Section("Collision")]

        public LayerMask LayerMask = new LayerMask();

        [Enhanced, EndFoldout, Range(0f, 1f)] public float OverlapRadius = .2f;
        #endregion

        #region Inputs
        [Enhanced, BeginFoldout("Inputs"), Section("Inputs")]

        [EndFoldout] public string MoveInput = "Player/Move";
        #endregion

        #region Idle
        [Enhanced, BeginFoldout("Idle"), Section("Idle")]

        [Range(0f, 10f)] public float IdleDelay = 0f;
        [Enhanced, Range(0f, 10f)] public float IdleDuration = 1f;
        public Vector3 IdleScale = new Vector3(1f, 1f, 1f);

        [Enhanced, EndFoldout] public AnimationCurve IdleCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);
        #endregion

        #region Scale
        [Enhanced, BeginFoldout("Scale"), Section("Scale")]

        [Range(0f, 1f)] public float ScaleDelay = 0f;
        [Enhanced, Range(0f, 10f)] public float ScaleDuration = 1f;

        [Enhanced, EndFoldout] public AnimationCurve ScaleCurve = AnimationCurve.Linear(0f, 0f, 1f, 0f);
        #endregion

        #region Movement
        [Enhanced, BeginFoldout("Movement"), Section("Movement")]

        [Range(0f, 1f)] public float MovementDelay = 0f;
        [Enhanced, Range(0f, 5f)] public float MovementDuration = 1f;
        [Enhanced, Range(0f, 5f)] public float MovementLandingDuration = 1f;

        [Space(10f)]

        [Enhanced, Range(0f, 5f)] public float MovementRootHeight = 1f;
        [Enhanced, Range(0f, 5f)] public float MovementUnit = 1f;

        [Space(10f)]

        public AnimationCurve MovementRootCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [Enhanced, EndFoldout] public Ease MovementEase = Ease.Linear;
        #endregion

        #region Instability
        [BeginFoldout("Instability"), Section("Instability")]

        [Enhanced, Range(0f, .1f)] public float InstabilityIngredientCoef = .01f;
        [Enhanced, Range(0f, 1f)] public float InstabilityMovementMax = .1f;
        [Enhanced, Range(2f, 10f)] public float InstabilityMinIngredient = 5f;

        [Space(10f)]

        [Enhanced, Range(0f, 5f)] public float InstabilityDecrease = 1f;
        [Enhanced, EndFoldout] public Ease InstabilityDecreaseEase = Ease.Linear;
        #endregion

        #region Ingredient
        [Enhanced, BeginFoldout("Ingredient"), Section("Ingredient")]

        [EndFoldout, Range(0f, 5f)] public float CollectDuration = .5f;
        #endregion

        #region Game Over
        [BeginFoldout("Game Over"), Section("Game Over")]

        [Enhanced, Range(0f, 5f)] public float SplashDuration = 1f;
        [EndFoldout, Enhanced, Range(0f, 5f)] public float EatDuration = 1f;
        #endregion

        #region Sounds
        [BeginFoldout("Sounds"), Section("Sounds")]
        [Enhanced, Range(0f, 1f)] public float VolumeScale = 1f;
        public AudioClip[] jumpingClips = new AudioClip[] { };
        public AudioClip[] landingClips = new AudioClip[] { };
        [EndFoldout] public AudioClip[] bonusClip = new AudioClip[] { };

        #endregion
    }
}
