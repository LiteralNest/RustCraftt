using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDataDisplayer : MonoBehaviour
{
   [Header("UI")] [SerializeField] private GameObject _displayingPanel;
   [SerializeField] private Image _fillingImage;
   [SerializeField] private TMP_Text _fillingText;

   public void DisableBuildingPanel()
      => _displayingPanel.SetActive(false);

   public void DisplayBuildingData(BuildingBlock block)
   {
      _displayingPanel.SetActive(true);
      
      int hp = block.Hp;
      int maxHp = block.CurrentBlock.Hp;
      
      _fillingImage.fillAmount = (float)hp / maxHp;
      _fillingText.text = hp + "/" + maxHp;
   }   
}