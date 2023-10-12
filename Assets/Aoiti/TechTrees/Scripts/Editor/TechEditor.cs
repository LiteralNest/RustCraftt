using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Aoiti.Techtrees;
using System.IO;

[CustomEditor(typeof(Tech))]
public class TechEditor : Editor
{
    bool namingFailed = false;

    

    public override void OnInspectorGUI()
    {
        
        Tech Target= target as Tech;
        if (namingFailed) GUI.color = Color.red;

        //string assetPath = AssetDatabase.GetAssetPath(Target);
        string _name = EditorGUILayout.TextField("Name", Target.name);

        Target.name = _name;

        //if (Target.name != _name)
        //{
        //    Target.name = _name;
        //    if (AssetDatabase.RenameAsset(assetPath, Target.name.Trim()) != "")
        //        namingFailed = true;
        //    else namingFailed = false;
        //    return;

        //}
        //Target.name = Path.GetFileNameWithoutExtension(assetPath);



        base.OnInspectorGUI();

    }
}
