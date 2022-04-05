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
        [SerializeField, Enhanced, Range(.1f, 1f)] private float volumeScale = .75f ;

        private Pattern pattern = null;
        private Vector2 startPosition, endPosition;
        private Sequence sequence = null; 
        public bool IsActive { get; private set; } = false;
        #endregion

        #region Methods 
        public void InitPattern(Pattern _pattern, Vector2 _start, Vector2 _end)
        {
            pattern = _pattern;
            root.localScale *= pattern.SizeMultiplier;
            renderer.sprite = _pattern.Sprites[0];
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
            root.gameObject.SetActive(true);
            warningRenderer.gameObject.SetActive(true);

            if (pattern.PatternType == PatternType.Slap || pattern.PatternType == PatternType.Dog)
            {
                endPosition = startPosition;
                warningRenderer.transform.localPosition = startPosition + Vector2.one;
                targetRenderer.transform.localPosition = endPosition;
                if (pattern.PatternType == PatternType.Slap) renderer.color = new Color(0, 0, 0, .75f); ;
            }
            else if (pattern.IsTriggerContinuous)
                warningRenderer.transform.localPosition = (startPosition + endPosition) / 2 + Vector2.one;
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
            {
                sequence.Join(renderer.DOFade(1f, pattern.FadeInDuration).SetEase(Ease.Linear));
                if (!pattern.IsTriggerContinuous) sequence.Join(targetRenderer.DOFade(1, pattern.FadeInDuration).SetEase(Ease.Linear));
                sequence.Append(warningRenderer.DOFade(0, pattern.StartingDelay/4f).SetLoops(6, LoopType.Yoyo));
                sequence.Join(warningRenderer.transform.DOScale(1, pattern.StartingDelay / 4f).SetLoops(6, LoopType.Yoyo));
                sequence.Join(root.transform.DOShakePosition(pattern.StartingDelay, .1f));
                sequence.Join(DOVirtual.Vector3(startPosition, endPosition, pattern.StartingDelay, p => lineRenderer.SetPosition(1, p)));
            }
            sequence.AppendCallback(() => collider.enabled = pattern.IsTriggerContinuous);
            if (pattern.StartSounds.Length > 0)
                sequence.AppendCallback(() => SoundManager.Instance.PlayClip(pattern.StartSounds[Random.Range(0, pattern.StartSounds.Length)], volumeScale));
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
                    _duration = pattern.Speed;
                    sequence.Append(renderer.DOColor(Color.white, _duration).SetEase(pattern.Acceleration));
                    sequence.Join(root.DOScale( pattern.SizeMultiplier +  .25f, _duration / 2).SetEase(Ease.OutCubic));
                    sequence.Append(root.DOScale(pattern.SizeMultiplier, _duration / 2).SetEase(Ease.InExpo));
                    break;
                case PatternType.Dog:
                    _duration = pattern.Speed;
                    sequence.Append(DOVirtual.DelayedCall(_duration, () => renderer.sprite = pattern.Sprites[1]));
                    break;
                default:
                    break;
            }
            lineRenderer.SetPositions(new Vector3[2] { startPosition, startPosition });
            lineRenderer.enabled = true;
            sequence.onComplete += EndPattern; 
        }

        private void EndPattern(){
            if (pattern.HitSounds.Length > 0)
                SoundManager.Instance.PlayClip(pattern.HitSounds[Random.Range(0, pattern.HitSounds.Length)], volumeScale);
            lineRenderer.enabled = false;
            lineRenderer.material.SetTextureOffset("_MainTex", Vector2.zero);
            collider.enabled = true;
            sequence = DOTween.Sequence();
            {
                root.transform.DOShakePosition(pattern.EndDuration, pattern.EndForce, pattern.EndVibrato,  90,  false,  false) ;
                sequence.Join(DOVirtual.DelayedCall(pattern.EndDuration, () => collider.enabled = false));
                sequence.Append(renderer.DOFade(0, pattern.FadeOutDuration));
                sequence.Join(targetRenderer.DOFade(0, pattern.FadeOutDuration));
                sequence.AppendCallback(Reset);
            }
        }

        public void Stop(bool _calledFromManager = false) {
            if(!_calledFromManager) PatternsManager.Instance.Stop();
            collider.enabled = false;
            sequence.Kill(false);
            if (pattern.IsTriggerContinuous && !_calledFromManager)
            {
                EndPattern();
            }
            else
                Reset();
        }

        private void Reset(){

            warningRenderer.gameObject.SetActive(false);
            lineRenderer.enabled = false;
            root.gameObject.SetActive(false);
            root.localScale = Vector3.one;
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
