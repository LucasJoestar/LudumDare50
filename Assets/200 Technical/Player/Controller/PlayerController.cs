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
        private const int BASE_INGREDIENT_COUNT = 1;

        [Section("Player Controller")]

        [SerializeField, Enhanced, Required] private PlayerControllerAttributes attributes = null;
        [SerializeField, Enhanced, Required] private Transform root = null;

        [Space(10f)]

        [SerializeField, Enhanced, MinMax(-100f, 100f)] private Vector2 horizontalBounds = new Vector2(-5f, 5f);
        [SerializeField, Enhanced, MinMax(-100f, 100f)] private Vector2 verticalBounds = new Vector2(-5f, 5f);

        [Space(10f)]

        [SerializeField, Enhanced, ReadOnly] private bool isPlayable = true;
        [SerializeField, Enhanced, ReadOnly, Range(0f, 1f)] private float instability = 0f;
        [SerializeField, Enhanced, ReadOnly] private int ingredientCount = BASE_INGREDIENT_COUNT;

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

            filter.useTriggers = true;
            filter.useLayerMask = true;
            filter.layerMask = attributes.LayerMask;

            // Get inputs.
            moveInput = GameManager.Instance.Settings.Inputs.asset.FindAction(attributes.MoveInput, true);

            ResetBehaviour();
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
            if (!isPlayable || moveSequence.IsActive()) {
                return;
            }

            Vector2 input = moveInput.ReadValue<Vector2>();
            if (input != Vector2.zero) {
                movementInput = input.normalized;

                // Wait during interval.
                moveSequence = DOTween.Sequence(); {
                    moveSequence.Join(DOVirtual.DelayedCall(attributes.MovementDelay, Move, false));
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
        private Sequence collectSequence = null;

        // ---------------

        private void Collect(/*Ingredient ingredient*/) {
            isPlayable = false;

            collectSequence = DOTween.Sequence(); {
                collectSequence.Join(DOVirtual.DelayedCall(attributes.CollectDuration, OnCollect, false));
            }
        }

        private void OnCollect() {
            isPlayable = true;
        }
        #endregion

        #region Lifetime
        private void Splash() {
            isPlayable = false;
        }
        #endregion

        #region Trigger
        public void EnterTrigger(Collider2D collision) {
            // Implement behaviour here.
        }
        #endregion

        #region Reset
        public void ResetBehaviour() {
            // Stop all tween and sequences.
            if (moveSequence.IsActive()) {
                collectSequence.Complete(false);
            }

            if (collectSequence.IsActive()) {
                collectSequence.Complete(false);
            }

            // Reset the whole behaviour.
            thisTransform.position = initialPosition;

            ingredientCount = BASE_INGREDIENT_COUNT;
            instability = 0f;
            isPlayable = true;
        }
        #endregion
    }
}
