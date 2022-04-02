// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using UnityEngine;
using UnityEngine.InputSystem;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50 {
    public class PlayerController : Singleton<PlayerController> {
        #region Global Members
        [Section("Player Controller")]

        [SerializeField, Enhanced, Required] private PlayerControllerAttributes attributes = null;
        [SerializeField, Enhanced, Required] private Transform root = null;

        [Space(10f)]

        [SerializeField, Enhanced, MinMax(-100f, 100f)] private Vector2 horizontalBounds = new Vector2(-5f, 5f);
        [SerializeField, Enhanced, MinMax(-100f, 100f)] private Vector2 verticalBounds = new Vector2(-5f, 5f);

        [Space(10f)]

        [SerializeField, Enhanced, ReadOnly, Range(0f, 1f)] private float instability = 0f;
        [SerializeField, Enhanced] private int ingredientCount = 1;

        // ---------------

        private Transform thisTransform = null;
        private Vector3 initialPosition = Vector3.zero;

        private InputAction moveInput = null;

        private ContactFilter2D filter = new ContactFilter2D();
        #endregion

        #region Behaviour
        private void Start() {
            // Initialization.
            thisTransform = transform;
            initialPosition = thisTransform.position;

            // Get inputs.
            moveInput = GameManager.Instance.Settings.Inputs.asset.FindAction(attributes.MoveInput, true);
        }

        private void Update() {
            UpdateMovement();
        }
        #endregion

        #region Movement
        private static readonly Collider2D[] buffer = new Collider2D[8];

        private Sequence moveSequence = null;
        private Vector2 movementInput = Vector2.zero;

        // ---------------

        private void UpdateMovement() {
            if (moveSequence.IsActive()) {
                return;
            }

            Vector2 input = moveInput.ReadValue<Vector2>();
            if (input != Vector2.zero) {
                movementInput = input.normalized;

                // Wait during interval.
                moveSequence = DOTween.Sequence(); {
                    moveSequence.Join(DOVirtual.DelayedCall(attributes.MovementDelay, Move));
                }
            }
        }

        private void Move() {
            Vector2 input = moveInput.ReadValue<Vector2>();
            if (input != Vector2.zero) {
                movementInput = input.normalized;
            }

            moveSequence.Kill();

            // Move sequence.
            moveSequence = DOTween.Sequence(); {
                Vector2 destination = thisTransform.position;
                if (Mathf.Abs(movementInput.x) > .4f) {
                    destination.x += GameManager.Instance.Settings.Unit * attributes.MovementUnit * Mathf.Sign(movementInput.x);
                }

                if (Mathf.Abs(movementInput.y) > .4f) {
                    destination.y += GameManager.Instance.Settings.Unit * attributes.MovementUnit * Mathf.Sign(movementInput.y);
                }

                destination.x = Mathf.Clamp(destination.x, horizontalBounds.x, horizontalBounds.y);
                destination.y = Mathf.Clamp(destination.y, verticalBounds.x, verticalBounds.y);

                moveSequence.Join(thisTransform.DOMove(destination, attributes.MovementDuration).SetEase(attributes.MovementEase));
                moveSequence.Join(root.DOLocalMoveY(attributes.MovementRootHeight, attributes.MovementDuration).SetEase(attributes.MovementRootCurve));
            }
        }

        private void Land() {
            if (instability == 1f) {
                Splash();
                return;
            }

            // Land without falling in pieces.
            int amount = Physics2D.OverlapCircle(transform.position, attributes.OverlapRadius, filter, buffer);
            for (int i = 0; i < amount; i++) {
                Collider2D collider = buffer[i];

                /*if (collider.TryGetComponent(out Ingredient ingredient)) {
                    Collect(ingredient);
                }*/
            }
        }
        #endregion

        #region Ingredient
        /*private void Collect(Ingredient ingredient) {

        }*/
        #endregion

        #region Lifetime
        private void Splash() {

        }
        #endregion

        #region Trigger
        public void EnterTrigger(Collider2D collision) {
            // Implement behaviour here.
        }
        #endregion

        #region Reset
        public void ResetBehaviour() {
            thisTransform.position = initialPosition;
        }
        #endregion
    }
}
