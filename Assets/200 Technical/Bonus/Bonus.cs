// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
	public class Bonus : MonoBehaviour {
        #region Global Members
        [Section("Bonus")]

        [SerializeField, Enhanced, Required] public BonusAttributes attributes = null;
        [SerializeField, Enhanced, Required] public SpriteRenderer feedback = null;
        [SerializeField, Enhanced, Required] public new Collider2D collider = null;
        [SerializeField, Enhanced, Range(0f, 100000f)] public float Score = 100f;

        public virtual bool DestroyOnCollect => true;
        #endregion

        #region Collect
        private Sequence sequence = null;

        // ---------------

        public virtual void Collect(PlayerController player) {
            // Play feedback here.
            PlayCollect();
        }

        protected virtual void PlayCollect() {

        }

        public void Destroy() {
            if (sequence.IsActive()) {
                sequence.Complete(false);
            }

            Destroy(gameObject);
        }
        #endregion
    }
}
