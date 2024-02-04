using System;

namespace CharacterStatsSystem
{
    public static class CharacterStatsEventsContainer
    {
        public static Action<CharacterStats> OnCharacterStatsAssign { get; set; }
        public static Action<CharacterStatType, int> OnCharacterStatAdded { get; set; }
        public static Action<CharacterStatType, int> OnCharacterStatRemoved { get; set; }
    }
}