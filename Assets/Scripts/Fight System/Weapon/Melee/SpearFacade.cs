using UnityEngine;

public class SpearFacade : MonoBehaviour
{
    [SerializeField] private MeleeShootingWeapon _trowingSpear;

    private void OnGUI()
    {
        var buttonWidth = 200f;
        var buttonHeight = 60f;
        var centerX = (Screen.width - buttonWidth) / 2;
        var centerY = (Screen.height - buttonHeight) / 2;

        if (_trowingSpear.IsInThrowingPosition)
        {
            if (GUI.Button(new Rect(centerX, centerY, buttonWidth, buttonHeight), "Throw"))
            {
                _trowingSpear.ThrowSpear();
            }
        }
        else
        {
            if (GUI.Button(new Rect(centerX, centerY, buttonWidth, buttonHeight), "Set Throwing Position"))
            {
                _trowingSpear.SetThrowingPosition();
            }
        }
    }
}