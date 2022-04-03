// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// TO DO LIST
// DISPLAY PATH AND WARNING
// ============================================================================ //

using EnhancedEditor;
using UnityEngine;
using DG.Tweening;

using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50
{
	public class PatternHolder : MonoBehaviour
    {
        #region Global Members
        [Section("PatternHolder")]
        [SerializeField] private Transform root = null; 
        [SerializeField] private new SpriteRenderer renderer = null;
        [SerializeField] private new BoxCollider2D collider = null;
        [SerializeField] private SpriteRenderer warningRenderer = null;

        private Pattern pattern = null;
        private Vector2 startPosition, endPosition;
        private Sequence sequence = null; 
        public bool IsActive { get; private set; } = false;

        [SerializeField] private Pattern tempPattern; 
        #endregion

        #region Methods 
        public void InitPattern(Pattern _pattern, Vector2 _start, Vector2 _end)
        {
            pattern = _pattern;
            renderer.sprite = _pattern.Sprite;
            collider.offset = _pattern.ColliderBounds.center;
            collider.size = _pattern.ColliderBounds.size;
            startPosition = _start;
            endPosition = _end;
            /*
            warningTransform.position = endPosition;
            warningTransform.gameObject.SetActive(true); 
            */
            IsActive = true;
            StartPattern();
        }

        private void StartPattern()
        {
            root.transform.localPosition = startPosition;
            collider.enabled = pattern.IsTriggerContinuous;
            warningRenderer.transform.position = endPosition;
            warningRenderer.enabled = true; 
            float _duration = Vector2.Distance(startPosition, endPosition)/pattern.Speed;
            sequence = DOTween.Sequence();
            sequence.Join(warningRenderer.DOFade(0, pattern.StartingDelay/4f).SetEase(Ease.Linear).SetLoops(5, LoopType.Yoyo)) ;
            switch (pattern.PatternType)
            {
                case PatternType.Linear:
                    root.transform.eulerAngles = Vector3.back * Vector2.Angle(Vector2.up, endPosition - startPosition);
                    sequence.Append(root.transform.DOLocalMove(endPosition, _duration).SetEase(pattern.Acceleration));
                    break;
                case PatternType.Circular:
                    root.transform.eulerAngles = Vector3.zero;
                    Vector3[] _path = new Vector3[3]
                    {
                        endPosition,
                        startPosition + Vector2.Perpendicular(endPosition - startPosition) * .5f,
                        endPosition + Vector2.Perpendicular(endPosition - startPosition) * .5f
                    };
                    sequence.Append(root.transform.DOLocalPath(_path, _duration, PathType.CubicBezier).SetEase(pattern.Acceleration));
                    break;
                default:
                    break;
            }
            sequence.onComplete += EndPattern; 
            sequence.Play();
        }

        private void EndPattern()
        {
            //warningRenderer.enabled = false;
            if (!pattern.IsTriggerContinuous) collider.enabled = true;
            sequence = DOTween.Sequence();
            {
                root.transform.DOShakePosition(pattern.EndDuration, pattern.EndForce, pattern.EndVibrato,  90,  false,  false) ;
            }
        }

        public void Stop() {

        }

        [Button("test")]
        private void Test(Pattern _test)
        {
            InitPattern(_test, new Vector2(-10, 0), Vector2.zero);
        }
        #endregion
    }
}