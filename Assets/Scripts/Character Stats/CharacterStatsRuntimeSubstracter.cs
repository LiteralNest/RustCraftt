using System.Threading.Tasks;
using UnityEngine;

namespace Character_Stats
{
    public class CharacterStatsRuntimeSubstracter : MonoBehaviour
    {
        [Header("Atached Scripts")]
        [SerializeField] private CharacterStats _characterStats;
        [Header("Main Params")]
        [SerializeField] private float _minusingValuePerSecond;

        private bool _minusing;
    
        private void Awake()
        {
            if(_characterStats == null)
                _characterStats = GetComponent<CharacterStats>();
        }

        private void Update()
        {
            if(_minusing) return;
            SubstractStats();
        }

        private async void SubstractStats()
        {
            _minusing = true;
            await Task.Delay(1000);
            _characterStats.MinusStat(CharacterStatType.Food, _minusingValuePerSecond);
            _characterStats.MinusStat(CharacterStatType.Water, _minusingValuePerSecond);
            _minusing = false;
        }
    }
}
