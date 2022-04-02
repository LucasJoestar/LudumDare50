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
        [SerializeField, Enhanced, Required] private PlayerIK ik = null;
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

            foreach (var ingredient in gameObject.GetComponentsInChildren<Ingredient>()) {
                ingredient.gameObject.layer = attributes.PlayerMask;
            }

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
            if (!isPlayable) {
                return;
            }

            Vector2 input = moveInput.ReadValue<Vector2>();
            if (moveSequence.IsActive()) {
                if (input != Vector2.zero) {
                    movementInput = input;
                }

                return;
            }

            if (input != Vector2.zero) {
                float duration = attributes.MovementDelay;

                ik.Squash(duration);
                movementInput = input;

                // Wait during interval.
                moveSequence = DOTween.Sequence(); {
                    moveSequence.Join(DOVirtual.DelayedCall(duration, Move, false));
                }
            }
        }

        private void Move() {
            Vector2 input = moveInput.ReadValue<Vector2>();
            if (input != Vector2.zero) {
                movementInput = input;
            }

            moveSequence.Kill();

            // Move sequence.
            moveSequence = DOTween.Sequence(); {
                Vector2 destination = thisTransform.position;
                input = movementInput.normalized;

                if (Mathf.Abs(input.x) > .4f) {
                    destination.x += GameManager.Instance.Settings.Unit * attributes.MovementUnit * Mathf.Sign(input.x);
                }

                if (Mathf.Abs(input.y) > .4f) {
                    destination.y += GameManager.Instance.Settings.Unit * attributes.MovementUnit * Mathf.Sign(input.y);
                }

                destination.x = Mathf.Clamp(destination.x, horizontalBounds.x, horizontalBounds.y);
                destination.y = Mathf.Clamp(destination.y, verticalBounds.x, verticalBounds.y);

                float duration = attributes.MovementDelay;
                Vector2 velocity = destination - (Vector2)thisTransform.position;

                ik.ApplyJumpIK(duration, velocity.x);

                moveSequence.Join(thisTransform.DOMove(destination, attributes.MovementDuration).SetEase(attributes.MovementEase));
                moveSequence.Join(root.DOLocalMoveY(attributes.MovementRootHeight, attributes.MovementDuration).SetEase(attributes.MovementRootCurve));

                moveSequence.OnComplete(Land);
            }
        }

        private void Land() {
            if (instability == 1f) {
                Splash();
                return;
            }

            // Landing callback.
            ik.ApplyLandingIK(attributes.MovementLandingDuration, instability);

            // Land without falling in pieces.
            int amount = Physics2D.OverlapCircle(transform.position, attributes.OverlapRadius, filter, buffer);
            for (int i = 0; i < amount; i++) {
                Collider2D collider = buffer[i];

                if (collider.TryGetComponent(out Ingredient ingredient)) {
                    Collect(ingredient);
                }
            }
        }
        #endregion

        #region Ingredient
        private Sequence collectSequence = null;

        // ---------------

        private void Collect(Ingredient ingredient) {
            isPlayable = false;

            if (collectSequence.IsActive()) {
                collectSequence.Complete(false);
            }

            float duration = attributes.CollectDuration;
            ingredient.gameObject.layer = attributes.PlayerMask;

            collectSequence = DOTween.Sequence(); {
                collectSequence.Join(DOVirtual.DelayedCall(duration, OnCollect, false));
            }

            ik.ApplyLandingIK(duration, ingredient);
        }

        private void OnCollect() {
            isPlayable = true;
        }
        #endregion

        #region Status
        private void Splash() {
            // IK callback goes here.
            ik.Splash(1f);

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
                moveSequence.Complete(false);
            }

            if (collectSequence.IsActive()) {
                collectSequence.Complete(false);
            }

            // Reset the whole behaviour.
            thisTransform.position = initialPosition;

            ingredientCount = BASE_INGREDIENT_COUNT;
            instability = 0f;
            isPlayable = true;

            ik.OnReset(BASE_INGREDIENT_COUNT);
        }
        #endregion
    }
}
