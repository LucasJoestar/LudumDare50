// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using EnhancedEditor;
using UnityEngine;

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
        [Section("Game Manager")]

        [SerializeField, Enhanced, Required] public GameSettings Settings = null;
        [SerializeField, Enhanced, Required] public Camera mainCamera = null;
        [SerializeField, Enhanced, Required] public Camera renderCamera = null;
        #endregion

        #region Behaviour
        private const float TARGET_RATIO = 16f / 9f;
        private float lastRatio = 0f;

        // ---------------

        protected override void OnEnable() {
            Cursor.visible = false;
            Settings.Inputs.asset.Enable();

            base.OnEnable();
        }

        private void Update() {
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
    }
}
