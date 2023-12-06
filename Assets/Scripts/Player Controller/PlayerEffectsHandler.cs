using System.Collections;
using System.Linq;
using Alerts_System.Alerts;
using Crafting_System.WorkBench;
using Tool_Clipboard;
using UnityEngine;
using Web.User;

public class PlayerEffectsHandler : MonoBehaviour
{
    private Coroutine _checkClipBoardCoroutine;
    
    private void HandleComfort(Collider other, bool isEntering)
    {
        if (!other.CompareTag("Comfort")) return;
        AlertsDisplayer.Singleton.DisplayComfortAlert(isEntering);
    }

    private void HandleWorkBenchAlert(Collider other, bool isEntering)
    {
        if (!other.CompareTag("WorkBench")) return;
        if (!isEntering)
        {
            AlertsDisplayer.Singleton.DisplayWorkBenchAlert(0, false);
            return;
        }

        var workBench = other.GetComponent<WorkBenchZone>();
        if (workBench == null) return;
        AlertsDisplayer.Singleton.DisplayWorkBenchAlert(workBench.TargetWorkBench.Level, true);
    }

    private IEnumerator HandleClipBoardAlertCoroutine(ShelfZoneHandler zone)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            var clipBoard = zone.ToolClipboard;

            var authorizedList = clipBoard.AuthorizedIds.AuthorizedIds.ToList();
            if (authorizedList.Contains(UserDataHandler.singleton.UserData.Id))
            {
                AlertsDisplayer.Singleton.DisplayBuildingUnblockedAlert(true);
                AlertsDisplayer.Singleton.DisplayBuildingBlockedAlert(false);
            }
            else
            {
                AlertsDisplayer.Singleton.DisplayBuildingUnblockedAlert(false);
                AlertsDisplayer.Singleton.DisplayBuildingBlockedAlert(true);
            }
        }
    }

    private void HandleClipBoardAlert(Collider other, bool isEntering)
    {
        if (!other.CompareTag("ShelfZone")) return;
        if (!isEntering)
        { 
            StopCoroutine(_checkClipBoardCoroutine);
            AlertsDisplayer.Singleton.DisplayBuildingBlockedAlert(false);
            AlertsDisplayer.Singleton.DisplayBuildingUnblockedAlert(false);
            return;
        }

        var shelfZone = other.GetComponent<ShelfZoneHandler>();
        if (shelfZone == null) return;

       _checkClipBoardCoroutine = StartCoroutine(HandleClipBoardAlertCoroutine(shelfZone));
    }

    private void OnTriggerEnter(Collider other)
    {
        HandleComfort(other, true);
        HandleWorkBenchAlert(other, true);
        HandleClipBoardAlert(other, true);
    }

    private void OnTriggerExit(Collider other)
    {
        HandleComfort(other, false);
        HandleWorkBenchAlert(other, false);
        HandleClipBoardAlert(other, false);
    }
}