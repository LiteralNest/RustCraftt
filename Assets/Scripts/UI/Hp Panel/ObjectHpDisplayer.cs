using FightSystem.Damage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hp_Panel
{
   public class ObjectHpDisplayer : MonoBehaviour
   {
      [Header("UI")] [SerializeField] private GameObject _displayingPanel;
      [SerializeField] private Image _fillingImage;
      [SerializeField] private TMP_Text _fillingText;

      public void DisablePanel()
         => _displayingPanel.SetActive(false);

      public void DisplayObjectHp(IDamagable damagable)
      {
         _displayingPanel.SetActive(true);
      
         int hp = damagable.GetHp();
         int maxHp = damagable.GetMaxHp();
      
         _fillingImage.fillAmount = (float)hp / maxHp;
         _fillingText.text = hp + "/" + maxHp;
      }

      public void DisplayBuildingHp(IBuildingDamagable buildingDamagable)
      {
         _displayingPanel.SetActive(true);
      
         int hp = buildingDamagable.GetHp();
         int maxHp = buildingDamagable.GetMaxHp();
      
         _fillingImage.fillAmount = (float)hp / maxHp;
         _fillingText.text = hp + "/" + maxHp;
      }
   }
}