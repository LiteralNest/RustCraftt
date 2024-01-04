using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory_System.Slots_Displayer.Tool_CLipBoard
{
   public class ToolClipBoardDisplayingCell : MonoBehaviour
   {
      [SerializeField] private Image _icon;
      [SerializeField] private TMP_Text _countText;

      public void Init(Sprite icon, int count)
      {
         _icon.sprite = icon;
         if (count > 1000)
         {
            _countText.text = "x" + (int)(count / 1000) + "," + count % 1000;
            return;
         }
         _countText.text =  "x" + count;
      }
   }
}
