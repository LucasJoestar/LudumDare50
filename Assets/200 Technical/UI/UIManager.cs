// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LudumDare50 {
	public class UIManager : Singleton<UIManager> {
        #region Global Members
        [Section("ScoreUIManager")]

        [SerializeField, Enhanced, Required] private UIManagerAttributes attributes = null;
        [SerializeField, Enhanced, Required] private ScoreUIFeedbackAttributes feedbackAttributes = null;

        [Space(10f)]

        [SerializeField, Enhanced, Required] private Canvas worldCanvas = null;
        [SerializeField, Enhanced, Required] private ScoreUIFeedback feedback = null;
        [SerializeField, Enhanced, Required] private UIButton[] buttons = null;

		[Space(10f)]

		[SerializeField, Enhanced, Required] private UIButton[] menuButtons = null;
		[SerializeField, Enhanced, Required] private UIButton[] creditsButtons = null;
		[SerializeField, Enhanced, Required] private UIButton[] pauseButtons = null;

		[Space(10f)]

        [SerializeField, Enhanced, Required] private CanvasGroup fadeToBlack = null;
		[SerializeField, Enhanced, Required] private RectTransform titleTransform = null;
		[SerializeField, Enhanced, Required] private CanvasGroup menu = null;
        [SerializeField, Enhanced, Required] private CanvasGroup title = null;
        [SerializeField, Enhanced, Required] private CanvasGroup credits = null;
        [SerializeField, Enhanced, Required] private CanvasGroup ingame = null;
        [SerializeField, Enhanced, Required] private CanvasGroup pause = null;
        [SerializeField, Enhanced, Required] private TextMeshProUGUI scoreText = null;
        [SerializeField, Enhanced, Required] private TextMeshProUGUI highscoreText = null;
		[SerializeField, Enhanced, Required] private RectTransform menuBackground = null;
		[SerializeField, Enhanced, Required] private RectTransform creditsAnchor = null;
		[SerializeField, Enhanced, Required] private Image scoreFlash = null;
		[SerializeField, Enhanced, Required] private Image gauge = null;

		[Space(10f)]
		[SerializeField, Enhanced, Required] private CanvasGroup gameOver = null;
		[SerializeField, Enhanced, Required] private UIButton[] gameOverButton = null;
		[SerializeField, Enhanced, Required] private TMPro.TMP_Text gameOverScoreText = null; 

		private DOTweenTMPAnimator animator = null;
        #endregion

        #region Behaviour
        private void Start() {
			animator = new DOTweenTMPAnimator(scoreText);
			DoScroll();
		}

		void DoScroll() {
			menuBackground.anchoredPosition = Vector2.zero;
			menuBackground.DOAnchorPos(attributes.MenuScroll, attributes.MenuScrollDuration).SetEase(attributes.MenuscrollEase).OnComplete(DoScroll);
		}
        #endregion

        #region Score
        private List<ScoreUIFeedback> allFeedbacks = new List<ScoreUIFeedback>();

		private Sequence increaseSequence = null;
		private Sequence resizeSequence = null;
		private Sequence flashSequence = null;
		private Sequence completeSequence = null;

		private int nextScore = 0;
		private float currentScore = 0f;
		private float scoreIncrease = 0f;

		private bool isWaitingForNextUpdate = true;

		// ---------------

		public void IncreaseScore(int increase) {
            // Spawn feedback.
            var _feedback = Instantiate(feedback, worldCanvas.transform);
            _feedback.Initialize(feedbackAttributes, increase);

			// Score increase.
			UpdateScore(nextScore + increase);
		}

        public void OnFeedbackComplete(ScoreUIFeedback feedback) {
            allFeedbacks.Remove(feedback);
        }

		// -----------------------

		public void UpdateScore(int score) {
			if (score == currentScore)
				return;

			// If already increase, wait for it to complete.
			if (increaseSequence.IsActive()) {
				isWaitingForNextUpdate = true;

				return;
			}

			// Increase.
			int scoreCurrent = Mathf.FloorToInt(nextScore);
			float scoreIncrement = score - scoreCurrent;

			if (scoreIncrement < 1)
				return;

			float amount = Mathf.Min(scoreIncrement, attributes.ScoreIncreaseMaxAmount);

			scoreIncrease = scoreIncrement / amount;
			nextScore = score;

			// Highscore.
			float highscore = PlayerPrefs.GetInt(GameManager.HIGHSCORE_KEY, 0);
			if (nextScore > highscore) {
				PlayerPrefs.SetInt(GameManager.HIGHSCORE_KEY, nextScore);
			}

			DoIncrease();

			// Bounce.
			if (!resizeSequence.IsActive()) {
				resizeSequence = DOTween.Sequence();

				int count = animator.textInfo.characterCount;

				for (int i = 0; i < count; i++) {
					if (!animator.textInfo.characterInfo[i].isVisible)
						continue;

					resizeSequence.Join(animator.DOScaleChar(i, attributes.ScoreScaleSize, attributes.ScoreScaleDuration).SetEase(attributes.ScoreScaleCurve));
				}

				// Flash.
				if (!flashSequence.IsActive()) {
					flashSequence = DOTween.Sequence(); {
						flashSequence.Join(scoreFlash.DOFade(1f, attributes.ScoreScaleFlashDuration).SetEase(attributes.ScoreScaleFlashCurve).SetDelay(attributes.ScoreScaleFlashDelay));
					}

					resizeSequence.Join(flashSequence);
				}
			}
		}

		// -----------------------

		private void DoIncrease() {
			// Increase.
			increaseSequence = DOTween.Sequence(); {
				string currentString = currentScore.ToString("0");
				string newString = (currentScore + scoreIncrease).ToString("0");

				// Get the total amount of changed digits.
				int digits = 0;
				int nextDigits = 0;

				if (newString.Length > currentString.Length) {
					digits = currentString.Length;
					nextDigits = newString.Length;
				} else {
					for (int i = 0; i < newString.Length; i++) {
						if (newString[i] != currentString[i]) {
							digits = newString.Length - i;
							nextDigits = digits;

							break;
						}
					}
				}

				float increaseDuration = attributes.ScoreIncreaseDuration * .5f;

				for (int i = digits; i-- > 0;) {
					if (!animator.textInfo.characterInfo[i].isVisible)
						continue;

					Sequence _sequence = DOTween.Sequence(); {
						_sequence.Join(animator.DORotateChar(i, new Vector3(45f, 0f, 0f), increaseDuration).SetEase(Ease.OutCirc));
						_sequence.Join(animator.DOOffsetChar(i, new Vector3(0f, attributes.ScoreOffset.y, 0f), increaseDuration).SetEase(attributes.ScoreOffsetInEase));

						increaseSequence.Join(_sequence);
					}
				}

				increaseSequence.PrependInterval(attributes.ScoreIncreaseInterval);
				increaseSequence.OnComplete(() => OnIncreaseUpdate(nextDigits));
			}

			// Complete.
			if (Mathf.RoundToInt(currentScore + scoreIncrease) == nextScore) {
				if (completeSequence.IsActive()) {
					completeSequence.Complete(false);
				}

				completeSequence = DOTween.Sequence();

				// Shake.
				Sequence shakeSequence = DOTween.Sequence(); {
					shakeSequence.Join(scoreText.rectTransform.DOShakeAnchorPos(attributes.ScoreShakeDuration, attributes.ScoreShakeStrength, (int)attributes.ScoreShakeVibrato, attributes.ScoreShakeRandomness, true).SetEase(attributes.ScoreShakeEase));

					shakeSequence.PrependInterval(attributes.ScoreShakeDelay);
					completeSequence.Join(shakeSequence);
				}

				// Flash.
				if (flashSequence.IsActive()) {
					flashSequence.Complete(false);
				}

				flashSequence = DOTween.Sequence(); {
					flashSequence.Join(scoreFlash.DOFade(1f, attributes.ScoreFlashDuration).SetEase(attributes.ScoreFlashCurve).SetDelay(attributes.ScoreFlashDelay));
					completeSequence.Join(flashSequence);
				}
			}
		}

		private void OnIncreaseUpdate(int digits) {
			increaseSequence.Complete(false);

			currentScore += scoreIncrease;
			scoreText.text = currentScore.ToString("### ### 000");

			increaseSequence = DOTween.Sequence(); {
				float increaseDuration = attributes.ScoreIncreaseDuration * .5f;

				for (int i = digits; i-- > 0;) {
					if (!animator.textInfo.characterInfo[i].isVisible)
						continue;

					Sequence _sequence = DOTween.Sequence(); {
						_sequence.Join(animator.DORotateChar(i, new Vector3(-45f, 0f, 0f), 0f));
						_sequence.Join(animator.DOOffsetChar(i, new Vector3(0f, attributes.ScoreOffset.x, 0f), 0f));

						_sequence.Join(animator.DORotateChar(i, new Vector3(0f, 0f, 0f), increaseDuration).SetEase(Ease.InCirc));
						_sequence.Join(animator.DOOffsetChar(i, new Vector3(0f, 0f, 0f), increaseDuration).SetEase(attributes.ScoreOffsetOutEase));

						increaseSequence.Join(_sequence);
					}
				}

				increaseSequence.OnComplete(OnIncreaseComplete);
			}
		}

		private void OnIncreaseComplete() {
			if (isWaitingForNextUpdate) {
				increaseSequence.Kill(false);
				isWaitingForNextUpdate = false;

				UpdateScore(nextScore);
				return;
			}

			if (Mathf.RoundToInt(currentScore) != nextScore) {
				DoIncrease();
			} else {
				currentScore = nextScore;
			}
		}
		#endregion

		#region Management
		private Sequence sequence = null;
		private Sequence fadeSequence = null;
		private Sequence creditsSequence = null;

        // ---------------

        public void ShowMenu(bool doForceFade) {
            if (sequence.IsActive()) {
                sequence.Kill(true);
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
                menu.alpha = 1f;
                title.alpha = 0f;
				ingame.alpha = 0f;
				pause.alpha = 0;
				gameOver.alpha = 0f;

				int highscore = PlayerPrefs.GetInt(GameManager.HIGHSCORE_KEY, 0);
				highscoreText.text = highscore.ToString("### ### 000").Trim();

				titleTransform.localScale = attributes.TitleScaleInSize;
                titleTransform.rotation = Quaternion.identity;
                titleTransform.anchoredPosition = new Vector2(titleTransform.anchoredPosition.x, attributes.TitleMoveIdleFrom);

                GameManager.Instance.ResetGame();
                EnableButtons(true);

				foreach (var button in menuButtons) {
					button.Selectable.enabled = true;
				}

				menuButtons[0].Selectable.Select();
			}

			void OnComplete() {
				// Idle sequence.
				sequence.Kill();

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
                sequence.Kill(true);
            }

            sequence = DOTween.Sequence();

            // Fade.
            Sequence _fadeOut = DOTween.Sequence(); {
                _fadeOut.AppendInterval(attributes.MenuFadeOutDelay);
                _fadeOut.Append(fadeToBlack.DOFade(1f, attributes.MenuFadeOutDuration).SetEase(attributes.MenuFadeOutEase));
				_fadeOut.AppendInterval(.2f);
                _fadeOut.Append(fadeToBlack.DOFade(0f, 0f));
				_fadeOut.Append(menu.DOFade(0f, 0f));

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

			foreach (var button in menuButtons) {
				button.Selectable.enabled = false;
			}
		}

		public void StartPlay() {
			ingame.alpha = 0f;

			if (fadeSequence.IsActive()) {
				fadeSequence.Complete();
			}

			fadeSequence = DOTween.Sequence(); {
				fadeSequence.Join(ingame.DOFade(1f, attributes.PlayFadeDuration).SetEase(attributes.PlayFadeEase).SetDelay(attributes.PlayFadeDelay));
			}
		}

        public void EnableButtons(bool enabled) {
            foreach (var button in buttons) {
                button.Selectable.interactable = enabled;
            }
        }

		// ---------------

		public void ShowCredits(bool isVisible) {
			if (creditsSequence.IsActive()) {
				creditsSequence.Kill();
			}

			foreach (var button in menuButtons) {
				button.Selectable.enabled = !isVisible;
			}

			foreach (var button in creditsButtons) {
				button.Selectable.enabled = isVisible;
			}

			creditsSequence = DOTween.Sequence();

			if (isVisible) {
				creditsSequence.Join(credits.DOFade(1f, attributes.CreditsFadeInDuration).SetEase(attributes.CreditsFadeInEase));

				creditsSequence.Join(creditsAnchor.DOAnchorMin(attributes.CreditsMoveInMinAnchor, attributes.CreditsMoveInDuration).SetEase(attributes.CreditsMoveInEase));
				creditsSequence.Join(creditsAnchor.DOAnchorMax(attributes.CreditsMoveInMaxAnchor, attributes.CreditsMoveInDuration).SetEase(attributes.CreditsMoveInEase));
			} else {
				creditsSequence.Join(credits.DOFade(0f, attributes.CreditsFadeOutDuration).SetEase(attributes.CreditsFadeOutEase));

				creditsSequence.Join(creditsAnchor.DOAnchorMin(attributes.CreditsMoveOutMinAnchor, attributes.CreditsMoveOutDuration).SetEase(attributes.CreditsMoveOutEase));
				creditsSequence.Join(creditsAnchor.DOAnchorMax(attributes.CreditsMoveOutMaxAnchor, attributes.CreditsMoveOutDuration).SetEase(attributes.CreditsMoveOutEase));
			}

			creditsSequence.InsertCallback(.1f, OnComplete);

			void OnComplete() {
				if (isVisible) {
					creditsButtons[0].Selectable.Select();
				} else {
					menuButtons[0].Selectable.Select();
				}
            }
		}

		public void PauseGame(bool isPaused, bool doForce) {
			if (fadeSequence.IsActive()) {
				fadeSequence.Complete();
			}

            foreach (var button in pauseButtons) {
				button.Selectable.enabled = isPaused;
            }

			if (doForce)
				return;

			fadeSequence = DOTween.Sequence();

			if (isPaused) {
				fadeSequence.Join(pause.DOFade(1f, attributes.PauseFadeInDuration).SetEase(attributes.PauseFadeInEase).SetDelay(attributes.PauseFadeInDelay));
            } else {
				fadeSequence.Join(pause.DOFade(0f, attributes.PauseFadeOutDuration).SetEase(attributes.PauseFadeOutEase).SetDelay(attributes.PauseFadeOutDelay));
			}

			fadeSequence.OnComplete(OnComplete);
			fadeSequence.SetUpdate(true);

			void OnComplete() {
				if (isPaused) {
					pauseButtons[0].Selectable.Select();
				}
			}
		}

        public void RestartGame() {
			if (fadeSequence.IsActive()) {
				fadeSequence.Complete();
			}

			fadeSequence = DOTween.Sequence();

			fadeSequence.Join(fadeToBlack.DOFade(1f, attributes.RestartFadeInDuration).SetEase(attributes.RestartFadeInEase).SetDelay(attributes.RestartFadeInDelay));
			fadeSequence.AppendInterval(attributes.RestartFadeOutDelay);

			fadeSequence.OnComplete(OnComplete);

			void OnComplete() {
				if (sequence.IsActive()) {
					sequence.Kill(true);
				}

				sequence = DOTween.Sequence();

				sequence.Join(fadeToBlack.DOFade(0f, attributes.RestartFadeOutDuration).SetEase(attributes.RestartFadeOutEase));

				GameManager.Instance.ResetGame();
				GameManager.Instance.StartPlay();
			}
		}

		public void OnGameOver(){

			if (fadeSequence.IsActive()){
				fadeSequence.Complete();
            }
			fadeSequence = DOTween.Sequence();

			fadeSequence.Join(fadeToBlack.DOFade(1f, attributes.CreditsFadeOutDuration).SetEase(attributes.GameOverFadeOutEase).SetDelay(attributes.GameOverFadeOutDelay));
			fadeSequence.AppendInterval(attributes.GameOverFadeOutDelay);

			fadeSequence.AppendCallback(OnFaded);

			fadeSequence.OnComplete(OnComplete);

			// ----- Local Methods ----- \\

			void OnFaded()
			{
				menu.alpha = 0f;
				title.alpha = 0f;
				ingame.alpha = 0f;
				pause.alpha = 0;
				gameOver.alpha = 1f;

				gameOverScoreText.text = currentScore.ToString("### ### 000").Trim();
				foreach (var button in gameOverButton)
				{
					button.Selectable.enabled = true;
				}

				gameOverButton[0].Selectable.Select();
			}
			void OnComplete()
			{
				if (sequence.IsActive())
				{
					sequence.Kill(true);
				}

				sequence = DOTween.Sequence();

				sequence.Join(fadeToBlack.DOFade(0f, attributes.GameOverFadeInDuration).SetEase(attributes.GameOverFadeInEase));

			}
		}
		#endregion

		#region Gameplay
		private Sequence gaugeSequence = null;

		// ---------------

		public void UpdateGauge(float percent) {
			if (gaugeSequence.IsActive()) {
				gaugeSequence.Kill();
			}

			gaugeSequence = DOTween.Sequence(); {
				gaugeSequence.AppendInterval(attributes.GaugeIncreaseDelay);
				gaugeSequence.Append(gauge.DOFillAmount(percent, attributes.GaugeIncreaseDuration).SetEase(attributes.GaugeIncreaseEase));

				if (percent == 1f) {
					gaugeSequence.OnComplete(GameManager.Instance.GoNextStep);
				}
			}
		}

		public void ResetGauge() {
			if (gaugeSequence.IsActive()) {
				gaugeSequence.Kill();
			}

			gaugeSequence = DOTween.Sequence(); {
				gaugeSequence.AppendInterval(attributes.GaugeDecreaseDelay);
				gaugeSequence.Append(gauge.DOFillAmount(0f, attributes.GaugeDecreaseDuration).SetEase(attributes.GaugeDecreaseEase));
			}
		}
        #endregion

        #region Reset
        public void ResetBehaviour() {
            if (increaseSequence.IsActive()) {
				increaseSequence.Complete(false);
            }

			if (completeSequence.IsActive()) {
				completeSequence.Complete(false);
			}

			if (creditsSequence.IsActive()) {
				creditsSequence.Complete(false);
			}

			if (fadeSequence.IsActive()) {
				fadeSequence.Kill();
			}

			if (gaugeSequence.IsActive()) {
				gaugeSequence.Kill();
			}

			foreach (var _feedback in allFeedbacks) {
                _feedback.Complete();
            }

            allFeedbacks.Clear();

			isWaitingForNextUpdate = false;
			nextScore = 0;
			currentScore = scoreIncrease
						 = 0f;

			scoreText.text = "000";
			pause.alpha = 0f;
			gauge.fillAmount = 0f;

			foreach (var button in menuButtons) {
				button.Selectable.enabled = false;
			}

			foreach (var button in creditsButtons) {
				button.Selectable.enabled = false;
			}

			foreach (var button in pauseButtons) {
				button.Selectable.enabled = false;
			}
		}
        #endregion
    }
}
