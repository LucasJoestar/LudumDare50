// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
	public class UIManager : Singleton<UIManager> {
        #region Global Members
        [Section("ScoreUIManager")]

        [SerializeField, Enhanced, Required] private UIManagerAttributes attributes = null;
        [SerializeField, Enhanced, Required] private ScoreUIFeedbackAttributes feedbackAttributes = null;

        [Space(10f)]

        [SerializeField, Enhanced, Required] private ScoreUIFeedback feedback = null;
        [SerializeField, Enhanced, Required] private UIButton[] buttons = null;

        [Space(10f)]

        [SerializeField, Enhanced, Required] private CanvasGroup fadeToBlack = null;
        [SerializeField, Enhanced, Required] private CanvasGroup menu = null;
        [SerializeField, Enhanced, Required] private CanvasGroup title = null;
        [SerializeField, Enhanced, Required] private RectTransform titleTransform = null;
        #endregion

        #region Score
        private List<ScoreUIFeedback> allFeedbacks = new List<ScoreUIFeedback>();

        // ---------------

        public void IncreaseScore(PlayerController player, float increase) {
            
        }

        private void SpawnFeedback(PlayerController player, float increase) {

        }

        public void OnFeedbackComplete(ScoreUIFeedback feedback) {
            allFeedbacks.Remove(feedback);
        }
        #endregion

        #region Management
        private Sequence sequence = null;

        // ---------------

        public void ShowMenu(bool doForceFade) {
            if (sequence.IsActive()) {
                sequence.Kill(false);
            }

            sequence = DOTween.Sequence();

            // Fade to black.
            Sequence _fadeToBlack = DOTween.Sequence(); {
                if (doForceFade) {
                    _fadeToBlack.Join(fadeToBlack.DOFade(1f, 0f));
                } else {
                    _fadeToBlack.AppendInterval(attributes.MenuFadeInBlackDelay);
                    _fadeToBlack.Append(fadeToBlack.DOFade(1f, attributes.MenuFadeInBlackDuration).SetEase(attributes.MenuFadeInBlackEase));
                }

                sequence.Join(_fadeToBlack);
            }

            sequence.AppendCallback(OnFaded);

            // Fade In.
            Sequence _fadeIn = DOTween.Sequence(); {
                _fadeIn.AppendInterval(attributes.MenuFadeOutBlackDelay);
                _fadeIn.Join(fadeToBlack.DOFade(0f, attributes.MenuFadeOutBlackDuration).SetEase(attributes.MenuFadeOutBlackEase));
                _fadeIn.Join(menu.DOFade(1f, attributes.MenuFadeInDuration).SetEase(attributes.MenuFadeInEase).SetDelay(attributes.MenuFadeInDelay));

                sequence.Append(_fadeIn);
            }

            // Title.
            Sequence _title = DOTween.Sequence(); {
                float rotate = 360f * attributes.TitleRotateInLoop;

                _title.AppendInterval(attributes.TitleInDelay);

                _title.Append(title.DOFade(1f, attributes.TitleFadeInDuration).SetEase(attributes.TitleFadeInEase));
                _title.Join(titleTransform.DOScale(1f, attributes.TitleScaleInDuration).SetEase(attributes.TitleScaleInEase));
                _title.Join(titleTransform.DORotate(new Vector3(0f, 0f, rotate), attributes.TitleRotateInDuration, RotateMode.FastBeyond360).SetEase(attributes.TitleRotateInEase));

                sequence.Join(_title);
            }

            sequence.OnComplete(OnComplete);

            // ----- Local Methods ----- \\

            void OnFaded() {
                menu.alpha = 0f;
                title.alpha = 0f;

                titleTransform.localScale = attributes.TitleScaleInSize;
                titleTransform.rotation = Quaternion.identity;
                titleTransform.anchoredPosition = new Vector2(titleTransform.anchoredPosition.x, attributes.TitleMoveIdleFrom);

                buttons[0].Select();

                GameManager.Instance.ResetGame();
                EnableButtons(true);
            }

            void OnComplete() {
                // Idle sequence.
                sequence = DOTween.Sequence(); {
                    titleTransform.rotation = Quaternion.identity;

                    sequence.Join(titleTransform.DOScale(attributes.TitleScaleIdle, attributes.TitleScaleIdleDuration).SetEase(attributes.TitleScaleIdleCurve));
                    sequence.Join(titleTransform.DOAnchorPosY(attributes.TitleMoveIdle, attributes.TitleMoveIdleDuration).SetEase(attributes.TitleMoveIdleCurve));

                    sequence.SetLoops(-1, LoopType.Restart);
                }
            }
        }

        public void HideMenu() {
            if (sequence.IsActive()) {
                sequence.Kill(false);
            }

            // Fade.
            Sequence _fadeOut = DOTween.Sequence(); {
                _fadeOut.AppendInterval(attributes.MenuFadeOutDelay);
                _fadeOut.Append(menu.DOFade(0f, attributes.MenuFadeOutDuration).SetEase(attributes.MenuFadeOutEase));

                sequence.Append(_fadeOut);
            }

            // Title.
            Sequence _title = DOTween.Sequence(); {
                float rotate = titleTransform.eulerAngles.z + (360f * attributes.TitleRotateOutLoop);

                _title.AppendInterval(attributes.TitleOutDelay);

                _title.Append(title.DOFade(0f, attributes.TitleFadeOutDuration).SetEase(attributes.TitleFadeOutEase));
                _title.Join(titleTransform.DOScale(attributes.TitleScaleOut, attributes.TitleScaleOutDuration).SetEase(attributes.TitleScaleOutEase));
                _title.Join(titleTransform.DORotate(new Vector3(0f, 0f, rotate), attributes.TitleRotateOutDuration, RotateMode.FastBeyond360).SetEase(attributes.TitleRotateOutEase));

                sequence.Join(_title);
            }
        }

        public void EnableButtons(bool enabled) {
            foreach (var button in buttons) {
                button.interactable = enabled;
            }
        }

        // ---------------

        public void PauseGame(bool isPaused) {
        }

        public void RestartGame() {
        }
        #endregion

        #region Reset
        public void ResetBehaviour() {
            foreach (var _feedback in allFeedbacks) {
                _feedback.Complete();
            }

            allFeedbacks.Clear();
        }
        #endregion
    }
}
