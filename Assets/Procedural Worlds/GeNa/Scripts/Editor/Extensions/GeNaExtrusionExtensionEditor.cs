using UnityEditor;
using UnityEngine;
namespace GeNa.Core
{
    [CustomEditor(typeof(GeNaExtrusionExtension))]
    public class GeNaExtrusionExtensionEditor : GeNaSplineExtensionEditor
    {
        protected void OnEnable()
        {
            if (m_editorUtils == null)
                m_editorUtils = PWApp.GetEditorUtils(this, "GeNaSplineExtensionEditor");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GeNaExtrusionExtension extension = target as GeNaExtrusionExtension;
            EditorGUI.BeginChangeCheck();
            {
                extension.SharedMaterial = (Material)m_editorUtils.ObjectField("Extrusion Material", extension.SharedMaterial, typeof(Material), true, HelpEnabled);
                extension.Smoothness = m_editorUtils.Slider("Mesh Smoothness", extension.Smoothness, 1f, 5f, HelpEnabled);
                extension.Width = m_editorUtils.FloatField("Mesh Width", extension.Width, HelpEnabled);
                extension.HeightOffset = m_editorUtils.FloatField("Mesh Height Offset", extension.HeightOffset, HelpEnabled);
                extension.SnapToGround = m_editorUtils.Toggle("Mesh Snap to Terrain", extension.SnapToGround, HelpEnabled);
                extension.Curve = m_editorUtils.CurveField("Extrusion", extension.Curve, HelpEnabled);
                extension.SplitAtTerrains = m_editorUtils.Toggle("SplitMeshesAtTerrains", extension.SplitAtTerrains, HelpEnabled);
                if (m_editorUtils.Button("BakeExtrusion"))
                    extension.Bake();
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(extension);
            }
        }
    }
}