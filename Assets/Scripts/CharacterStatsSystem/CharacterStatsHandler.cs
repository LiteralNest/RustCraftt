using UnityEngine;

namespace CharacterStatsSystem
{
    public class CharacterStatsHandler : MonoBehaviour
    {
        private CharacterStats _characterStats;

        private void Awake()
            => CharacterStatsEventsContainer.OnCharacterStatsAssign += Init;

        private void OnDestroy()
        {
            CharacterStatsEventsContainer.OnCharacterStatAdded -= _characterStats.AddStatServerRpc;
            CharacterStatsEventsContainer.OnCharacterStatRemoved -= _characterStats.MinusStatServerRpc;
            CharacterStatsEventsContainer.OnCharacterStatsAssign -= Init;
        }

        private void Init(CharacterStats characterStats)
        {
            _characterStats = characterStats;
            CharacterStatsEventsContainer.OnCharacterStatAdded += characterStats.AddStatServerRpc;
            CharacterStatsEventsContainer.OnCharacterStatRemoved += characterStats.MinusStatServerRpc;
        }
    }
}