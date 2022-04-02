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
        private void AddNewIngredient(float _duration, Ingredient _newIngredient)
        {
            Debug.Log("IMPROVE THIS METHOD");
            ingredients.Add(_newIngredient);
            verticalSequence.AppendInterval(0);
            float _height = verticalOffset;
            // Jump
            for (int i = ingredients.Count; i-- > 0;)
            {
                _height += ingredients[i].Height;
                verticalSequence.Join(ingredients[i].transform.DOLocalMoveY(_height + attributes.BoneLengthMax * (ingredients.Count - i), _duration/2).SetEase(attributes.StretchCurve));

                verticalSequence.Join(transform.DOScaleY(1, _duration/2).SetEase(attributes.SquashCurve));
                verticalSequence.Join(transform.DOScaleX(1, _duration/2).SetEase(attributes.SquashCurve));
            }
            _height = verticalOffset;
            verticalSequence.AppendInterval(0);
            // Squash
            for (int i = ingredients.Count; i-- > 0;)
            {
                verticalSequence.Join(ingredients[i].transform.DOLocalMoveY(_height, _duration/2).SetEase(attributes.SquashCurve));
                _height += ingredients[i].Height;
            }
            verticalSequence.Join(transform.DOScaleY(1.0f - attributes.SquashMultiplier, _duration/2).SetEase(attributes.SquashCurve));
            verticalSequence.Join(transform.DOScaleX(1.0f + attributes.SquashMultiplier, _duration/2).SetEase(attributes.SquashCurve));
        }

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

        private void JumpHorizontal(float _jumpDuration, float _horizontalVelocity)
        {
            Debug.Log("IMPROVE HERE AS WELL");
            if (horizontalSequence.IsActive())
                horizontalSequence.Kill(false);

            horizontalSequence = DOTween.Sequence();

            float _endValue = attributes.HorizontalOffset * -_horizontalVelocity; 
            for (int i = ingredients.Count; i-- > 0;)
            {
                horizontalSequence.Join(ingredients[i].transform.DOLocalMoveX(Mathf.Lerp(0, _endValue, 1f - (float)i / ingredients.Count), _jumpDuration).SetEase(attributes.JumpCurve));
            }
            horizontalSequence.Play();
        }

        public void ApplyJumpIK(float _jumpDuration, float _horizontalVelocity)
        {
            JumpVertical(_jumpDuration);
            if (_horizontalVelocity != 0) JumpHorizontal(_jumpDuration, _horizontalVelocity);
        }

        public void ApplyLandingIK(float _landingDuration, float _instability)
        {
            LandingHorizontal(_landingDuration, _instability);
            LandingVertical(_landingDuration);

            horizontalSequence.Play();
            verticalSequence.Play();
        }

        public void ApplyLandingIK(float _landingDuration, /*float _gettingIngredientDuration,*/ Ingredient _newIngredient)
        {
            LandingHorizontal(_landingDuration, 0);
            LandingVertical(_landingDuration);
            AddNewIngredient(_landingDuration, _newIngredient);
            horizontalSequence.Play();
            verticalSequence.Play();
        }


        private void LandingVertical(float _landingDuration)
        {
            if (verticalSequence.IsActive())
                verticalSequence.Kill(false);
            verticalSequence = DOTween.Sequence();
            float _height = verticalOffset;
            for (int i = ingredients.Count; i-- > 0;)
            {
                verticalSequence.Join(ingredients[i].transform.DOLocalMoveY(_height, _landingDuration).SetEase(attributes.LandingCurve));
                _height += ingredients[i].Height;
            }
        }

        private void LandingHorizontal(float _landingDuration, float _instability)
        {
            Debug.Log("IMRPOVE HERE");
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
                horizontalSequence.Join(ingredients[i].transform.DOLocalMoveX(Mathf.Lerp(0, _direction, 1f - (float)i / ingredients.Count), _landingDuration).SetEase(attributes.JumpCurve));
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
