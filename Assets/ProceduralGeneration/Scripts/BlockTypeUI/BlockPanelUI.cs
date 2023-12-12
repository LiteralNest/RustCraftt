using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace ProceduralGeneration.Scripts.BlockTypeUI
{
    public class BlockPanelUI : MonoBehaviour
    {
        [SerializeField] private BlockType[] _blockTypes;
        [SerializeField] private Button[] _blockButtons;

        private int selectedBlockIndex = 0;

        private void Start()
        {
            for (int i = 0; i < _blockButtons.Length; i++)
            {
                int index = i;
                _blockButtons[i].onClick.AddListener(() => OnBlockButtonClick(index));
            }
        }

        private void OnBlockButtonClick(int blockIndex)
        {
            _blockButtons[selectedBlockIndex].interactable = true;

            selectedBlockIndex = blockIndex;
            
            _blockButtons[selectedBlockIndex].interactable = false;
        }

        public BlockType GetSelectedBlockType()
        {
            return _blockTypes[selectedBlockIndex];
        }
    }
}