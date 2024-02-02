using Building_System.Upgrading.UI;
using Events;
using UnityEngine;

namespace Building_System.Upgrading
{
    public class BuildingUpgrader : MonoBehaviour
    {
        [Header("Attached scripts")] [SerializeField]
        private BuildingUpgradeView buildingUpgradeView;

        [Header("Main Params")] [SerializeField]
        private LayerMask _targetMask;

        [SerializeField] private Camera _targetCamera;
        [SerializeField] private float _hammerRange = 5f;

        private IHammerInteractable _hammerInteractable;

        private void OnDisable()
        {
            buildingUpgradeView.DisplayButtons(false, false, false, false);
            buildingUpgradeView.DisplayCycle(false);
        }

        private void Start()
        {
            if (_targetCamera == null)
                _targetCamera = Camera.main;
        }

        private void Update()
            => _hammerInteractable = TryRayCastUpgradable();

        private IHammerInteractable TryRayCastUpgradable()
        {
            Vector3 rayOrigin = _targetCamera.transform.position;
            Vector3 rayDirection = _targetCamera.transform.forward;
            RaycastHit hit;

            if (Physics.Raycast(rayOrigin, rayDirection, out hit, _hammerRange, _targetMask))
            {
                var upgradable = hit.transform.GetComponent<IHammerInteractable>();
                if (upgradable == null)
                {
                    buildingUpgradeView.DisplayButtons(false, false, false, false);
                    return null;
                }

                buildingUpgradeView.DisplayButtons(upgradable.CanBeUpgraded(), upgradable.CanBeDestroyed(),
                    upgradable.CanBeRepaired(), upgradable.CanBePickUp(), upgradable.GetNeededCellsForUpgrade(),
                    upgradable.GetLevel());
                return upgradable;
            }

            buildingUpgradeView.DisplayButtons(false, false, false, false);
            return null;
        }

        public void UpgradeTo(int lvl)
        {
            if (_hammerInteractable == null) return;
            _hammerInteractable.UpgradeTo(lvl);
        }

        public void Repair()
        {
            if (_hammerInteractable == null) return;
            _hammerInteractable.Repair();
        }

        public void Destroy()
        {
            if (_hammerInteractable == null) return;
            _hammerInteractable.Destroy();
        }

        public void PickUp()
        {
            if (_hammerInteractable == null) return;
            _hammerInteractable.PickUp();
        }
    }
}