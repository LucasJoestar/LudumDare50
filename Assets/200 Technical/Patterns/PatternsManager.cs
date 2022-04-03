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
            Pattern _pattern = null;
            Vector2 _startPosition = Vector2.zero; // new Vector2(Random.Range(PlayerController.Instance.HorizontalBounds.x, PlayerController.Instance.HorizontalBounds.y), Random.Range(PlayerController.Instance.VerticalBounds.x, PlayerController.Instance.VerticalBounds.y));
            Vector2 _endPosition = Vector2.one; //  new Vector2(Random.Range(PlayerController.Instance.HorizontalBounds.x, PlayerController.Instance.HorizontalBounds.y), Random.Range(PlayerController.Instance.VerticalBounds.x, PlayerController.Instance.VerticalBounds.y));
            _holder.InitPattern(_pattern, _startPosition, _endPosition);
            StartSequence();
        }

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
        public Pattern[] Patterns = new Pattern[]{};
    }
}
