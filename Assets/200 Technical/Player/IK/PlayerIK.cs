// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using System.Collections.Generic;
using UnityEngine;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50
{
	public class PlayerIK : MonoBehaviour
    {
        #region Global Members
        [Section("PlayerIK")]
        [SerializeField] private List<Ingredient> ingredients = new List<Ingredient>();
        [SerializeField, Enhanced, Range(0.0f, 1.0f)] private float verticalOffset = .5f;
        [SerializeField, Enhanced] private PlayerIKAttributes attributes = null;

        private Sequence verticalSequence = null;
        private Sequence horizontalSequence = null;

        public List<Ingredient> Ingredients => ingredients;
        #endregion


        #region Methods
        /// <summary>
        /// Called when a new ingredient is added to the burger
        /// </summary>
        /// <param name="_duration">Duration of the sequence</param>
        /// <param name="_newIngredient">New Ingredient to add</param>
        private void AddNewIngredient(float _duration, Ingredient _newIngredient)
        {
            ingredients.Add(_newIngredient);
            _newIngredient.transform.SetParent(transform);
            float _height = verticalOffset;
            // Jump
            verticalSequence.AppendInterval(0);
            for (int i = ingredients.Count; i-- > 0;)
            {
                _height += ingredients[i].Height;
                verticalSequence.Join(ingredients[i].transform.DOLocalMoveY(_height + attributes.BoneLengthMax * (ingredients.Count - i), _duration/2).SetEase(attributes.StretchCurve));
            }
            horizontalSequence.Append(_newIngredient.transform.DOLocalMoveX(0, _duration/2).SetEase(attributes.HorizontalIngredientCurve));
            // Squash
            verticalSequence.AppendInterval(0);
            _height = verticalOffset;
            for (int i = ingredients.Count; i-- > 0;)
            {
                verticalSequence.Join(ingredients[i].transform.DOLocalMoveY(_height, _duration/2).SetEase(attributes.SquashCurve));
                _height += ingredients[i].Height;
            }
        }

        /// <summary>
        /// Called before a jump to squash the burger
        /// </summary>
        /// <param name="_squashDuration">Duration of the sequence</param>
        public void Squash(float _squashDuration)
        {
            if(verticalSequence.IsActive())
                verticalSequence.Kill(false);
            verticalSequence = DOTween.Sequence();
            float _height = verticalOffset;
            for (int i = ingredients.Count; i-- > 0;)
            {
                verticalSequence.Join(ingredients[i].transform.DOLocalMoveY(_height, _squashDuration).SetEase(attributes.SquashCurve));
                _height += ingredients[i].Height;
            }
            verticalSequence.Join(transform.DOScaleY(1.0f-attributes.SquashMultiplier, _squashDuration).SetEase(attributes.SquashCurve));
            verticalSequence.Join(transform.DOScaleX(1.0f+attributes.SquashMultiplier, _squashDuration).SetEase(attributes.SquashCurve));
            verticalSequence.Play();
        }

        /// <summary>
        /// Called during a jump.
        /// Stretch the burger
        /// </summary>
        /// <param name="_stretchDuration">Duration of the sequence</param>
        public void JumpVertical(float _stretchDuration)
        {
            if (verticalSequence.IsActive())
                verticalSequence.Kill(false);
            verticalSequence = DOTween.Sequence();
            // Stretch 
            float _height = verticalOffset;
            for (int i = ingredients.Count; i-- > 0;)
            {
                _height += ingredients[i].Height;
                verticalSequence.Join(ingredients[i].transform.DOLocalMoveY(_height + attributes.BoneLengthMax * (ingredients.Count - i), _stretchDuration).SetEase(attributes.StretchCurve));

                verticalSequence.Join(transform.DOScaleY(1, _stretchDuration).SetEase(attributes.SquashCurve));
                verticalSequence.Join(transform.DOScaleX(1, _stretchDuration).SetEase(attributes.SquashCurve));
            }
            verticalSequence.Play();
        }
        
        /// <summary>
        /// Called during a jump and apply offset on X.
        /// </summary>
        /// <param name="_jumpDuration">Duratio of the sequence</param>
        /// <param name="_horizontalVelocity">Direction of the jump</param>
        private void JumpHorizontal(float _jumpDuration, float _horizontalVelocity)
        {
            if (horizontalSequence.IsActive())
                horizontalSequence.Kill(false);

            horizontalSequence = DOTween.Sequence();

            float _endValue = attributes.HorizontalOffset * -_horizontalVelocity; 
            for (int i = ingredients.Count; i-- > 0;)
            {
                horizontalSequence.Join(ingredients[i].transform.DOLocalMoveX(Mathf.Lerp(0, _endValue, 1f - (float)i / ingredients.Count), _jumpDuration).SetEase(attributes.HorizontalJumpCurve));
            }
            horizontalSequence.Play();
        }

        /// <summary>
        /// Call the methods JumpHorizontal and JumpVertical
        /// </summary>
        /// <param name="_jumpDuration">Duration of the sequence</param>
        /// <param name="_horizontalVelocity">Direction of the jump</param>
        public void ApplyJumpIK(float _jumpDuration, float _horizontalVelocity)
        {
            JumpVertical(_jumpDuration);
            if (_horizontalVelocity != 0) JumpHorizontal(_jumpDuration, _horizontalVelocity);
        }

        /// <summary>
        /// Call the methods Landing Vertical and Horizontal and start de sequences
        /// </summary>
        /// <param name="_landingDuration">Duration of the sequence</param>
        /// <param name="_instability">Instability of the player</param>
        public void ApplyLandingIK(float _landingDuration, float _instability)
        {
            LandingHorizontal(_landingDuration, _instability);
            LandingVertical(_landingDuration);

            horizontalSequence.Play();
            verticalSequence.Play();
        }

        /// <summary>
        /// Call the methods Landing Vertical and Horizontal and start de sequences and add a new Ingredients
        /// </summary>
        /// <param name="_landingDuration">Duration of the sequence</param>
        /// <param name="_newIngredient">Added Ingredient</param>
        public void ApplyLandingIK(float _landingDuration, /*float _gettingIngredientDuration,*/ Ingredient _newIngredient)
        {
            LandingHorizontal(_landingDuration, 0);
            LandingVertical(_landingDuration);
            AddNewIngredient(_landingDuration, _newIngredient);
            horizontalSequence.Play();
            verticalSequence.Play();
        }

        /// <summary>
        /// Apply Vertical Landing Movement 
        /// </summary>
        /// <param name="_landingDuration">Duration of the sequence</param>
        private void LandingVertical(float _landingDuration)
        {
            if (verticalSequence.IsActive())
                verticalSequence.Kill(false);
            verticalSequence = DOTween.Sequence();
            float _height = verticalOffset;
            for (int i = ingredients.Count; i-- > 0;)
            {
                verticalSequence.Join(ingredients[i].transform.DOLocalMoveY(_height, _landingDuration).SetEase(attributes.VerticalLandingCurve));
                _height += ingredients[i].Height;
            }
        }

        /// <summary>
        /// Apply Horizontal Landing Movement
        /// </summary>
        /// <param name="_landingDuration">Duration of the sequence</param>
        /// <param name="_instability"></param>
        private void LandingHorizontal(float _landingDuration, float _instability)
        {
            if (horizontalSequence.IsActive())
                horizontalSequence.Kill(false);
            horizontalSequence = DOTween.Sequence();
            float _distance = verticalOffset;
            for (int i = 0; i < ingredients.Count; i++)
            {
                _distance += ingredients[i].Height;
            }
            float _direction = Mathf.Sin(attributes.InstabilityMax * _instability) * _distance;
            for (int i = ingredients.Count; i-- > 0;)
            {
                horizontalSequence.Join(ingredients[i].transform.DOLocalMoveX(Mathf.Lerp(0, _direction, 1f - (float)i / ingredients.Count), _landingDuration).SetEase(attributes.HorizontalLandingCurve));
            }
        }

        public void Splash(float _splashDuration) {

        }
        #endregion

        public void OnReset(int _baseIngredient)
        {
            for (int i = ingredients.Count; i-- > _baseIngredient;)
            {
                Destroy(ingredients[i].gameObject);
                ingredients.RemoveAt(i);
            }
            ApplyLandingIK(0, 0);
        }
    }
}
