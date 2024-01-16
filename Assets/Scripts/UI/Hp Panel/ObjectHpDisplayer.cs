using FightSystem.Damage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectHpDisplayer : MonoBehaviour
{
   [Header("UI")] [SerializeField] private GameObject _displayingPanel;
   [SerializeField] private Image _fillingImage;
   [SerializeField] private TMP_Text _fillingText;

   public void DisableBuildingPanel()
      => _displayingPanel.SetActive(false);

   public void DisplayObjectHp(IDamagable damagable)
   {
      _displayingPanel.SetActive(true);
      
      int hp = damagable.GetHp();
      int maxHp = damagable.GetMaxHp();
      
      _fillingImage.fillAmount = (float)hp / maxHp;
      _fillingText.text = hp + "/" + maxHp;
   }   
}