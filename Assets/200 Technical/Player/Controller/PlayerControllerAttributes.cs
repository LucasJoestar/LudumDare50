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

        #region Ingredient
        [Enhanced, BeginFoldout("Ingredient"), Section("Ingredient")]

        public LayerMask IngredienMask = new LayerMask();
        public LayerMask PlayerMask = new LayerMask();

        [Space(10f)]

        [EndFoldout, Range(0f, 5f)] public float CollectDuration = .5f;
        #endregion
    }
}
