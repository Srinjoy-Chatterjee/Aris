//Copyright(c)2020 Procedural Worlds Pty Limited 
using UnityEngine;
using UnityEditor;
namespace GeNa.Core
{
    [CustomEditor(typeof(GeNaRiverExtension))]
    public class GeNaRiverExtensionEditor : GeNaSplineExtensionEditor
    {
        protected Editor m_riverProfileEditor;
        protected void OnEnable()
        {
            if (m_editorUtils == null)
                m_editorUtils = PWApp.GetEditorUtils(this, "GeNaSplineExtensionEditor");
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GeNaRiverExtension extension = target as GeNaRiverExtension;
            EditorGUI.BeginChangeCheck();
            {
                bool defaultGUIEnabled = GUI.enabled;
                EditorGUILayout.BeginHorizontal();
                m_editorUtils.LabelField("Tag", GUILayout.MaxWidth(40));
                extension.Tag = EditorGUILayout.TagField(extension.Tag);
                m_editorUtils.LabelField("Layer", GUILayout.MaxWidth(40));
                extension.Layer = EditorGUILayout.LayerField(extension.Layer);
                EditorGUILayout.EndHorizontal();
                m_editorUtils.InlineHelp("TagAndLayerHelp", HelpEnabled);
                Constants.RenderPipeline pipeline = GeNaUtility.GetActivePipeline();
                if (pipeline != Constants.RenderPipeline.BuiltIn)
                {
                    EditorGUILayout.BeginHorizontal();
                    extension.CastShadows = m_editorUtils.Toggle("CastShadows", extension.CastShadows);
                    extension.ReceiveShadows = m_editorUtils.Toggle("ReceiveShadows", extension.ReceiveShadows);
                    m_editorUtils.InlineHelp("ShadowsHelp", HelpEnabled);
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.Space();
                m_editorUtils.Heading("RiverMeshSettings");
                m_editorUtils.InlineHelp("RiverMeshSettings", HelpEnabled);
                EditorGUI.indentLevel++;
                extension.StartFlow = m_editorUtils.FloatField("StartDepth", extension.StartFlow, HelpEnabled);
                extension.CapDistance = m_editorUtils.FloatField("StartCapDistance", extension.CapDistance, HelpEnabled);
                extension.EndCapDistance = m_editorUtils.FloatField("EndCapDistance", extension.EndCapDistance, HelpEnabled);
                extension.RiverWidth = m_editorUtils.FloatField("RiverWidth", extension.RiverWidth, HelpEnabled);
                extension.VertexDistance = m_editorUtils.Slider("VertexDistance", extension.VertexDistance, 1.5f, 8.0f, HelpEnabled);
                extension.BankOverstep = m_editorUtils.FloatField("BankOverstep", extension.BankOverstep, HelpEnabled);
                if (extension.RiverProfile.RiverParameters.m_renderMode == Constants.ProfileRenderMode.PWShader)
                {
                    extension.UseWorldspaceTextureWidth = m_editorUtils.Toggle("Use Worldspace Width Texturing", extension.UseWorldspaceTextureWidth, HelpEnabled);
                    GUI.enabled = extension.UseWorldspaceTextureWidth;
                    EditorGUI.indentLevel++;
                    extension.WorldspaceWidthRepeat = m_editorUtils.Slider("Worldspace Width Repeat", extension.WorldspaceWidthRepeat, 0.5f, 50.0f, HelpEnabled);
                    EditorGUI.indentLevel--;
                    GUI.enabled = true;
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                m_editorUtils.Heading("RiverBehaviourSettings");
                m_editorUtils.InlineHelp("RiverBehaviourSettings", HelpEnabled);
                EditorGUI.indentLevel++;
                if (GeNaUtility.Gaia2Present)
                {
                    extension.UseGaiaSeaLevel = m_editorUtils.Toggle("UseSeaLevel", extension.UseGaiaSeaLevel, HelpEnabled);
                    GUI.enabled = !extension.UseGaiaSeaLevel;
                    EditorGUI.indentLevel++;
                    extension.SeaLevel = m_editorUtils.FloatField("SeaLevel", extension.SeaLevel, HelpEnabled);
                    EditorGUI.indentLevel--;
                    GUI.enabled = true;
                }
                else
                {
                    extension.SeaLevel = m_editorUtils.FloatField("SeaLevel", extension.SeaLevel, HelpEnabled);
                }
                extension.UpdateOnTerrainChange = m_editorUtils.Toggle("Auto-Update On Terrain Change", extension.UpdateOnTerrainChange, HelpEnabled);
                extension.RaycastTerrainOnly = m_editorUtils.Toggle("RaycastTerrainOnly", extension.RaycastTerrainOnly, HelpEnabled);
                extension.AddCollider = m_editorUtils.Toggle("AddCollider", extension.AddCollider, HelpEnabled);
                extension.SplitAtTerrains = m_editorUtils.Toggle("SplitMeshesAtTerrains", extension.SplitAtTerrains, HelpEnabled);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                GUI.enabled = extension.Spline.gameObject.activeInHierarchy;
                {
                    m_editorUtils.Heading("RiverRenderingSettings");
                    m_editorUtils.InlineHelp("RiverRenderingSettings", HelpEnabled);
                    EditorGUI.indentLevel++;
                    EditorGUI.BeginChangeCheck();
                    extension.RiverProfile = (GeNaRiverProfile)m_editorUtils.ObjectField("RiverProfile", extension.RiverProfile, typeof(GeNaRiverProfile), false, HelpEnabled);
                    if (EditorGUI.EndChangeCheck())
                        extension.UpdateMaterial();
                    if (extension.RiverProfile != null)
                    {
                        if (m_riverProfileEditor == null)
                            m_riverProfileEditor = CreateEditor(extension.RiverProfile);
                        GeNaRiverProfileEditor.SetProfile(extension.RiverProfile, (GeNaRiverProfileEditor)m_riverProfileEditor);
                        EditorGUI.BeginChangeCheck();
                        m_riverProfileEditor.OnInspectorGUI();
                        if (EditorGUI.EndChangeCheck())
                            extension.UpdateMaterial();
                    }
                    if (extension.RiverProfile != null && extension.RiverProfile.RiverParameters.m_renderMode == Constants.ProfileRenderMode.RiverFlow)
                    {
                        if (m_editorUtils.Button("Save Flow Texture"))
                            extension.CaptureRiverFlowTexture(true);
                    }
                    EditorGUI.indentLevel--;
                }
                GUI.enabled = defaultGUIEnabled;
                if (m_editorUtils.Button("MakeSplineDownhill", HelpEnabled))
                    extension.SetSplineToDownhill();
                if (m_editorUtils.Button("BakeRiver", HelpEnabled))
                {
                    if (EditorUtility.DisplayDialog(m_editorUtils.GetTextValue("BakeTitleRiver"), m_editorUtils.GetTextValue("BakeMessageRiver"), "Ok"))
                    {
                        extension.Bake(true);
                    }
                    GUIUtility.ExitGUI();
                }
                if (extension.HasBakedRivers())
                {
                    EditorGUILayout.Space(3);
                    if (m_editorUtils.Button("DeleteBakedRiver", HelpEnabled))
                    {
                        if (EditorUtility.DisplayDialog(m_editorUtils.GetTextValue("DeleteBakeTitleRiver"), m_editorUtils.GetTextValue("DeleteBakeMessageRiver"), "Ok", "Cancel"))
                        {
                            extension.DeleteBakedRiver(true);
                        }
                        GUIUtility.ExitGUI();
                    }
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(extension);
            }
        }
    }
}