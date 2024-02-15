using System.Collections.Generic;

namespace CloudStorageSystem.SendingBackPack.Data
{
    public struct BackPackListData
    {
        public List<BackPackSlotData> Slots { get; set; }
        
        public BackPackListData(List<BackPackSlotData> slots)
            => Slots = new List<BackPackSlotData>(slots);
    }
}