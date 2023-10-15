using Aoiti.Techtrees;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleScene_TechTreePanelController : MonoBehaviour
{
    public Techtree techtree;
    public GameObject nodePrefab;
    public GameObject edgePrefab;
    public Dictionary<TechNode, ExampleScene_TechnodeUI> nodeGameObjects;
    public Transform treeContainer;
    public Vector2 offset;
    // Start is called before the first frame update
    void Start()
    
    {
        // techtree = ScriptableObject.Instantiate<Techtree>(techtree); 
        nodeGameObjects = new Dictionary<TechNode, ExampleScene_TechnodeUI>();
        
        foreach (TechNode tn in techtree.nodes)
        {
            nodeGameObjects.Add(tn, Instantiate<GameObject>(nodePrefab,treeContainer).GetComponent<ExampleScene_TechnodeUI>());
            nodeGameObjects[tn].transform.position = new Vector2(tn.UIposition.x+offset.x, -tn.UIposition.y+offset.y);
            nodeGameObjects[tn].progressBar.size = (float)tn.researchInvested / tn.researchCost;
            nodeGameObjects[tn].techname.text = tn.tech.name;
            nodeGameObjects[tn].researchButton.onClick.AddListener(() => 
            {
                techtree.Research(tn, 25);
            }) ;
        }
        DrawLines();
    }

    public void RestartTree(Techtree tree)
    {
        techtree = tree;
        if (nodeGameObjects!=null)
        {
            foreach (ExampleScene_TechnodeUI tnui in nodeGameObjects.Values)
            {
                Destroy(tnui.gameObject);
            }
            Start();
        }
        
    }

    private void OnGUI()
    {
        UpdateTree();
    }


    public void UpdateTree()
    {
        Camera mainCam = Camera.main;

        foreach (TechNode tn in techtree.nodes)
        {

            nodeGameObjects[tn].GetComponentInChildren<Scrollbar>().size = (float)tn.researchInvested / (float)tn.researchCost;

            if (techtree.RequirementsMet(tn))
            {
                if (!tn.isResearched)
                    nodeGameObjects[tn].researchButton.interactable = true;
                else
                {
                    nodeGameObjects[tn].researchButton.interactable = false;
                    nodeGameObjects[tn].checkImage.SetActive(true);
                }

            }

        }
    }
    


    public void DrawLines()
    {
        Camera mainCam = Camera.main;

        foreach (TechNode tn in techtree.nodes)
        {

            nodeGameObjects[tn].GetComponentInChildren<Scrollbar>().size= (float) tn.researchInvested / (float)tn.researchCost;
            bool incompleteReq = false;
            if (tn.requirements.Count>0)
            {
                foreach (Tech req in tn.requirements)
                {

                    if (!techtree.FindTechNode(req).isResearched) incompleteReq = true;
                    var edge =GameObject.Instantiate<GameObject>(edgePrefab, nodeGameObjects[tn].transform);
                    var edgeController= edge.GetComponent<ExampleScene_TechEdgeUI>();
                    edgeController.start = (nodeGameObjects[tn].transform.position + nodeGameObjects[tn].incomingNode.transform.localPosition);
                    edgeController.end = (nodeGameObjects[techtree.FindTechNode(req)].transform.position + nodeGameObjects[tn].outgoingNode.transform.localPosition);
                }

            } 
            if (!incompleteReq)
            {
                if (!tn.isResearched)
                    nodeGameObjects[tn].researchButton.interactable = true;
                else
                {
                    nodeGameObjects[tn].researchButton.interactable=false;
                    nodeGameObjects[tn].checkImage.SetActive(true);
                }
                    
            }

        }
    }

    
}