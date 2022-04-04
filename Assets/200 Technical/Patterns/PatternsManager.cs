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

        public void StartRandomPattern()
        {
            PatternHolder _holder = GetHolderFromQueue();
            lastPatternIndex++;
            if (lastPatternIndex >= patterns.Length /*|| patterns[lastPatternIndex].MinimumIngredient > PlayerController.Instance.IngredientCount*/) 
                lastPatternIndex = 0;
            Pattern _pattern = patterns[lastPatternIndex].GetRandomPattern();

            Vector2 _startPosition = (Vector2)PlayerController.Instance.transform.position + (Vector2.right * Random.value * 5.5f) + (Vector2.up * Random.value * 5.5f);
            // if (Random.value >= .5f) _startPosition *= -1;
            // _startPosition = Vector2.Max(_startPosition, new Vector2(PlayerController.Instance.HorizontalBounds.x, PlayerController.Instance.VerticalBounds.x)); 
            // _startPosition = Vector2.Min(_startPosition, new Vector2(PlayerController.Instance.HorizontalBounds.y, PlayerController.Instance.VerticalBounds.y)); 

            Vector2 _endPosition = (Vector2)PlayerController.Instance.transform.position + (Vector2.right * Random.value * 5.5f) + (Vector2.up * Random.value * 5.5f);
            // if (Random.value >= .5f) _endPosition *= -1;
            // _endPosition = Vector2.Max(_endPosition, new Vector2(PlayerController.Instance.HorizontalBounds.x, PlayerController.Instance.VerticalBounds.x));
            // _endPosition = Vector2.Min(_endPosition, new Vector2(PlayerController.Instance.HorizontalBounds.y, PlayerController.Instance.VerticalBounds.y));

            _holder.InitPattern(_pattern, _startPosition, _endPosition);
            StartSequence();
        }

        [Button]
        void StartSequence()
        {
            sequence = DOTween.Sequence();
            sequence.AppendInterval(waitingInterval);
            sequence.AppendCallback(StartRandomPattern);               
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
