using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace TechTree
{
    public class TechnologyManager : NetworkBehaviour
    {
        public static TechnologyManager Singleton { get; private set; }

        [field: SerializeField]
        public NetworkVariable<CustomSendingTechnologyArrayData> TechArrayNetData { get; private set; } = new();

        private void Awake()
            => Singleton = this;

        [ServerRpc(RequireOwnership = false)]
        public void AddTechServerRpc(int itemId, int userId)
        {
            if (!IsServer) return;
            var data = GetTech(itemId, userId);

            TechArrayNetData.Value = new CustomSendingTechnologyArrayData(AddItem(itemId, data, TechArrayNetData.Value));
        }

        private int TryAddToExistedTech(int addingItemId, CustomSendingTechnologyData item, out CustomSendingTechnologyData res)
        {
            for (int i = 0; i < TechArrayNetData.Value.TechnologyArray.Length; i++)
            {
                if (TechArrayNetData.Value.TechnologyArray[i].UserId == item.UserId)
                {
                    var tech = TechArrayNetData.Value.TechnologyArray[i];
                    tech.AddItem(addingItemId);
                    res = tech;
                    return i;
                }
            }

            res = default;
            return -1;
        }

        private CustomSendingTechnologyData[] AddItem(int addingItemId, CustomSendingTechnologyData item,
            CustomSendingTechnologyArrayData data)
        {
            var buff = new CustomSendingTechnologyData[data.TechnologyArray.Length];

            var addingTechId = TryAddToExistedTech(addingItemId, item, out CustomSendingTechnologyData addingTech);
            if (addingTechId != -1)
            {
                buff[addingTechId] = addingTech;
                TechArrayNetData.Value = new CustomSendingTechnologyArrayData(buff);
            }
            else
            {
                buff = new CustomSendingTechnologyData[data.TechnologyArray.Length + 1];
                data.TechnologyArray.CopyTo(buff, 0);
                buff[data.TechnologyArray.Length] = item;
            }

            return buff;
        }

        private CustomSendingTechnologyData GetTech(int techItemId, int userId)
        {
            foreach (var tech in TechArrayNetData.Value.TechnologyArray)
            {
                if (tech.UserId != userId) continue;
                return tech;
            }

            var data = new CustomSendingTechnologyData(new int[] { techItemId }, userId);
            return data;
        }
    }
}