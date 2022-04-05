// ===== Enhanced Editor - https://github.com/LucasJoestar/LudumDare50 ===== //
//
// ============================================================================ //

using DG.Tweening;
using EnhancedEditor;
using UnityEngine;
using System.Collections.Generic;
using Range = EnhancedEditor.RangeAttribute;

namespace LudumDare50
{
	public class PatternsManager : Singleton<PatternsManager>
    {
        #region Global Members
        [Section("PatternsManager")]
        [SerializeField] private PatternHolder holderPrefab = null;
        //[SerializeField] private PatternInfos[] patterns = new PatternInfos[] { };
        // [SerializeField] private float waitingInterval = 10.0f;
        private Queue<PatternHolder> availableHolders = new Queue<PatternHolder>();
        private List<PatternHolder> holders = new List<PatternHolder>();
        private Sequence sequence;
        private int lastPatternIndex = -1;
        #endregion

        #region Methods
        private PatternHolder GetHolderFromQueue()
        {
            PatternHolder _holder;
            if (availableHolders.Count > 0)
            {
                _holder = availableHolders.Peek();
                availableHolders.Dequeue();
            }
            else
                _holder = Instantiate(holderPrefab, Vector3.zero, Quaternion.identity);

            holders.Add(_holder);
            return _holder;
        }

        public void PushHolderToQueue(PatternHolder _holder)
        {
            holders.Remove(_holder);
            availableHolders.Enqueue(_holder);
        }

        private void StartRandomPattern()
        {
            PatternHolder _holder = GetHolderFromQueue();

            int _randomIndex = Random.Range(0, GameManager.Instance.CurrentStep.Pattern.Length);
            if (_randomIndex == lastPatternIndex && _randomIndex + 1 < GameManager.Instance.CurrentStep.Pattern.Length - 1)
                _randomIndex++;
            Pattern _pattern = GameManager.Instance.CurrentStep.Pattern[_randomIndex];
            lastPatternIndex = _randomIndex;

            Vector2 _startPosition = Vector2.zero, _endPosition = Vector2.zero;
            float _halfHorizontal = GameManager.Instance.renderCamera.orthographicSize * (16 / 9);
            float _halfVertical = GameManager.Instance.renderCamera.orthographicSize;
            // From Left - Right 
            if (Random.value >= .5f) {
                // Horizontal
                if (Random.value >= .5f) {
                    _startPosition.x = GameManager.Instance.renderCamera.transform.position.x - _halfHorizontal + 1;
                    _endPosition.x = GameManager.Instance.renderCamera.transform.position.x + _halfHorizontal - 1;

                }
                else {
                    _startPosition.x = GameManager.Instance.renderCamera.transform.position.x + _halfHorizontal - 1;
                    _endPosition.x = GameManager.Instance.renderCamera.transform.position.x - _halfHorizontal + 1;
                }

            }
            else { // Top - Bottom
                // Vertical
                if (Random.value >= .5f)
                {
                    _startPosition.y = GameManager.Instance.renderCamera.transform.position.y + _halfVertical - 1;
                    _endPosition.y = GameManager.Instance.renderCamera.transform.position.y - _halfVertical + 1;
                }
                else
                {
                    _startPosition.y = GameManager.Instance.renderCamera.transform.position.y - _halfVertical + 1;
                    _endPosition.y = GameManager.Instance.renderCamera.transform.position.y + _halfVertical - 1; 
                }
            }

            _holder.InitPattern(_pattern, _startPosition, _endPosition);
            StartSequence();
        }

        public void StartSequence()
        {
            sequence = DOTween.Sequence();
            sequence.AppendInterval(Random.Range(GameManager.Instance.CurrentStep.PatternSpawnInterval.x, GameManager.Instance.CurrentStep.PatternSpawnInterval.y));
            sequence.AppendCallback(StartRandomPattern);               
        }

        public void Stop()
        {
            for (int i = 0; i < holders.Count; i++)
            {
                holders[i].Stop(true);
            }
            sequence.Kill(false);
        }
        #endregion
    }

    [System.Serializable]
    public class PatternInfos
    {
        public int MinimumIngredient = 0;
        [SerializeField] private Pattern[] patterns = new Pattern[]{};

        public Pattern GetRandomPattern() => patterns[Random.Range(0, patterns.Length)];
    }
}
