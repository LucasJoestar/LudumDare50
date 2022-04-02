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

        #region Inputs
        [Enhanced, BeginFoldout("Inputs"), Section("Inputs")]

        [EndFoldout] public string MoveInput = "Player/Move";
        #endregion

        #region Movement
        [Enhanced, BeginFoldout("Movement"), Section("Movement")]

        [Range(0f, 1f)] public float MovementDelay = 0f;
        [Enhanced, Range(0f, 5f)] public float MovementDuration = 0f;

        [Space(10f)]

        [Enhanced, Range(0f, 5f)] public float MovementRootHeight = 1f;
        [Enhanced, Range(0f, 5f)] public float MovementUnit = 1f;

        [Space(10f)]

        public AnimationCurve MovementRootCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [Enhanced, EndFoldout] public Ease MovementEase = Ease.Linear;
        #endregion

        #region Collision
        [Enhanced, BeginFoldout("Collision"), Section("Collision")]

        [EndFoldout, Range(0f, 1f)] public float OverlapRadius = .2f;
        #endregion
    }
}
