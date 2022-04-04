// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using EnhancedEditor;
using UnityEngine;
using DG.Tweening;
using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50
{
    [CreateAssetMenu(fileName = "Pattern", menuName = "LudumDare50/Pattern")]
	public class Pattern : ScriptableObject
    {
        #region Global Members
        [Section("Pattern")]
        public Sprite Sprite = null;
        public Vector2 ColliderOffset = Vector2.zero;
        public Vector2 ColliderSize = Vector2.one;
        [Section("Movement")]
        public float Speed = 1.0f;
        public Ease Acceleration = Ease.Linear;
        public bool IsTriggerContinuous = false;
        public float StartingDelay = 2.0f;
        public PatternType PatternType = PatternType.Linear;
        [Section("End")]
        public float EndDuration = 1.0f;
        public float EndForce = 1.0f;
        public int EndVibrato = 10;
        public float RestingDuration = 2.0f;
        #endregion

        #region Method

        #endregion
    }

    public enum PatternType
    {
        Linear, 
        Circular, 
        Slap
    }
}
