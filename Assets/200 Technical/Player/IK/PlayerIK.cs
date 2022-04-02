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
        public void Squash(float _squishDuration)
        {
            if(verticalSequence.IsActive())
                verticalSequence.Kill(false);
            verticalSequence = DOTween.Sequence();
            //Squish
            float _height = verticalOffset;
            for (int i = ingredients.Count; i-- > 0;)
            {
                verticalSequence.Join(ingredients[i].transform.DOLocalMoveY(_height, _squishDuration).SetEase(attributes.SquishCurve));
                _height += ingredients[i].Height;
            }
            verticalSequence.Play();
        }

        public void Stretch(float _stretchDuration)
        {
            if (verticalSequence.IsActive())
                verticalSequence.Kill(false);
            verticalSequence = DOTween.Sequence();
            // Stretch 
            for (int i = ingredients.Count; i-- > 0;)
            {
                verticalSequence.Join(ingredients[i].transform.DOLocalMoveY(attributes.BoneLengthMax * (ingredients.Count - i), _stretchDuration).SetEase(attributes.StretchCurve));
            }
            verticalSequence.Play();
        }

        public void ApplyJumpDecal(float _jumpDuration, float _horizontalVelocity)
        {
            Stretch(_jumpDuration);
            if (_horizontalVelocity == 0) return;

            if (horizontalSequence.IsActive())
                horizontalSequence.Kill(false);

            horizontalSequence = DOTween.Sequence();
            float _endValue = Mathf.Min(Mathf.Abs(_horizontalVelocity), attributes.HorizontalOffsetMinMax.y);
            _endValue = Mathf.Max(_endValue, attributes.HorizontalOffsetMinMax.x);
            if (_horizontalVelocity > 0)
                _endValue *= -1;
            for (int i = ingredients.Count; i-- > 0;)
            {
                horizontalSequence.Join(ingredients[i].transform.DOLocalMoveX(Mathf.Lerp(0, _endValue, 1f - (float)i / ingredients.Count), _jumpDuration).SetEase(attributes.JumpCurve));
            }
            horizontalSequence.Play();
        }

        public void ApplyLandingDecal(float _landingDuration, float _instability)
        {
            Debug.Log("Apply Landing");
            // WORK IN PROGRESS
            if (horizontalSequence.IsActive())
                horizontalSequence.Kill(false);

            horizontalSequence = DOTween.Sequence();
            float _distance = verticalOffset;
            for (int i = 0; i < ingredients.Count; i++){
                _distance += ingredients[i].Height;
            }
            Vector2 _direction = new Vector2(Mathf.Sin(attributes.InstabilityMax * _instability), Mathf.Cos(attributes.InstabilityMax * _instability)) * _distance;
            for (int i = ingredients.Count; i-- > 0;){
                horizontalSequence.Join(ingredients[i].transform.DOLocalMoveX(Mathf.Lerp(0, _direction.x, 1f - (float)i / ingredients.Count), _landingDuration).SetEase(attributes.JumpCurve));
            }
            horizontalSequence.Play();
        }

        public void ApplyLandingDecal(float _landingDuration, Ingredient _newIngredient)
        {
            // WORK IN PROGRESS
            if (horizontalSequence.IsActive())
                horizontalSequence.Kill(false);
            horizontalSequence = DOTween.Sequence();
        }

        public void Splash(float _splashDuration) {

        }
        #endregion

        public void OnReset(int _baseIngredient)
        {
            for (int i = ingredients.Count; i-- > _baseIngredient;)
            {
                Destroy(ingredients[i]);
                ingredients.RemoveAt(i);
            }
            for (int i = 0; i < ingredients.Count; i++)
            {
                ingredients[i].transform.localPosition = new Vector3(0,ingredients[i].transform.localPosition.y,0);
            }
        }
    }
}
