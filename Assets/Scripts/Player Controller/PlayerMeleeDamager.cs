using FightSystem.Damage;
using Inventory_System;
using Items_System.Items;
using UnityEngine;

namespace Player_Controller
{
    public class PlayerMeleeDamager : MonoBehaviour
    {
        [SerializeField] private float _damageRange = 1f;
        [SerializeField] private LayerMask _damageLayer;

        private bool _canDamage = true;

        private void Start()
            => _canDamage = true;

        public void TryDamage(Tool gatheringTool)
        {
            if (!_canDamage) return;
            var cameraTransform = Camera.main.transform;

            var ray = new Ray(cameraTransform.position, cameraTransform.forward);
            var targets = Physics.RaycastAll(ray, _damageRange, _damageLayer);
            foreach (var target in targets)
            {
                var damagable = target.collider.GetComponent<IDamagable>();
                if (damagable != null)
                    damagable.GetDamageToServer(gatheringTool.Damage);

                var damagableBuilding = target.collider.GetComponent<IBuildingDamagable>();
                if (damagableBuilding != null)
                    damagableBuilding.GetDamageToServer(gatheringTool.Id);

                if (damagableBuilding != null || damagable != null)
                {
                    InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.MinusCurrentHp(1);
                    return;
                }
            }
        }
    }
}