using UnityEngine;
using UnityEditor;

namespace FrustumCullingSpace
{
    [CustomEditor(typeof(FrustumCulling))]
    [CanEditMultipleObjects]

    public class FrustumCullingCustomInspector : Editor
    {
        SerializedProperty autoCatchCamera,
        mainCam,
        gameView,
        activationDirection,
        xRangeTwoD,
        runEveryFrames,
        restingFrames,
        cullInScene,
        distanceCulling,
        distanceToCull,
        prioritizeDistanceCulling,
        distanceCullingOnly;

        FrustumCulling[] scripts;
        

        void OnEnable()
        {
            autoCatchCamera = serializedObject.FindProperty("autoCatchCamera");
            mainCam = serializedObject.FindProperty("mainCam");
            gameView = serializedObject.FindProperty("gameView");
            activationDirection = serializedObject.FindProperty("activationDirection");
            xRangeTwoD = serializedObject.FindProperty("xRangeTwoD");
            runEveryFrames = serializedObject.FindProperty("runEveryFrames");
            restingFrames = serializedObject.FindProperty("restingFrames");

            cullInScene = serializedObject.FindProperty("cullInScene");
            
            distanceCulling = serializedObject.FindProperty("distanceCulling");
            distanceToCull = serializedObject.FindProperty("distanceToCull");
            prioritizeDistanceCulling = serializedObject.FindProperty("prioritizeDistanceCulling");
            distanceCullingOnly = serializedObject.FindProperty("distanceCullingOnly");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update ();

            var button = GUILayout.Button("Click for more tools");
            if (button) Application.OpenURL("https://assetstore.unity.com/publishers/39163");
            EditorGUILayout.Space(10);

            FrustumCulling script = (FrustumCulling) target;
            
            Object[] monoObjects = targets;
            scripts = new FrustumCulling[monoObjects.Length];
            for (int i = 0; i < monoObjects.Length; i++) {
                scripts[i] = monoObjects[i] as FrustumCulling;
            }
            EditorGUILayout.Space(10);
            
            
            EditorGUILayout.LabelField("Camera Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(autoCatchCamera);
            if (script.autoCatchCamera == false) {
                EditorGUILayout.PropertyField(mainCam);
            }
            EditorGUILayout.PropertyField(gameView);
            EditorGUILayout.PropertyField(activationDirection);
            if (script.gameView == FrustumCulling.GameViewOption.TwoD) {
                EditorGUILayout.PropertyField(xRangeTwoD);
            }
            EditorGUILayout.Space(10);


            EditorGUILayout.LabelField("Performance Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(runEveryFrames);
            EditorGUILayout.PropertyField(restingFrames);
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Objects Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(cullInScene);
            EditorGUILayout.Space(10);

            EditorGUILayout.LabelField("Distance Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(distanceCulling);
            
            EditorGUI.BeginDisabledGroup(script.distanceCulling == false);
                EditorGUILayout.PropertyField(distanceToCull);
                
                EditorGUI.BeginDisabledGroup(script.distanceCullingOnly == true);
                    EditorGUILayout.PropertyField(prioritizeDistanceCulling);
                EditorGUI.EndDisabledGroup ();
                
                EditorGUILayout.PropertyField(distanceCullingOnly);
            EditorGUI.EndDisabledGroup ();


            serializedObject.ApplyModifiedProperties();
        }
    }
}
