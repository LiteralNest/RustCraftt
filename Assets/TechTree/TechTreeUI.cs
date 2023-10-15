using System.Collections.Generic;
using Aoiti.Techtrees;
using UnityEngine;
using UnityEngine.UI;

public class TechTreeUI : MonoBehaviour
{
    public Dictionary<TechNode, TechNodeUI> NodeGameObjects;
    [SerializeField] private Techtree _techTree;
    [SerializeField] private GameObject _nodePrefab;
    [SerializeField] private GameObject _edgePrefab;
    [SerializeField] private Transform _treeContainer;
    [SerializeField] private Vector2 _offset;

    public void Start()
    {
        // Initialize the dictionary to store node game objects.
        NodeGameObjects = new Dictionary<TechNode, TechNodeUI>();
        
        foreach (var techNode in _techTree.nodes)
        {
            // Create a new node game object based on the prefab.
            var nodeGo = Instantiate(_nodePrefab, _treeContainer);
            var uiComponent = nodeGo.GetComponent<TechNodeUI>();
            NodeGameObjects.Add(techNode, uiComponent);

            // Position the UI element according to the node's UI position and offset.
            uiComponent.transform.position = new Vector2(techNode.UIposition.x + _offset.x, -techNode.UIposition.y + _offset.y);

            // Set progress bar size based on research progress.
            uiComponent.progressBar.size = (float)techNode.researchInvested / techNode.researchCost;

            // Set the text displaying the tech name.
            uiComponent.techname.text = techNode.tech.name;
            
            //Set the image displaying the tech image
            uiComponent.techImage.sprite = Sprite.Create(techNode.tech.image, new Rect(0, 0, techNode.tech.image.width, techNode.tech.image.height), new Vector2(0.5f, 0.5f));
        

            // Add a listener to the research button to trigger research in the TechTree.
            uiComponent.researchButton.onClick.AddListener(() => 
            {
                _techTree.Research(techNode, 100);
            });
        }

        // Draw connections between nodes.
        DrawLines();
    }

    // Method to restart the tree with a new Techtree.
    public void RestartTree(Techtree tree)
    {
        _techTree = tree;

        if (NodeGameObjects != null)
        {
            // Destroy existing node game objects.
            foreach (var nodeUI in NodeGameObjects.Values)
            {
                Destroy(nodeUI.gameObject);
            }

            // Restart the tree creation process.
            Start();
        }
    }

    private void OnGUI()
    {
        UpdateTree();
    }

    // Update the UI elements based on research progress.
    private void UpdateTree()
    {
        foreach (var techNode in _techTree.nodes)
        {
            // Update progress bar size.
            NodeGameObjects[techNode].GetComponentInChildren<Scrollbar>().size = (float)techNode.researchInvested / (float)techNode.researchCost;

            if (_techTree.RequirementsMet(techNode))
            {
                if (!techNode.isResearched)
                    NodeGameObjects[techNode].researchButton.interactable = true;
                else
                {
                    NodeGameObjects[techNode].researchButton.interactable = false;
                    NodeGameObjects[techNode].checkImage.SetActive(true);
                }
            }
        }
    }

    // Draw lines connecting nodes based on their requirements.
    private void DrawLines()
    {
        foreach (var techNode in _techTree.nodes)
        {
            // Update progress bar size.
            NodeGameObjects[techNode].GetComponentInChildren<Scrollbar>().size = (float)techNode.researchInvested / (float)techNode.researchCost;
            bool incompleteReq = false;

            if (techNode.requirements.Count > 0)
            {
                foreach (var tech in techNode.requirements)
                {
                    // Check if the requirement is met.
                    if (!_techTree.FindTechNode(tech).isResearched) incompleteReq = true;

                    // Create an edge between nodes.
                    var edge = Instantiate(_edgePrefab, NodeGameObjects[techNode].transform);
                    var edgeController = edge.GetComponent<ExampleScene_TechEdgeUI>();

                    // Set start and end positions for the edge.
                    edgeController.start = (NodeGameObjects[techNode].transform.position + NodeGameObjects[techNode].incomingNode.transform.localPosition);
                    edgeController.end = (NodeGameObjects[_techTree.FindTechNode(tech)].transform.position + NodeGameObjects[techNode].outgoingNode.transform.localPosition);
                }
            } 

            if (!incompleteReq)
            {
                if (!techNode.isResearched)
                    NodeGameObjects[techNode].researchButton.interactable = true;
                else
                {
                    NodeGameObjects[techNode].researchButton.interactable = false;
                    NodeGameObjects[techNode].checkImage.SetActive(true);
                }
            }
        }
    }
}
