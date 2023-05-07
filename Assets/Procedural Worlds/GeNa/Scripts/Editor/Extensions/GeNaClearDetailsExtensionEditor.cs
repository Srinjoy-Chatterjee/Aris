using UnityEditor;
using UnityEngine;
namespace GeNa.Core
{
    [CustomEditor(typeof(GeNaClearDetailsExtension))]
    public class GeNaClearDetailsExtensionEditor : GeNaSplineExtensionEditor
    {
        protected void OnEnable()
        {
            if (m_editorUtils == null)
                m_editorUtils = PWApp.GetEditorUtils(this, "GeNaSplineExtensionEditor");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!GeNaEditorUtility.ValidateComputeShader())
            {
                Color guiColor = GUI.backgroundColor;
                GUI.backgroundColor = Color.red;
                EditorGUILayout.BeginVertical(Styles.box);
                m_editorUtils.Text("NoComputeShaderHelp");
                EditorGUILayout.EndVertical();
                GUI.backgroundColor = guiColor;
                GUI.enabled = false;
            }
            GeNaClearDetailsExtension extension = target as GeNaClearDetailsExtension;
            EditorGUI.BeginChangeCheck();
            {
                extension.Width = m_editorUtils.FloatField("Width", extension.Width, HelpEnabled);
                extension.Shoulder = m_editorUtils.FloatField("Shoulder", extension.Shoulder, HelpEnabled);
                extension.ShoulderFalloff = m_editorUtils.CurveField("Shoulder Falloff", extension.ShoulderFalloff, HelpEnabled);
                m_editorUtils.Fractal(extension.MaskFractal, HelpEnabled);
                if (m_editorUtils.Button("Clear Details Btn", HelpEnabled))
                    extension.Clear();
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(extension);
            }
        }
    }
}