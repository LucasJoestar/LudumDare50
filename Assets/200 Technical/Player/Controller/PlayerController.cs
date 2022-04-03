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
        private const int BASE_INGREDIENT_COUNT = 2;

        [Section("Player Controller")]

        [SerializeField, Enhanced, Required] private PlayerControllerAttributes attributes = null;
        [SerializeField, Enhanced, Required] private PlayerIK ik = null;
        [SerializeField, Enhanced, Required] private Transform root = null;

        [Space(10f)]

        [SerializeField, Enhanced, MinMax(-100f, 100f)] private Vector2 horizontalBounds = new Vector2(-5f, 5f);
        [SerializeField, Enhanced, MinMax(-100f, 100f)] private Vector2 verticalBounds = new Vector2(-5f, 5f);

        [Space(10f)]

        [SerializeField, Enhanced, ReadOnly] private bool isPlayable = true;
        [SerializeField, Enhanced, ReadOnly, Range(-1f, 1f)] private float instability = 0f;
        [SerializeField, Enhanced, ReadOnly] private int ingredientCount = BASE_INGREDIENT_COUNT;

        public static int IngredienMask { get; private set; }
        public static int PlayerMask { get; private set; }

        // ---------------

        private Transform thisTransform = null;
        private Vector3 initialPosition = Vector3.zero;

        private InputAction moveInput = null;

        private ContactFilter2D filter = new ContactFilter2D();
        #endregion

        #region Behaviour
        protected override void OnEnable() {
            base.OnEnable();

            // Initialization.
            thisTransform = transform;
            initialPosition = thisTransform.position;

            filter.useTriggers = true;
            filter.useLayerMask = true;
            filter.layerMask = attributes.LayerMask;

            IngredienMask = LayerMask.NameToLayer("Ingredient");
            PlayerMask = LayerMask.NameToLayer("Player");
    }

        private void Start() {
            // Get inputs.
            moveInput = GameManager.Instance.Settings.Inputs.asset.FindAction(attributes.MoveInput, true);
        }

        private void Update() {
            if (!isPlayable) {
                return;
            }

            UpdateMovement();
        }
        #endregion

        #region Movement
        private static readonly Collider2D[] buffer = new Collider2D[8];

        private Sequence moveSequence = null;
        private Vector2 movementInput = Vector2.zero;

        // ---------------

        private void UpdateMovement() {
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

                float duration = attributes.MovementDuration;
                Vector2 velocity = destination - (Vector2)thisTransform.position;

                ik.ApplyJumpIK(duration, velocity.x);

                moveSequence.Join(thisTransform.DOMove(destination, duration).SetEase(attributes.MovementEase));
                moveSequence.Join(root.DOLocalMoveY(attributes.MovementRootHeight, duration).SetEase(attributes.MovementRootCurve));

                moveSequence.OnComplete(Land);
            }
        }

        private void Land() {
            if (Mathf.Abs(instability) == 1f) {
                Splash();
                return;
            }

            // Landing callback.
            ik.ApplyLandingIK(attributes.MovementLandingDuration, instability);

            // Land without falling in pieces.
            int amount = Physics2D.OverlapCircle(transform.position, attributes.OverlapRadius, filter, buffer);
            for (int i = 0; i < amount; i++) {
                Collider2D collider = buffer[i];
                Transform parent = collider.transform.parent;

                if (!ReferenceEquals(parent, null) && parent.TryGetComponent(out Bonus _bonus)) {
                    _bonus.Collect(this);
                }
            }
        }
        #endregion

        #region Bonus
        private Sequence collectSequence = null;

        // ---------------

        public void Collect(Ingredient ingredient) {
            SetPlayable(false);
            instability = 0f;

            if (collectSequence.IsActive()) {
                collectSequence.Complete(false);
            }

            float duration = attributes.CollectDuration;

            collectSequence = DOTween.Sequence(); {
                collectSequence.Join(DOVirtual.DelayedCall(duration, OnCollect, false));
            }

            ik.ApplyLandingIK(duration, ingredient);
        }

        private void OnCollect() {
            ingredientCount++;
            SetPlayable(true);
        }
        #endregion

        #region Gameplay
        private Sequence gameOverSequence = null;

        // ---------------

        private void Splash() {
            float duration = attributes.SplashDuration;

            // Splash animation is in IK.
            ik.Splash(duration);
            SetPlayable(false);

            gameOverSequence = DOTween.Sequence(); {
                gameOverSequence.Join(DOVirtual.DelayedCall(duration, GameOver, false));
            }
        }

        public void EnterTrigger(Collider2D collision) {
            if (!isPlayable)
                return;

            Transform parent = collision.transform.parent;
            if (ReferenceEquals(parent, null) || !parent.TryGetComponent(out PatternHolder pattern)) {
                return;
            }

            pattern.Stop();

            float duration = attributes.EatDuration;
            SetPlayable(false);

            if (moveSequence.IsActive()) {
                moveSequence.Pause();
            }

            // Eat sequence goes here.
            gameOverSequence = DOTween.Sequence(); {
                gameOverSequence.Join(DOVirtual.DelayedCall(duration, GameOver, false));
            }
        }

        private void GameOver() {
            if (idleSequence.IsActive()) {
                idleSequence.Complete(false);
            }

            GameManager.Instance.GameOver();
        }
        #endregion

        #region Playable
        private Sequence idleSequence = null;

        // ---------------

        public void SetPlayable(bool _isPlayable) {
            isPlayable = _isPlayable;
        }

        public void ResetBehaviour() {
            // Stop all tween and sequences.
            if (moveSequence.IsActive()) {
                moveSequence.Complete(false);
            }

            if (collectSequence.IsActive()) {
                collectSequence.Complete(false);
            }

            if (gameOverSequence.IsActive()) {
                gameOverSequence.Complete(false);
            }

            if (idleSequence.IsActive()) {
                idleSequence.Complete(false);
            }

            // Reset the whole behaviour.
            thisTransform.position = initialPosition;
            thisTransform.localScale = Vector3.one;

            idleSequence = DOTween.Sequence(); {
                idleSequence.AppendInterval(attributes.IdleDelay);
                idleSequence.Join(thisTransform.DOScale(attributes.IdleScale, attributes.IdleDuration).SetEase(attributes.IdleCurve));

                idleSequence.SetLoops(-1, LoopType.Restart);
            }

            ingredientCount = BASE_INGREDIENT_COUNT;
            instability = 0f;
            SetPlayable(false);

            ik.OnReset(BASE_INGREDIENT_COUNT);
        }
        #endregion
    }
}
