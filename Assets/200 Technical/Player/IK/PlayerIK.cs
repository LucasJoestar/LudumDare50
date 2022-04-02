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
        #endregion


        #region Methods
        public void Squish(float _squishDuration)
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
                verticalSequence.Join(ingredients[i].transform.DOLocalMoveY(attributes.BoneLengthMinMax.y * (i + 1), _stretchDuration).SetEase(attributes.StretchCurve));
            }
            verticalSequence.Play();
        }

        public void ApplyJumpDecal(float _jumpDuration, float _horizontalVelocity)
        {
            if (horizontalSequence.IsActive())
                horizontalSequence.Kill(false);
            horizontalSequence = DOTween.Sequence();
            float _endValue = Mathf.Min(Mathf.Abs(_horizontalVelocity), attributes.HorizontalOffsetMinMax.y);
            _endValue = Mathf.Max(_endValue, attributes.HorizontalOffsetMinMax.x);
            if (_horizontalVelocity > 0)
                _endValue *= -1;
            float _horizontal = 0.0f; 
            for (int i = 0; i< ingredients.Count; i++)
            {
                _horizontal += Mathf.Lerp(0, _endValue, (1 / ingredients.Count) * (i + 1));
                horizontalSequence.Join(ingredients[i].transform.DOLocalMoveX(_horizontal, _jumpDuration).SetEase(attributes.JumpCurve));
            }
            horizontalSequence.Play();
        }

        public void ApplyLandingDecal(float _landingDuration, float _instability)
        {
            if (horizontalSequence.IsActive())
                horizontalSequence.Kill(false);

            horizontalSequence = DOTween.Sequence();
        }

        public void ApplyLandingDecal(float _landingDuration, Ingredient _newIngredient)
        {
            if (horizontalSequence.IsActive())
                horizontalSequence.Kill(false);
            horizontalSequence = DOTween.Sequence();
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
