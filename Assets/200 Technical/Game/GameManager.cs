// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
        #region Global Members
        public static T Instance = null;
        #endregion

        #region Behaviour
        protected virtual void OnEnable() {
            if (ReferenceEquals(Instance, null)) {
                Instance = this as T;
            } else {
                enabled = false;
            }
        }

        protected virtual void OnDisable() {
            if (Instance == this) {
                Instance = null;
            }
        }
        #endregion
    }

    public class GameManager : Singleton<GameManager> {
        #region Global Members
        public const string HIGHSCORE_KEY = "PicnicPanicHighscore";

        [Section("Game Manager")]

        [SerializeField, Enhanced, Required] public GameSettings Settings = null;
        [SerializeField, Enhanced, Required] public PlayableDirector Intro = null;
        [SerializeField, Enhanced, Required] public Camera mainCamera = null;
        [SerializeField, Enhanced, Required] public Camera renderCamera = null;

        [Space(10f)]

        [SerializeField, Enhanced, ReadOnly] private bool isInMenu = false;
        [SerializeField, Enhanced, ReadOnly] private bool isInIntro = false;
        [SerializeField, Enhanced, ReadOnly] private bool isInTransition = false;
        [SerializeField, Enhanced, ReadOnly] private bool isPaused = false;

        // ---------------

        private InputAction skipInput = null;
        private InputAction pauseInput = null;
        private InputAction restartInput = null;
        private InputAction menuInput = null;
        #endregion

        #region Behaviour
        private const float TARGET_RATIO = 16f / 9f;
        private float lastRatio = 0f;

        // ---------------

        protected override void OnEnable() {
            base.OnEnable();

            Cursor.visible = false;
            Settings.Inputs.asset.Enable();
            Intro.stopped += OnIntroStopped;

            // Load inputs.
            skipInput = Settings.Inputs.asset.FindAction(Settings.SkipInput, true);
            pauseInput = Settings.Inputs.asset.FindAction(Settings.PauseInput, true);
            restartInput = Settings.Inputs.asset.FindAction(Settings.RestartInput, true);
            menuInput = Instance.Settings.Inputs.asset.FindAction(Settings.MenuInput, true);
        }

        private void Start() {
            ShowMenu(true);
        }

        private void Update() {
            // Inputs.
            if (isInIntro) {
                if (skipInput.WasPerformedThisFrame()) {
                    Intro.Stop();
                }
            } else if (!isInTransition && !isInMenu) {
                // In-game inputs.
                if (pauseInput.WasPerformedThisFrame()) {
                    PauseGame();
                } else if (!isPaused && restartInput.WasPerformedThisFrame()) {
                    RestartGame();
                } else if (!isPaused && menuInput.WasPerformedThisFrame()) {
                    ShowMenu();
                }
            }

            // Aspec ratio.
            float ratio = (float)Screen.width / Screen.height;
            if (ratio == lastRatio)
                return;

            lastRatio = ratio;
            float heightRatio = ratio / TARGET_RATIO;

            if (heightRatio < 1f) {
                Rect rect = new Rect(0f, (1f - heightRatio) / 2f, 1f, heightRatio);
                mainCamera.rect = rect;
                renderCamera.rect = rect;
            } else
              {
                float widthRatio = 1f / heightRatio;

                Rect rect = new Rect((1f - widthRatio) / 2f, 0f, widthRatio, 1f);
                mainCamera.rect = rect;
                renderCamera.rect = rect;
            }
        }

        protected override void OnDisable() {
            base.OnDisable();

            Settings.Inputs.asset.Disable();
        }
        #endregion

        #region Menu
        public void PlayGame() {
            if (!isInMenu)
                return;

            isInMenu = false;
            isInTransition = true;

            UIManager.Instance.HideMenu();
            DOVirtual.DelayedCall(Settings.IntroDelay, OnIntroStart);
        }

        // ---------------

        private void OnIntroStart() {
            isInIntro = true;
            Intro.Play();
        }

        private void OnIntroStopped(PlayableDirector playable) {
            StartPlay();
        }

        public void StartPlay() {
            UIManager.Instance.StartPlay();
            PlayerController.Instance.SetPlayable(true);

            isInIntro = false;
            isInTransition = false;
        }
        #endregion

        #region Management
        public void PauseGame() {
            isPaused = !isPaused;
            Time.timeScale = isPaused
                           ? 0f
                           : 1f;

            UIManager.Instance.PauseGame(isPaused);
        }

        public void ShowMenu(bool doForceFade = false) {
            isInTransition = true;
            isInMenu = true;

            UIManager.Instance.ShowMenu(doForceFade);
        }

        public void RestartGame() {
            isInTransition = true;

            UIManager.Instance.RestartGame();
        }

        public void GameOver() {
            // Show game over screen.
            ShowMenu();
        }

        // ---------------

        public void ResetGame() {
            PlayerController.Instance.ResetBehaviour();
            UIManager.Instance.ResetBehaviour();
            SpawnManager.Instance.Reset();
            isInTransition = false;
        }
        #endregion
    }
}
