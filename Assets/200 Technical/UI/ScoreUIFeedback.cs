// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
	public class ScoreUIFeedback : MonoBehaviour {
        #region Global Members
        [Section("ScoreUIFeedback")]

        [SerializeField, Enhanced, Range(0f, 1f)] private float variable = 1f;
        #endregion

        #region Behaviour
        private Sequence sequence = null;

        // ---------------

        public void Initialize(ScoreUIFeedbackAttributes attributes) {
            sequence = DOTween.Sequence(); {

                sequence.OnComplete(OnComplete);
            }

            // ----- Local Methods ----- \\

            void OnComplete() {
                UIManager.Instance.OnFeedbackComplete(this);
            }
        }

        public void Complete() {
            sequence.Kill(false);
            Destroy(gameObject);
        }
        #endregion
    }
}
