using System.Collections;
using System.Linq;
using AlertsSystem;
using Cloud.DataBaseSystem.UserData;
using Crafting_System.WorkBench;
using Tool_Clipboard;
using UnityEngine;

namespace Player_Controller
{
    public class PlayerEffectsHandler : MonoBehaviour
    {
        public bool CanInteract { private get; set; }
        
        private Coroutine _checkClipBoardCoroutine;

        private void HandleComfort(Collider other, bool isEntering)
        {
            if (!other.CompareTag("Comfort")) return;
            AlertEventsContainer.OnComfortAlert?.Invoke(isEntering);
        }

        private void HandleWorkBenchAlert(Collider other, bool isEntering)
        {
            if (!other.CompareTag("WorkBench")) return;
            if (!isEntering)
            {
                AlertEventsContainer.OnWorkBenchAlert?.Invoke(0, false);
                return;
            }

            var workBench = other.GetComponent<WorkBenchZone>();
            if (workBench == null) return;
            AlertEventsContainer.OnWorkBenchAlert?.Invoke(workBench.Level, true);
        }

        private IEnumerator HandleClipBoardAlertCoroutine(ShelfZoneHandler zone)
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                var clipBoard = zone.ToolClipboard;

                var authorizedList = clipBoard.AuthorizedIds.AuthorizedIds.ToList();
                if (authorizedList.Contains(UserDataHandler.Singleton.UserData.Id))
                {
                    AlertEventsContainer.OnBuildingUnblockedAlert?.Invoke(true);
                    AlertEventsContainer.OnBuildingBlockedAlert?.Invoke(false);
                }
                else
                {
                    AlertEventsContainer.OnBuildingUnblockedAlert?.Invoke(false);
                    AlertEventsContainer.OnBuildingBlockedAlert?.Invoke(true);
                }

                AlertEventsContainer.OnBuildingDecayAlert?.Invoke(clipBoard.IsDecay());
            }
        }

        private void HandleClipBoardAlert(Collider other, bool isEntering)
        {
            if (!other.CompareTag("ShelfZone")) return;
            if (!isEntering)
            {
                if (_checkClipBoardCoroutine != null)
                    StopCoroutine(_checkClipBoardCoroutine);

                AlertEventsContainer.OnBuildingBlockedAlert?.Invoke(false);
                AlertEventsContainer.OnBuildingUnblockedAlert?.Invoke(false);
                AlertEventsContainer.OnBuildingDecayAlert?.Invoke(false);
                return;
            }

            var shelfZone = other.GetComponent<ShelfZoneHandler>();
            if (shelfZone == null) return;

            if (_checkClipBoardCoroutine != null)
                StopCoroutine(_checkClipBoardCoroutine);
            _checkClipBoardCoroutine = StartCoroutine(HandleClipBoardAlertCoroutine(shelfZone));
        }

        private void OnTriggerEnter(Collider other)
        {
            if(!CanInteract) return;
            HandleComfort(other, true);
            HandleWorkBenchAlert(other, true);
            HandleClipBoardAlert(other, true);
        }

        private void OnTriggerExit(Collider other)
        {
            if(!CanInteract) return;
            HandleComfort(other, false);
            HandleWorkBenchAlert(other, false);
            HandleClipBoardAlert(other, false);
        }
    }
}