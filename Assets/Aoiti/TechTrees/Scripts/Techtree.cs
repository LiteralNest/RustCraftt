using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Aoiti.Techtrees
{
    [System.Serializable]
    public class TechNode
    {
        public Tech tech;
        public List<Tech> requirements;
        public int researchCost;
        
        [SerializeField]private int _researchInvested;
        public int researchInvested
        {
            set
            {
                if (_researchInvested< researchCost && value>= researchCost) //first time research is complete
                {
                    //OnResearchComplete.Invoke();
                    tech.OnResearchComplete.Invoke();
                }
                _researchInvested = value;
            }
            get { return _researchInvested; }
        } 
        //[SerializeField] public UnityEvent OnResearchComplete;

        public Vector2 UIposition; // required for GUI
        public TechNode(Tech tech, List<Tech> reqs, int cost, Vector2 position)
        {
            this.tech = tech;
            this.requirements = reqs;
            this.researchCost = cost;
            this.researchInvested = 0;
            this.UIposition = position;
            //this.OnResearchComplete = new UnityEvent(); //moved to Tech
        }

        public bool isResearched { get => researchInvested >= researchCost; }
        
    }

    //[CreateAssetMenu(menuName = "Techtrees/new Techtree")] //only if Techtree is changed into ScriptableObject
    public class Techtree : MonoBehaviour
    {
        public Techtree(Techtree originalTemplateTree)
        {
            this.nodes = originalTemplateTree.nodes;
        }

        [SerializeField]
        public List<TechNode> nodes= new List<TechNode>();

        public IEnumerable<TechNode> IterateTechNodes()
        {
            foreach (TechNode tn in nodes)
            { yield return tn; }
        }


        public bool IsResearched(Tech tech)
        {
            return nodes[FindTechIndex(tech)].isResearched;
        }
        
        public bool Research(Tech tech, int amount, bool ignoreRequirements = false)
        {
            return Research(FindTechNode(tech), amount, ignoreRequirements);
        }
        public bool Research(int techNodeIdx, int amount, bool ignoreRequirements = false)
        {
            return Research(nodes[techNodeIdx], amount, ignoreRequirements);
        }
        public bool Research(TechNode techNode, int amount, bool ignoreRequirements= false)
        {
            if (ignoreRequirements || RequirementsMet(techNode))
            {
                techNode.researchInvested += amount;
                return techNode.isResearched;
            }
            return false;
        }

        public bool RequirementsMet(TechNode techNode)
        {
            bool hasIncompleteReq = false;
            if (techNode.requirements.Count > 0)
            {
                foreach (Tech req in techNode.requirements)
                {
                    if (!FindTechNode(req).isResearched) hasIncompleteReq = true;
                }
            }
            return !hasIncompleteReq;
        }
        public bool RequirementsMet(Tech tech)
        {
            TechNode techNode = FindTechNode(tech);
            return RequirementsMet(techNode);
        } 
        public bool RequirementsMet(int TechNodeIdx)
        {
            TechNode techNode = nodes[TechNodeIdx];
            return RequirementsMet(techNode);
        }

        public bool AddNode(Tech tech, Vector2 UIpos, int cost=100)
        {
            int tIdx = FindTechIndex(tech);
            if (tIdx==-1)
            {
                nodes.Add(new TechNode(tech, new List<Tech>(), cost, UIpos));

                return true;
            }else 
                return false;
        }

        public void DeleteNode(int nodeIdx)
        {
            Tech tech = nodes[nodeIdx].tech;
            nodes.RemoveAt(nodeIdx);
            foreach (TechNode tn in nodes)
            {
                if (tn.requirements.Contains(tech)) tn.requirements.Remove(tech);
            }
        }
        public void DeleteNode(Tech tech)
        {
            nodes.RemoveAt( FindTechIndex(tech));
            foreach( TechNode tn in nodes)
            {
                if (tn.requirements.Contains(tech)) tn.requirements.Remove(tech);
            }
        }

        public int FindTechIndex(Tech tech)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].tech == tech) return i;
            }
            return -1;
        }

        public TechNode FindTechNode(Tech tech)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (nodes[i].tech == tech) return nodes[i];
            }
            return null;
        }

        public bool DoesLeadsToInCascade(int query, int subject)
        {
            foreach (Tech t in nodes[query].requirements)
            {
                if (t == nodes[subject].tech) return true;
                if (DoesLeadsToInCascade(FindTechIndex(t), subject)) return true;
            }
            return false;
        }

        public bool DoesLeadsToInCascade(Tech query, Tech subject)
        {
            return DoesLeadsToInCascade(FindTechIndex(query), FindTechIndex(subject));
        }

        public bool IsConnectible(int incomingNodeIdx, int outgoingNodeIdx)
        {
            if (incomingNodeIdx == outgoingNodeIdx) return false;
            return !(DoesLeadsToInCascade(incomingNodeIdx, outgoingNodeIdx) || DoesLeadsToInCascade(outgoingNodeIdx, incomingNodeIdx));

        }


        public HashSet<Tech> GetAllPastRequirements(int nodeIdx, bool includeSelfRequirements=true)
        {
            HashSet<Tech> allRequirements = (includeSelfRequirements)? new HashSet<Tech>(nodes[nodeIdx].requirements): new HashSet<Tech>();
            foreach (Tech t in nodes[nodeIdx].requirements)
            {
                allRequirements.UnionWith(GetAllPastRequirements ( FindTechIndex(t) ) );
            }
            return allRequirements;
        }

        public void CorrectRequirementCascades(int idx)
        {
            HashSet<Tech> allConnectedThroughChildren = GetAllPastRequirements(idx,false);
            foreach (Tech t in allConnectedThroughChildren)
            {

                if (nodes[idx].requirements.Contains(t)) nodes[idx].requirements.Remove(t);
            }
        }

    }

    



}
