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
	public class SpawnManager : Singleton<SpawnManager>
    {
        #region Global Members
        [Section("SpawnManager")]

        // [SerializeField, Enhanced, Range(0f, 60f)] private float spawnInterval = 10f;
        [SerializeField] private Bonus[] bonuses = new Bonus[] { };
        [SerializeField] private bool useEarlyRespawn = false;
        private Sequence sequence = null;
        private List<Bonus> spawnedBonuses = new List<Bonus>();
        #endregion

        #region Methods
        public void StartSpawnSequence()
        {
            sequence = DOTween.Sequence();
            {
                sequence.AppendInterval(Random.Range(GameManager.Instance.CurrentStep.BonusSpawnInterval.x, GameManager.Instance.CurrentStep.BonusSpawnInterval.y));
                sequence.AppendCallback(ApplySpawn);
            }
        }

        public void ApplySpawn()
        {
            Vector2 _position = new Vector2(
                Random.Range((int)PlayerController.Instance.HorizontalBounds.x + 1, (int)PlayerController.Instance.HorizontalBounds.y),
                Random.Range((int)PlayerController.Instance.VerticalBounds.x + 1, (int)PlayerController.Instance.VerticalBounds.y));
            if(spawnedBonuses.Exists(b => (Vector2)b.transform.position == _position))
            {
                ApplySpawn();
                return;
            }
            Bonus _bonus = Instantiate(bonuses[Random.Range(0, bonuses.Length)], _position, Quaternion.identity);

            spawnedBonuses.Add(_bonus);
            StartSpawnSequence();
        }
        
        public void ApplySpawn(Bonus _bonus, Vector2 _position)
        {
            _bonus = Instantiate(_bonus, _position, Quaternion.identity);

            spawnedBonuses.Add(_bonus);
            StartSpawnSequence();
        }

        public void OnBonusGet(Bonus _bonus)
        {
            spawnedBonuses.Remove(_bonus);
            // Cancel the sequence and Spawn a new bonus immediatly
            if(useEarlyRespawn)
            {
                if (sequence.IsActive())
                    sequence.Kill(false);
                ApplySpawn();
            }
        }

        public void Reset()
        {
            sequence.Kill(false);
            for (int i = 0; i < spawnedBonuses.Count; i++)
            {
                spawnedBonuses[i].Destroy();
            }
            spawnedBonuses.Clear();
        }
        #endregion
    }
}
