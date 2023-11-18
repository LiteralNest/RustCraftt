using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Technology : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] public int Cost;
    [SerializeField] private Image _image;

    [SerializeField] private Technology[] _unlockedTech;
    [SerializeField] public bool isResearched;
    [SerializeField] private PlayerScrapTest _scrapTest; //TEST

    public bool IsResearched => isResearched;
    public string TechName { get; private set; }
    public string TechDescription { get; private set; }
    public Sprite TechImage { get; private set; }

    private int _currentResourceAmount;
    
    private void Awake()
    {
        _currentResourceAmount = _scrapTest.Scrap;

        TechName = _item.Name;
        TechDescription = _item.Description;
        TechImage = _item.Icon;
        _image.sprite = TechImage;
    }
    
    public void Research()
    {
        if (!isResearched)
        {
            if (CanResearch() && _currentResourceAmount >= Cost)
            {
                _currentResourceAmount -= Cost;
                isResearched = true;
                Debug.Log("Reserched");
            }
        }
    }

    public bool CanResearch()
    {
        foreach (var tech in _unlockedTech)
        {
            if (!tech.IsResearched)
            {
                return false;
            }
        }
     
        return true;
    }
}
