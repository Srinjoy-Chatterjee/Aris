using UnityEditor;
using UnityEngine;
namespace GeNa.Core
{
    [CustomEditor(typeof(GeNaCarveExtension))]
    public class GeNaCarveExtensionEditor : GeNaSplineExtensionEditor
    {
        protected void OnEnable()
        {
            if (m_editorUtils == null)
                m_editorUtils = PWApp.GetEditorUtils(this, "GeNaSplineExtensionEditor");
        }
        public override void OnInspectorGUI()
        {
            Initialize();
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
            GeNaCarveExtension extension = target as GeNaCarveExtension;
            EditorGUI.BeginChangeCheck();
            {
                extension.Width = m_editorUtils.FloatField("Width", extension.Width, HelpEnabled);
                if (extension.Width < 0.05f)
                {
                    extension.Width = 0.05f;
                }
                extension.HeightOffset = m_editorUtils.FloatField("Height Offset", extension.HeightOffset, HelpEnabled);
                extension.Shoulder = m_editorUtils.FloatField("Shoulder", extension.Shoulder, HelpEnabled);
                extension.ShoulderFalloff = m_editorUtils.CurveField("Shoulder Falloff", extension.ShoulderFalloff, HelpEnabled);
                extension.RoadLike = m_editorUtils.Toggle("Road Like", extension.RoadLike, HelpEnabled);
                m_editorUtils.Fractal(extension.MaskFractal, HelpEnabled);
                extension.ShowPreview = m_editorUtils.Toggle("Preview Btn", extension.ShowPreview, HelpEnabled);
                if (m_editorUtils.Button("Carve Btn", HelpEnabled))
                    extension.Carve();
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(extension);
            }
        }
    }
}