using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatsDisplayer : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private Image _hpFill;
    [SerializeField] private TMP_Text _foodText;
    [SerializeField] private Image _foodFill;
    [SerializeField] private TMP_Text _waterText;
    [SerializeField] private Image _waterFill;

    public void DisplayHp(int hp)
    {
        _hpText.text = hp.ToString();
        _hpFill.fillAmount = (float)hp / 100;
    }
    
    public void DisplayFood(int food)
    {
        _foodText.text = food.ToString();
        _foodFill.fillAmount = (float)food / 100;
    }
    
    public void DisplayWater(int water)
    {
        _waterText.text = water.ToString();
        _waterFill.fillAmount = (float)water / 100;
    }
}
