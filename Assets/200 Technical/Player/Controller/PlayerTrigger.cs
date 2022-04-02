// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using UnityEngine;

namespace LudumDare50 {
	public class PlayerTrigger : MonoBehaviour {
        #region Behaviour
        private void OnTriggerEnter2D(Collider2D collision) {
            PlayerController.Instance.EnterTrigger(collision);
        }
        #endregion
    }
}
