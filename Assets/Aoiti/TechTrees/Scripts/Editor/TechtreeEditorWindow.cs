using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Aoiti.Techtrees;
using System;
using System.IO;
using System.Reflection;
[CustomEditor(typeof(Techtree))]
public class TechtreeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Techtree Editor Window...",GUILayout.Height(30)))
        {
            TechtreeEditorWindow.Init(target as Techtree);
        }
        base.OnInspectorGUI();
    }
}

public class TechtreeEditorWindow: EditorWindow
{
    Techtree targetTree;
    int instanceID;

    // positioning
    Vector2 nodeSize = new Vector2(150f, 70f);
    Vector2 incomingEdgeVec = new Vector2(150f, 10f);
    Vector2 outgoingEdgeVec = new Vector2(0f, 10f);
    Vector2 upArrowVec = new Vector2(-10f, -10f);
    Vector2 downArrowVec = new Vector2(-10f, 10f);
    Vector2 nextLineVec = new Vector2(0f, 20f);
    Vector2 indentVec = new Vector2(148f, 0f);
    Vector2 nodeContentSize = new Vector2(40f, 20f);
    Vector2 nodeLabelSize = new Vector2(150f, 20f);
    
    // scrolling and moving
    Vector2 mouseSelectionOffset;
    Vector2 scrollPosition = Vector2.zero; // Move everything by scrollPosition
    Vector2 scrollStartPos;

    TechNode activeNode; //moved node stored here
    TechNode selectedNode; //selected node stored here

    Vector2 horizontalLimits=new Vector2(0f, 720f);
    Vector2 verticalLimits=new Vector2(0f, 720f);

    [SerializeField]
    bool useGrid=true;
    [SerializeField]
    int gridSize = 12;
    [SerializeField]
    bool showEvents;
    public static void Init(Techtree techTree=null)
    {
        var inspWndType = typeof(SceneView);
        var window = GetWindow<TechtreeEditorWindow>(inspWndType);
        window.Show(true);
        if (techTree!=null)
        {
            window.instanceID = techTree.GetInstanceID();
        }
    }
    //GUIStyle toolbarStyle; //left for improvement

    [MenuItem("Tools/Techtrees/Techtree Editor...")]
    public static void Init()
    {
        var inspWndType = typeof(SceneView);
        var window = GetWindow<TechtreeEditorWindow>(inspWndType);
        window.Show(true);
    }



    public void OnGUI()
    {
        
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox,GUILayout.Height(26));
        targetTree = EditorUtility.InstanceIDToObject(instanceID) as Techtree;
        var newTarget = EditorGUILayout.ObjectField("Target Techtree:", targetTree, typeof(Techtree), allowSceneObjects: true, GUILayout.Width(400)) as Techtree;
        if (newTarget != targetTree)
        {
            selectedNode = null;
            activeNode = null;
            targetTree = newTarget;
        }
        if (targetTree!=null)
            instanceID = targetTree.GetInstanceID();
        if (targetTree==null || PrefabUtility.IsPartOfPrefabAsset(targetTree))
        {
            EditorGUILayout.LabelField("Cannot edit prefabs! Please select Techtree from the scene or create a Techtree from Tools/Techtrees/ menu.");
            EditorGUILayout.EndHorizontal();
            return;

        }
        
        useGrid = EditorGUILayout.Toggle("Snap to grid",useGrid, GUILayout.MaxWidth(180));
        showEvents = EditorGUILayout.Toggle("Show events", showEvents, GUILayout.MaxWidth(180));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox, GUILayout.Height(26));
        // Shows selected node tech and gives option to delete node
        if ( GUILayout.Button("Create a tech", GUILayout.MaxWidth(160)))
        {
            //var newTech = ScriptableObject.CreateInstance<Tech>();
            GameObject newTechGO = new GameObject("Tech");
            newTechGO.transform.parent = targetTree.transform;
            Tech newTech = newTechGO.AddComponent<Tech>();
            newTech.name = "untitled tech";

            //AssetDatabase.CreateAsset(newTech, GetDirectoryPath(AssetDatabase.GetAssetPath(selectedNode.tech)) + "/" + newTech.name + ".asset");
            //AssetDatabase.SaveAssets();
            
            if (targetTree.AddNode(newTech, ((selectedNode!=null)? selectedNode.UIposition: scrollPosition) + Vector2.one * 50))
            {
                Selection.activeObject = newTech;
                selectedNode = targetTree.FindTechNode(newTech);
            }
            //if (PrefabUtility.IsPartOfPrefabAsset(targetTree.gameObject)) 

            //PrefabUtility.ApplyPrefabInstance(targetTree.gameObject,InteractionMode.UserAction);

        }
        if (selectedNode == null || selectedNode.tech == null)
        {
            EditorGUILayout.LabelField("Selected tech: none");
        }
        else
        {
            EditorGUILayout.LabelField("Selected tech: " + selectedNode.tech.name, GUILayout.MaxWidth(240));

            
            if (selectedNode != null && GUILayout.Button("Remove tech from tree", GUILayout.MaxWidth(160)))
            {
                targetTree.DeleteNode(selectedNode.tech);
                if (activeNode == selectedNode) activeNode = null;
                selectedNode = null;
            }
            if (selectedNode != null)
            {
                if( !PrefabUtility.IsPartOfPrefabInstance(selectedNode.tech.gameObject) )
                {
                    if (GUILayout.Button("Delete tech", GUILayout.MaxWidth(160)))
                    {
                        //AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(selectedNode.tech));
                        DestroyImmediate(selectedNode.tech.gameObject,true);
                        selectedNode = null;
                    }
                } else
                {
                    GUILayout.Label("Cannot delete part of a prefab. Unpack before deleting.");
                    //connected to prefab
                }
            }
        }
        EditorGUILayout.EndHorizontal();

        if (targetTree == null) return; //Do not do anything until

        //Mouse events
        Event currentEvent = Event.current;
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        EventType UIEvent = currentEvent.GetTypeForControl(controlID);

        //node styles
        GUIStyle nodeStyle = new GUIStyle(EditorStyles.helpBox);
        GUIStyle selectedStyle = new GUIStyle(EditorStyles.helpBox);
        selectedStyle.fontStyle = FontStyle.BoldAndItalic;

        #region Draw nodes
        //The techtree view
        EditorGUILayout.BeginScrollView(Vector2.zero); 
        List<int> nodesMarkedForDeletion = new List<int>();
        if (targetTree.nodes == null) targetTree.nodes = new List<TechNode>();
        for (int nodeIdx = 0; nodeIdx < targetTree.nodes.Count; nodeIdx++)
        {
            horizontalLimits.x = Mathf.Min(horizontalLimits.x, targetTree.nodes[nodeIdx].UIposition.x - 100f);
            horizontalLimits.y = Mathf.Max(horizontalLimits.y, targetTree.nodes[nodeIdx].UIposition.x + 100f);
            verticalLimits.x = Mathf.Min(verticalLimits.x, targetTree.nodes[nodeIdx].UIposition.y - 100f);
            verticalLimits.y = Mathf.Max(verticalLimits.y, targetTree.nodes[nodeIdx].UIposition.y + 100f);
            if (targetTree.nodes[nodeIdx].tech == null) {
                nodesMarkedForDeletion.Add(nodeIdx);
                if (selectedNode == targetTree.nodes[nodeIdx])
                    selectedNode = null;
                continue;
            }
            //Draw node
            Rect nodeRect = new Rect(targetTree.nodes[nodeIdx].UIposition - scrollPosition, nodeSize);
            
            EditorGUI.BeginFoldoutHeaderGroup(nodeRect, true, targetTree.nodes[nodeIdx].tech.name, (selectedNode == targetTree.nodes[nodeIdx] ? selectedStyle : nodeStyle));
            EditorGUI.LabelField(new Rect(targetTree.nodes[nodeIdx].UIposition - scrollPosition + nextLineVec, nodeLabelSize), "Reseach cost");
            targetTree.nodes[nodeIdx].researchCost = EditorGUI.IntField(new Rect(targetTree.nodes[nodeIdx].UIposition - scrollPosition + nextLineVec + indentVec, nodeContentSize), targetTree.nodes[nodeIdx].researchCost);
            EditorGUI.LabelField(new Rect(targetTree.nodes[nodeIdx].UIposition - scrollPosition + nextLineVec * 2, nodeLabelSize), "Invested");
            targetTree.nodes[nodeIdx].researchInvested = EditorGUI.IntField(new Rect(targetTree.nodes[nodeIdx].UIposition - scrollPosition + nextLineVec * 2 + indentVec, nodeContentSize), targetTree.nodes[nodeIdx].researchInvested);

            if (showEvents)
            {
                var serializedProperty = new SerializedObject(targetTree.nodes[nodeIdx].tech).FindProperty(nameof(Tech.OnResearchComplete));
                //var serializedProperty = new SerializedObject(targetTree).FindProperty("nodes").GetArrayElementAtIndex(nodeIdx).FindPropertyRelative(nameof(TechNode.OnResearchComplete));
                EditorGUI.PropertyField(new Rect(targetTree.nodes[nodeIdx].UIposition - scrollPosition + nextLineVec * 3, new Vector2(200f, 200f)),serializedProperty,true);
                serializedProperty.serializedObject.ApplyModifiedProperties();

            }
            EditorGUI.EndFoldoutHeaderGroup();

            //Draw connections
            foreach (Tech req in targetTree.nodes[nodeIdx].requirements)
            {
                int reqIdx = targetTree.FindTechIndex(req);
                if (reqIdx != -1)
                {
                    //Draw connecting curve
                    Handles.DrawBezier(targetTree.nodes[nodeIdx].UIposition - scrollPosition + outgoingEdgeVec,
                        targetTree.nodes[reqIdx].UIposition - scrollPosition + incomingEdgeVec,
                        targetTree.nodes[nodeIdx].UIposition - scrollPosition + outgoingEdgeVec + Vector2.left * 100,
                        targetTree.nodes[reqIdx].UIposition - scrollPosition + incomingEdgeVec + Vector2.right * 100,
                        Color.white
                        , null
                        , 3f);
                    //Draw arrow
                    Handles.DrawLine(targetTree.nodes[nodeIdx].UIposition - scrollPosition + outgoingEdgeVec, targetTree.nodes[nodeIdx].UIposition - scrollPosition + outgoingEdgeVec + upArrowVec);
                    Handles.DrawLine(targetTree.nodes[nodeIdx].UIposition - scrollPosition + outgoingEdgeVec, targetTree.nodes[nodeIdx].UIposition - scrollPosition + outgoingEdgeVec + downArrowVec);
                }
                else Debug.LogWarning("missing tech " + req.name);
            }
            Repaint(); //reqired to avoid stutter 
            //mouse events
            if (nodeRect.Contains(currentEvent.mousePosition)) //if the cursor is on the node
            {
                if (UIEvent == EventType.MouseDown) //if the a mouse button is pressed
                {
                    // Set activeNode
                    if (currentEvent.button == 0) //if left mouse button is pressed
                    {
                        activeNode = targetTree.nodes[nodeIdx];
                        mouseSelectionOffset = activeNode.UIposition - currentEvent.mousePosition; //offset from the corner of the node to mouse position
                        selectedNode = targetTree.nodes[nodeIdx];
                        Repaint(); //repaint to see the style change momentarily
                    }
                }
                else
                // Create/Destroy connections
                if (UIEvent == EventType.MouseUp) //if the mouse button is released
                {
                    //if right button is released and selectionNode is not empty
                    if (currentEvent.button == 1 && selectedNode != null && selectedNode != targetTree.nodes[nodeIdx])
                    {
                        //remove any connection between the selectedNode and hovered node if exists
                        if (targetTree.nodes[nodeIdx].requirements.Contains(selectedNode.tech))
                            targetTree.nodes[nodeIdx].requirements.Remove(selectedNode.tech);
                        else if (selectedNode.requirements.Contains(targetTree.nodes[nodeIdx].tech))
                            selectedNode.requirements.Remove(targetTree.nodes[nodeIdx].tech);
                        else
                        //if doesn't exists, and they are connectible then create a connection.
                        if (targetTree.IsConnectible(targetTree.nodes.IndexOf(selectedNode), nodeIdx))
                        {
                            targetTree.nodes[nodeIdx].requirements.Add(selectedNode.tech);
                            //craeting connection may annul other requirement connections. So lets check all connections.
                            for (int k = 0; k < targetTree.nodes.Count; k++)
                                targetTree.CorrectRequirementCascades(k);
                        }
                    }
                }
            }
        }
        #endregion

        #region Delete nodes marked for deletion
        nodesMarkedForDeletion.Reverse();
        foreach (int idx in nodesMarkedForDeletion )
        {
            targetTree.DeleteNode(idx);
            Repaint();
        }
        #endregion


        #region Scroll in the TechTree view
        if (currentEvent.button == 2) //if the middle mouse button is pressed,held or released
        {
            if (currentEvent.type == EventType.MouseDown) //if the mouse button is down
                scrollStartPos = (currentEvent.mousePosition + scrollPosition); //store where the coordinate
            else if (currentEvent.type == EventType.MouseDrag) // if the mouse button is held
            {
                scrollPosition = -(currentEvent.mousePosition - scrollStartPos); //recalculate scrollPosition. This moves everything
                Repaint(); //repaint the GUI
            }
        }
        #endregion

        #region Draw guiding connection when right mouse button is held
        if (selectedNode != null && currentEvent.button == 1) //if rightmouse button is used and selection is not empty.
        {
            //Draw connection guide between selected node and the mouse position.
            Handles.DrawBezier(currentEvent.mousePosition,
            selectedNode.UIposition - scrollPosition + incomingEdgeVec,
            currentEvent.mousePosition + Vector2.left * 100,
            selectedNode.UIposition - scrollPosition + incomingEdgeVec + Vector2.right * 100,
            Color.white
            , null
            , 1.5f);
            Repaint();
        }
        #endregion

        #region Move nodes with left mouse button
        if (UIEvent == EventType.MouseDown && selectedNode!=null && selectedNode.tech != null)
        {

            Selection.instanceIDs = new int[1]
            {
                selectedNode.tech.GetInstanceID()
            };
        }
        if (UIEvent == EventType.MouseUp) //if dropped
        {
            activeNode = null;
        }
        else if (UIEvent == EventType.MouseDrag) //while dragged
        {
            if (activeNode != null)
            {
                activeNode.UIposition = currentEvent.mousePosition + mouseSelectionOffset;
                if (useGrid)
                    activeNode.UIposition = new Vector2(Mathf.Round(activeNode.UIposition.x/gridSize)*gridSize, Mathf.Round(activeNode.UIposition.y / gridSize) *gridSize);
            }
            
        }
        #endregion

        #region Import new Tech
        if (currentEvent.type == EventType.DragUpdated)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }
        else if (currentEvent.type == EventType.DragPerform)
        {
            for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
            {
                if (DragAndDrop.objectReferences[i] is Tech)
                    targetTree.AddNode(DragAndDrop.objectReferences[i] as Tech, currentEvent.mousePosition + scrollPosition);
                else if (DragAndDrop.objectReferences[i] is GameObject)
                {
                    var newTech = (DragAndDrop.objectReferences[i] as GameObject).GetComponent<Tech>();
                    if (newTech!=null)
                        targetTree.AddNode(newTech, currentEvent.mousePosition + scrollPosition);
                    #region left for improvement
                    //else
                    //{
                    //    var newTechtree= (DragAndDrop.objectReferences[i] as GameObject).GetComponent<Techtree>();
                    //    if (newTechtree!=null)
                    //    {
                    //        if (newTechtree != targetTree)
                    //        {
                    //            selectedNode = null;
                    //            activeNode = null;
                    //            targetTree = newTechtree;
                    //        }
                    //        if (targetTree != null)
                    //            instanceID = targetTree.GetInstanceID();
                    //        if (targetTree == null || PrefabUtility.IsPartOfPrefabAsset(targetTree))
                    //        {
                    //            EditorGUILayout.EndHorizontal();
                    //            EditorGUILayout.EndScrollView();
                    //            return;

                    //        }
                    //    }
                    //}
                    #endregion
                EditorUtility.SetDirty(targetTree); // makes sure the changes are presistent.
                    Repaint();
                }
            }
        }
        #endregion

        EditorGUILayout.EndScrollView();
        Rect scrollArea = GUILayoutUtility.GetLastRect();

        # region scrollview adjustments
        scrollPosition.x = GUILayout.HorizontalScrollbar(scrollPosition.x, 20f, horizontalLimits.x, horizontalLimits.y);
        scrollPosition.y = GUI.VerticalScrollbar(new Rect(0, scrollArea.yMin, 20, scrollArea.height), scrollPosition.y, 20f, verticalLimits.x, verticalLimits.y);
        #endregion
        EditorUtility.SetDirty(targetTree); // makes sure the changes are presistent.
    }

    public static string GetDirectoryPath(string filename)
    {
        return filename.Substring(0, filename.LastIndexOfAny(new char[] { '\\', '/' }));
    }
    
    public static string CurrentProjectFolderPath
    {
        get
        {
            Type projectWindowUtilType = typeof(ProjectWindowUtil);
            MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
            object obj = getActiveFolderPath.Invoke(null, new object[0]);
            return obj.ToString();
        }
    }

    [MenuItem("Tools/Techtrees/Create Techtree")]
    private static void CreatePrefabInProject()
    {
        //string prefabName = "Techtree";
        //string targetPath = $"{CurrentProjectFolderPath}/{prefabName}.prefab";
        //int i = 0;
        //while (File.Exists(targetPath))
        //{
        //    i++;
        //    targetPath= $"{CurrentProjectFolderPath}/{prefabName+ " " + i.ToString()}.prefab";
        //}
        var source = new GameObject("Techtree");
        source.AddComponent<Techtree>();
        //PrefabUtility.SaveAsPrefabAsset(source, targetPath);
    }


}
