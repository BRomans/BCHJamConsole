using Gtec.Chain.Common.Nodes.FilterNodes;
using Gtec.Chain.Common.SignalProcessingPipelines.Configuration;
using UnityEditor;
using UnityEngine;

namespace Gtec.UnityInterface
{
    [CustomEditor(typeof(BCIConfiguration))]
    public class ConfigurationEditor : Editor
    {
        Texture banner;
        SerializedProperty _flashMode;
        SerializedProperty _syncMode;
        SerializedProperty _autoTraining;
        SerializedProperty TrainingInterval;
        SerializedProperty NumberOfTrainingTrials;
        SerializedProperty NumberOfClasses;
        SerializedProperty OnOffRatio;
        SerializedProperty SelectionThreshold;
        SerializedProperty SaveDataCSV;
        SerializedProperty _advancedSettings;
        SerializedProperty OnTimeMs;
        SerializedProperty OffTimeMs;
        SerializedProperty LowpassOrder;
        SerializedProperty RegularizationCoefficient;



        void OnEnable()
        {
            banner = (Texture)AssetDatabase.LoadAssetAtPath("Assets/g.tec/Unity Interface/Images/logo/unicorn-logo-horizontal.png", typeof(Texture));
            _flashMode = serializedObject.FindProperty("FlashMode");
            _syncMode = serializedObject.FindProperty("_syncMode");
            _autoTraining = serializedObject.FindProperty("_autoTraining");
            TrainingInterval = serializedObject.FindProperty("TrainingInterval");
            NumberOfTrainingTrials = serializedObject.FindProperty("NumberOfTrainingTrials");
            NumberOfClasses = serializedObject.FindProperty("NumberOfClasses");
            OnOffRatio = serializedObject.FindProperty("OnOffRatio");
            SelectionThreshold = serializedObject.FindProperty("SelectionThreshold");
            SaveDataCSV = serializedObject.FindProperty("SaveDataCSV");
            _advancedSettings = serializedObject.FindProperty("_advancedSettings");
            OnTimeMs = serializedObject.FindProperty("OnTimeMs");
            OffTimeMs = serializedObject.FindProperty("OffTimeMs");
            LowpassOrder = serializedObject.FindProperty("LowpassOrder");
            RegularizationCoefficient = serializedObject.FindProperty("RegularizationCoefficient");

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            GUILayout.Box(banner, GUILayout.ExpandWidth(true), GUILayout.Height(80));
            GUI.enabled = false;
            EditorGUILayout.PropertyField(_flashMode);
            GUI.enabled = true;
            //EditorGUILayout.PropertyField(_autoTraining);
            if (_autoTraining.boolValue)
            {
                EditorGUILayout.PropertyField(TrainingInterval);

            }
            EditorGUILayout.PropertyField(NumberOfTrainingTrials);
            EditorGUILayout.PropertyField(NumberOfClasses);
            if (_flashMode.enumValueIndex == (int)BCIConfiguration.StimulationMode.ERP || _flashMode.enumValueIndex == (int)BCIConfiguration.StimulationMode.ERPNoOverlap)
                EditorGUILayout.PropertyField(OnTimeMs);
            EditorGUILayout.PropertyField(SelectionThreshold);
            EditorGUILayout.PropertyField(SaveDataCSV);
            EditorGUILayout.PropertyField(_advancedSettings);

            if (_advancedSettings.boolValue)
            {
                EditorGUILayout.HelpBox("These settings are intended for advanced users and research, changing them may severly impact the performance of the BCI.", MessageType.Info);
                EditorGUILayout.PropertyField(_syncMode);
                if (_flashMode.enumValueIndex == (int)BCIConfiguration.StimulationMode.ERP)
                {
                    EditorGUILayout.PropertyField(OffTimeMs);
                    EditorGUILayout.PropertyField(OnOffRatio);
                    EditorGUILayout.HelpBox("The sum of OnTime + OffTime must be >= (n_classes / refresh_rate (60Hz) * 1000)", MessageType.Warning);
                }
                EditorGUILayout.PropertyField(LowpassOrder);
                EditorGUILayout.PropertyField(RegularizationCoefficient);
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}
