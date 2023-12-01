using Items_System.Items.Abstract;
using TMPro;
using UnityEngine;

public class CreatingQueueAlertDisplayer : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _titleText;
    [SerializeField] private TMP_Text _timeText;

    public void Init(Item item, int count, int time)
    {
        _titleText.text = item.Name + "(" + count + ")";
        _timeText.text = time + "s";
    }
}
