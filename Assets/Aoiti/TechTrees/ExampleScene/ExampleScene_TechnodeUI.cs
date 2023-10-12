using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleScene_TechnodeUI : MonoBehaviour
{
    public RectTransform rectTransform;
    public RectTransform incomingNode;
    public RectTransform outgoingNode;
    public GameObject checkImage;
    public Button researchButton;
    public Scrollbar progressBar;
    public LineRenderer lineRenderer;
    public Text techname;
    private void Awake()
    {
        rectTransform = transform as RectTransform;
    }


}
