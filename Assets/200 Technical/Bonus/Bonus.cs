// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
	public class Bonus : MonoBehaviour {
        #region Global Members
        [Section("Bonus")]

        [SerializeField, Enhanced, Required] protected BonusAttributes attributes = null;
        [SerializeField, Enhanced, Required] protected SortingGroup group = null;
        [SerializeField, Enhanced, Required] protected ParticleSystem puff = null;
        [SerializeField, Enhanced, Required] protected new Collider2D collider = null;

        [Space(10f)]

        [SerializeField, Enhanced, Required] protected SpriteRenderer sprite = null;
        [SerializeField, Enhanced, Required] protected Transform feedbackRoot = null;
        [SerializeField, Enhanced, Required] protected SpriteRenderer feedback = null;
        [SerializeField, Enhanced, Required] protected SpriteRenderer feedbackOutline = null;

        [Space(10f)]

        [SerializeField, Enhanced, Range(0f, 100000f)] public float Score = 100f;
        [SerializeField] protected bool isCollected = false;

        public virtual bool DestroyOnCollect => true;
        #endregion

        #region Behaviour
        private void Start() {
            if (!isCollected) {
                PlaySpawn();
            }
        }

        private void Update() {
            if (isCollected)
                return;

            group.sortingOrder = (PlayerController.Instance.transform.position.y > transform.position.y)
                               ? 1
                               : -1;
        }
        #endregion

        #region Collect
        protected Sequence collectSequence = null;
        protected List<Tween> spriteSequence = new List<Tween>();
        protected List<Tween> colorSequence = new List<Tween>();
        protected List<Tween> feedbackSequence = new List<Tween>();

        // ---------------

        public virtual void Collect(PlayerController player) {
            // Play feedback here.
            PlayCollect();
            UIManager.Instance.IncreaseScore((int)Score);

            isCollected = true;
        }

        public virtual void OnCollect(int order) {
            // Stop sequences.
            CompleteSequences();

            colorSequence.Clear();
            spriteSequence.Clear();
            feedbackSequence.Clear();

            feedbackRoot.gameObject.SetActive(false);

            // Collect it.
            gameObject.layer = PlayerController.PlayerMask;
            collider.gameObject.layer = PlayerController.PlayerMask;
            group.sortingOrder = order;

            isCollected = true;
        }

        // ---------------

        protected void PlaySpawn() {
            puff.transform.SetParent(null);
            puff.Play();

            // Spawn.
            {
                transform.localScale = attributes.SpawnScale;
                transform.DOScale(1f, attributes.SpawnScaleDuration).SetEase(attributes.SpawnScaleEase).SetDelay(attributes.SpawnScaleDelay);
            }

            // Sprite.
            {
                spriteSequence.Add(sprite.transform.DOScale(attributes.IdleScale, attributes.IdleScaleDuration).SetEase(attributes.IdleScaleCurve).SetLoops(-1, LoopType.Restart));
                spriteSequence.Add(sprite.transform.DOLocalMoveY(attributes.IdleMovement, attributes.IdleMovementDuration).SetEase(attributes.IdleMovementCurve)
                              .SetLoops(-1, LoopType.Restart));
            }
            
            // Feedback.
            {
                feedbackSequence.Add(feedbackRoot.DOLocalMoveY(attributes.FeedbackMovement, attributes.FeedbackMovementDuration).SetEase(attributes.FeedbackMovementCurve)
                                .SetLoops(-1, LoopType.Restart));

                feedbackSequence.Add(feedbackRoot.DOLocalRotate(new Vector3(0f, 0f, attributes.FeedbackRotation), attributes.FeedbackRotationDuration, RotateMode.FastBeyond360)
                                .SetEase(attributes.FeedbackRotationCurve).SetLoops(-1, LoopType.Restart));

                feedbackSequence.Add(feedback.transform.DOScale(attributes.FeedbackScale, attributes.FeedbackScaleDuration).SetEase(attributes.FeedbackScaleCurve)
                                .SetLoops(-1, LoopType.Restart));

                feedbackSequence.Add(feedbackOutline.transform.DOScale(attributes.FeedbackOutlineScale, attributes.FeedbackOutlineScaleDuration).SetEase(attributes.FeedbackOutlineScaleCurve)
                                .SetLoops(-1, LoopType.Restart));
            }

            // Color.
            {
                colorSequence.Add(feedback.DOGradientColor(attributes.FeedbackGradient, attributes.FeedbackGradientDuration).SetEase(attributes.FeedbackGradientEase)
                             .SetLoops(-1, LoopType.Restart));

                colorSequence.Add(feedbackOutline.DOGradientColor(attributes.FeedbackOutlineGradient, attributes.FeedbackOutlineGradientDuration)
                             .SetEase(attributes.FeedbackOutlineGradientEase).SetLoops(-1, LoopType.Restart));
            }
        }

        protected virtual void PlayCollect() {
            // Stop sequences.
            foreach (var _tween in spriteSequence) {
                if (_tween.IsActive()) {
                    _tween.Kill(false);
                }
            }

            spriteSequence.Clear();

            foreach (var _tween in colorSequence) {
                if (_tween.IsActive()) {
                    _tween.Kill(false);
                }
            }

            colorSequence.Clear();

            // Do collect.
            collectSequence = DOTween.Sequence(); {
                // Sprite.
                collectSequence.Join(sprite.transform.DOLocalMoveY(attributes.CollectMovement, attributes.CollectMovementDuration).SetEase(attributes.CollectMovementCurve)
                               .SetDelay(attributes.CollectMovementDelay));

                collectSequence.Join(sprite.transform.DOScale(attributes.CollectScale, attributes.CollectScaleDuration).SetEase(attributes.CollectScaleCurve)
                               .SetDelay(attributes.CollectScaleDelay));

                collectSequence.Join(sprite.DOFade(attributes.CollectFade, attributes.CollectFadeDuration).SetEase(attributes.CollectFadeEase)
                               .SetDelay(attributes.CollectFadeDelay));

                // Feedback.
                collectSequence.Join(feedbackRoot.DOScale(attributes.CollectFeedbackScale, attributes.CollectFeedbackScaleDuration).SetEase(attributes.CollectFeedbackScaleEase)
                               .SetDelay(attributes.CollectFeedbackScaleDelay));

                collectSequence.Join(feedback.DOFade(0f, attributes.CollectFeedbackFadeDuration).SetEase(attributes.CollectFeedbackFadeEase)
                               .SetDelay(attributes.CollectFeedbackFadeDelay));

                collectSequence.Join(feedbackOutline.DOFade(0f, attributes.CollectFeedbackFadeDuration).SetEase(attributes.CollectFeedbackFadeEase)
                               .SetDelay(attributes.CollectFeedbackFadeDelay));
            }
        }

        // ---------------

        public void Destroy() {
            CompleteSequences();
            Destroy(gameObject);
        }

        private void CompleteSequences() {
            if (collectSequence.IsActive()) {
                collectSequence.Kill(true);
            }

            foreach (var _tween in spriteSequence) {
                if (_tween.IsActive()) {
                    _tween.Kill(true);
                }
            }

            foreach (var _tween in colorSequence) {
                if (_tween.IsActive()) {
                    _tween.Kill(true);
                }
            }

            foreach (var _tween in feedbackSequence) {
                if (_tween.IsActive()) {
                    _tween.Kill(true);
                }
            }
        }
        #endregion
    }
}
