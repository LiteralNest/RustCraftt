using UnityEngine;
using UnityEngine.UI;

public class TechNodeUI : MonoBehaviour
{
    public RectTransform rectTransform;
    public RectTransform incomingNode;
    public RectTransform outgoingNode;
    public Image techImage;
    public GameObject checkImage;
    public Button researchButton;
    public Scrollbar progressBar;
    public LineRenderer lineRenderer;
    public Text techname;
    private void Awake()
    {
        rectTransform = transform as RectTransform;
    }
    
    public void SetTechImage(Sprite sprite)
    {
        techImage.sprite = sprite;
    }
}