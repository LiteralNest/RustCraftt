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
            => DisplayHp(damagable.GetMaxHp(), damagable.GetHp());

        public void DisplayBuildingHp(IBuildingDamagable buildingDamagable)
        {
            _displayingPanel.SetActive(true);

            int hp = buildingDamagable.GetHp();
            int maxHp = buildingDamagable.GetMaxHp();

            _fillingImage.fillAmount = (float)hp / maxHp;
            _fillingText.text = hp + "/" + maxHp;
        }

        public void DisplayHp(int maxHp, int currentHp)
        {
            _displayingPanel.SetActive(true);
            _fillingImage.fillAmount = (float)currentHp / maxHp;
            _fillingText.text = currentHp + "/" + maxHp;
        }
    }
}