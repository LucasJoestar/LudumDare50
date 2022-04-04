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
        [SerializeField] private PatternInfos[] patterns = new PatternInfos[] { };
        [SerializeField] private float waitingInterval = 10.0f;
        private Queue<PatternHolder> availableHolders = new Queue<PatternHolder>();
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

            return _holder;
        }

        public void PushHolderToQueue(PatternHolder _holder) => availableHolders.Enqueue(_holder);

        [Button]
        private void StartRandomPattern()
        {
            PatternHolder _holder = GetHolderFromQueue();
            lastPatternIndex++;
            if (lastPatternIndex >= patterns.Length || patterns[lastPatternIndex].MinimumIngredient > PlayerController.Instance.IngredientCount) 
                lastPatternIndex = 0;
            Pattern _pattern = patterns[lastPatternIndex].GetRandomPattern();

            Vector2 _startPosition = Vector2.zero, _endPosition = Vector2.zero;
            // From Left - Right 
            if(Random.value >= .5f) {
                // Horizontal
                if (Random.value >= .5f) {
                    _startPosition.x = PlayerController.Instance.HorizontalBounds.x + 1;
                    _endPosition.x = PlayerController.Instance.HorizontalBounds.y - 1;

                }
                else {
                    _startPosition.x = PlayerController.Instance.HorizontalBounds.y - 1;
                    _endPosition.x = PlayerController.Instance.HorizontalBounds.x + 1;
                }

                _startPosition.y = Random.Range(PlayerController.Instance.VerticalBounds.x + 1, PlayerController.Instance.VerticalBounds.y - 1);
                _endPosition.y = Random.Range(PlayerController.Instance.VerticalBounds.x + 1, PlayerController.Instance.VerticalBounds.y - 1);
            }
            else { // Top - Bottom
                // Vertical
                if (Random.value >= .5f)
                {
                    _startPosition.y = PlayerController.Instance.VerticalBounds.x + 1;
                    _endPosition.y = PlayerController.Instance.VerticalBounds.y - 1;

                }
                else
                {
                    _startPosition.y = PlayerController.Instance.VerticalBounds.y - 1;
                    _endPosition.y = PlayerController.Instance.VerticalBounds.x + 1;
                }

                _startPosition.x = Random.Range(PlayerController.Instance.HorizontalBounds.x + 1, PlayerController.Instance.HorizontalBounds.y - 1);
                _endPosition.x = Random.Range(PlayerController.Instance.HorizontalBounds.x + 1, PlayerController.Instance.HorizontalBounds.y - 1);
            }

            _holder.InitPattern(_pattern, _startPosition, _endPosition);
            StartSequence();
        }

        public void StartSequence()
        {
            sequence = DOTween.Sequence();
            sequence.AppendInterval(waitingInterval);
            sequence.AppendCallback(StartRandomPattern);               
        }

        public void Stop()
        {
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
