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
        #endregion

        #region Behaviour
        protected override void OnEnable() {
            Settings.Inputs.asset.Enable();

            base.OnEnable();
        }

        protected override void OnDisable() {
            base.OnDisable();

            Settings.Inputs.asset.Disable();
        }
        #endregion
    }
}
