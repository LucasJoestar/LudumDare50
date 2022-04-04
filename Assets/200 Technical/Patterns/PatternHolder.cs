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
        [SerializeField] private SpriteRenderer targetRenderer = null;
        [SerializeField] private LineRenderer lineRenderer = null;
        [SerializeField, Enhanced, Range(.1f, 2f)] private float scrollSpeed = 1.0f ;
        private Pattern pattern = null;
        private Vector2 startPosition, endPosition;
        private Sequence sequence = null; 
        public bool IsActive { get; private set; } = false;
        #endregion

        #region Methods 
        public void InitPattern(Pattern _pattern, Vector2 _start, Vector2 _end)
        {
            pattern = _pattern;
            renderer.sprite = _pattern.Sprite;
            collider.offset = _pattern.ColliderOffset;
            collider.size = _pattern.ColliderSize;
            startPosition = _start;
            endPosition = _end;

            IsActive = true;
            StartPattern();
        }

        private void StartPattern(){
            root.transform.localPosition = startPosition;
            root.eulerAngles = Vector3.forward * Vector2.SignedAngle(Vector2.up, endPosition - startPosition);
            collider.enabled = pattern.IsTriggerContinuous;
            root.gameObject.SetActive(true);
            warningRenderer.gameObject.SetActive(true);

            if (pattern.IsTriggerContinuous)
            {
                warningRenderer.transform.localPosition = (startPosition + endPosition) / 2 + Vector2.one;

            }
            else
            {
                endPosition = Vector2.Lerp(startPosition, endPosition, Random.Range(.5f, 1f));
                targetRenderer.transform.localPosition = endPosition;
                targetRenderer.enabled = true;
                warningRenderer.transform.localPosition = endPosition + Vector2.one;
            }
            warningRenderer.gameObject.SetActive(true);

            float _duration = Vector2.Distance(startPosition, endPosition)/pattern.Speed;
            sequence = DOTween.Sequence();
            sequence.Join(warningRenderer.DOFade(0, pattern.StartingDelay/4f).SetLoops(6, LoopType.Yoyo));
            sequence.Join(root.transform.DOShakePosition(pattern.StartingDelay, .1f));
            switch (pattern.PatternType)
            {
                case PatternType.Linear:
                    sequence.Append(root.transform.DOLocalMove(endPosition, _duration).SetEase(pattern.Acceleration));
                    break;
                case PatternType.Circular:
                    root.eulerAngles = Vector3.zero;
                    renderer.flipX = startPosition.x > endPosition.x;
                    Vector3[] _path = new Vector3[3]
                    {
                        endPosition,
                        startPosition + Vector2.Perpendicular(endPosition - startPosition).normalized,
                        endPosition + Vector2.Perpendicular(endPosition - startPosition).normalized
                    };
                    sequence.Append(root.transform.DOLocalPath(_path, _duration, PathType.CubicBezier).SetEase(pattern.Acceleration));
                    break;
                case PatternType.Slap:
                    // Insert Slap Behaviour here
                    break;
                default:
                    break;
            }
            lineRenderer.SetPositions(new Vector3[2] { startPosition, endPosition });
            lineRenderer.enabled = true;
            sequence.onComplete += EndPattern; 
        }

        private void EndPattern(){
            lineRenderer.enabled = false;
            lineRenderer.material.SetTextureOffset("_MainTex", Vector2.zero);
            if (!pattern.IsTriggerContinuous) collider.enabled = true;
            sequence = DOTween.Sequence();
            {
                root.transform.DOShakePosition(pattern.EndDuration, pattern.EndForce, pattern.EndVibrato,  90,  false,  false) ;
            }
            sequence.AppendInterval(pattern.RestingDuration);
            sequence.AppendCallback(Reset);
        }

        public void Stop() {
            PatternsManager.Instance.Stop();
            collider.enabled = false;
            if (pattern.IsTriggerContinuous)
            {
                sequence.Kill(false);
                EndPattern();
            }
        }

        private void Reset(){
            warningRenderer.gameObject.SetActive(false);
            root.gameObject.SetActive(false);
            renderer.flipX = false;
            targetRenderer.enabled = false;
            IsActive = false;
            PatternsManager.Instance.PushHolderToQueue(this);
        }

        private void Update()
        {
            if(IsActive)
            {
                lineRenderer.material.SetTextureOffset("_MainTex", Vector2.left * Time.time * scrollSpeed);
            }
        }
        #endregion
    }
}
