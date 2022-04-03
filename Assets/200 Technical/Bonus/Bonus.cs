// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using UnityEngine;
using UnityEngine.Rendering;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
	public class Bonus : MonoBehaviour {
        #region Global Members
        [Section("Bonus")]

        [SerializeField, Enhanced, Required] protected BonusAttributes attributes = null;
        [SerializeField, Enhanced, Required] protected SortingGroup group = null;
        [SerializeField, Enhanced, Required] protected new Collider2D collider = null;

        [Space(10f)]

        [SerializeField, Enhanced, Required] protected SpriteRenderer sprite = null;
        [SerializeField, Enhanced, Required] protected SpriteRenderer feedback = null;
        [SerializeField, Enhanced, Required] protected SpriteRenderer feedbackOutline = null;

        [Space(10f)]

        [SerializeField, Enhanced, Range(0f, 100000f)] public float Score = 100f;
        [SerializeField, Enhanced, ReadOnly] protected bool isCollected = false;

        public virtual bool DestroyOnCollect => true;
        #endregion

        #region Behaviour
        private void Update() {
            if (isCollected)
                return;

            group.sortingOrder = (PlayerController.Instance.transform.position.y > transform.position.y)
                               ? 1
                               : -1;
        }
        #endregion

        #region Collect
        private Sequence sequence = null;

        // ---------------

        public virtual void Collect(PlayerController player) {
            // Play feedback here.
            PlayCollect();

            isCollected = true;
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
