using System.Collections;
using FightSystem.Damage;
using Items_System.Items;
using Unity.Netcode;
using UnityEngine;

namespace Player_Controller
{
    public class PlayerMeleeDamager : NetworkBehaviour
    {
        [SerializeField] private float _damageRange = 1f;
        [SerializeField] private LayerMask _damageLayer;

        private bool _canDamage = true;

        private void Start()
            => _canDamage = true;

        public void TryDamage(Tool gatheringTool, float coolDownTime)
        {
            if (!_canDamage) return;
            StartCoroutine(ResetCanDamageRoutine(coolDownTime));
            var cameraTransform = Camera.main.transform;
            DamageServerRpc(gatheringTool.Id, gatheringTool.Damage, cameraTransform.position, cameraTransform.forward);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DamageServerRpc(int toolId, int damage, Vector3 startPos, Vector3 direction)
        {
            var ray = new Ray(startPos, direction);
            var targets = Physics.RaycastAll(ray, _damageRange, _damageLayer);
            foreach (var target in targets)
            {
                var damagable = target.collider.GetComponent<IDamagable>();
                if (damagable != null)
                    damagable.GetDamageOnServer(damage);

                var damagableBuilding = target.collider.GetComponent<IBuildingDamagable>();
                if (damagableBuilding != null)
                    damagableBuilding.GetDamageOnServer(toolId);

                if (damagableBuilding != null || damagable != null)
                {
                    InventoryHandler.singleton.ActiveSlotDisplayer.ItemDisplayer.MinusCurrentHp(1);
                    return;
                }
            }
        }


        private IEnumerator ResetCanDamageRoutine(float coolDownTime)
        {
            _canDamage = false;
            yield return new WaitForSeconds(coolDownTime);
            _canDamage = true;
        }
    }
}