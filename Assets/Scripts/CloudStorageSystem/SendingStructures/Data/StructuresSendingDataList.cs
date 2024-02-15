using System.Collections.Generic;

namespace CloudStorageSystem.SendingStructures.Data
{
    public struct StructuresSendingDataList
    {
        public List<StructureSendingData> Structures { get; set; }

        public StructuresSendingDataList(List<StructureSendingData> structures)
            => Structures = new List<StructureSendingData>(structures);
    }
}