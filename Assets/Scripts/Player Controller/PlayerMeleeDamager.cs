using System.Collections;
using Building_System.Building.Blocks;
using Building_System.Building.Placing_Objects;
using FightSystem.Damage;
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

        public bool TryDamage(Tool gatheringTool, float coolDownTime)
        {
            if (!_canDamage) return false;
            StartCoroutine(ResetCanDamageRoutine(coolDownTime));
            var cameraTransform = Camera.main.transform;
            var ray = new Ray(cameraTransform.position, cameraTransform.forward);
            if (Physics.Raycast(ray, out var hit, _damageRange, _damageLayer))
            {
                var damagable = hit.collider.GetComponent<IDamagable>();
                if (damagable != null)
                    damagable.GetDamageOnServer(gatheringTool.Damage);
                
                var damagableBuilding = hit.collider.GetComponent<IBuildingDamagable>();
                if (damagableBuilding != null)
                    damagableBuilding.GetDamageOnServer(gatheringTool.Id);


                if(damagableBuilding != null || damagable != null)
                    InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.MinusCurrentHp(1);
                return true;
            }

            return false;
        }

        private IEnumerator ResetCanDamageRoutine(float coolDownTime)
        {
            _canDamage = false;
            yield return new WaitForSeconds(coolDownTime);
            _canDamage = true;
        }
    }
}