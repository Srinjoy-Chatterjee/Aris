//Copyright(c)2020 Procedural Worlds Pty Limited 
using UnityEditor;
using UnityEngine;
namespace GeNa.Core
{
    [CustomEditor(typeof(GeNaRoadExtension))]
    public class GeNaRoadExtensionEditor : GeNaSplineExtensionEditor
    {
        protected Editor m_roadProfileEditor;
        protected void OnEnable()
        {
            if (m_editorUtils == null)
                m_editorUtils = PWApp.GetEditorUtils(this, "GeNaSplineExtensionEditor");
        }
        public override void OnSceneGUI()
        {
            GeNaRoadExtension roadExtension = target as GeNaRoadExtension;
            if (roadExtension.Spline.Settings.Advanced.DebuggingEnabled == false)
                return;
            Handles.color = Color.red;
            foreach (GeNaCurve curve in roadExtension.Spline.Curves)
            {
                DrawCurveDirecton(curve);
            }
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GeNaRoadExtension extension = target as GeNaRoadExtension;
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
                EditorGUILayout.BeginHorizontal();
                extension.CastShadows = m_editorUtils.Toggle("CastShadows", extension.CastShadows);
                var sceneView = SceneView.lastActiveSceneView;
                if (sceneView != null)
                {
                    var camera = sceneView.camera;
                    if (camera != null)
                    {
                        if (camera.actualRenderingPath != RenderingPath.Forward)
                            GUI.enabled = false;
                    }
                }
                extension.ReceiveShadows = m_editorUtils.Toggle("ReceiveShadows", extension.ReceiveShadows);
                GUI.enabled = defaultGUIEnabled;
                EditorGUILayout.EndHorizontal();
                m_editorUtils.InlineHelp("ShadowsHelp", HelpEnabled);
                EditorGUILayout.Space();
                m_editorUtils.Heading("RoadMeshSettings");
                m_editorUtils.InlineHelp("RoadMeshSettings", HelpEnabled);
                EditorGUI.indentLevel++;
                extension.Width = m_editorUtils.FloatField("MeshWidth", extension.Width, HelpEnabled);
                extension.IntersectionSize = m_editorUtils.Slider("IntersectionSize", extension.IntersectionSize, 0.8f, 1.2f, HelpEnabled);
                extension.UseSlopedCrossSection = m_editorUtils.Toggle("UseSlopedCrossSection", extension.UseSlopedCrossSection, HelpEnabled);
                extension.SplitAtTerrains = m_editorUtils.Toggle("SplitMeshesAtTerrains", extension.SplitAtTerrains, HelpEnabled);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                m_editorUtils.Heading("RoadBehaviourSettings");
                m_editorUtils.InlineHelp("RoadBehaviourSettings", HelpEnabled);
                EditorGUI.indentLevel++;
                extension.AddRoadCollider = m_editorUtils.Toggle("AddCollider", extension.AddRoadCollider, HelpEnabled);
                extension.RaycastTerrainOnly = m_editorUtils.Toggle("RaycastTerrainOnly", extension.RaycastTerrainOnly, HelpEnabled);
                extension.ConformToGround = m_editorUtils.Toggle("ConformToGround", extension.ConformToGround, HelpEnabled);
                if (!extension.ConformToGround)
                {
                    EditorGUI.indentLevel++;
                    extension.GroundAttractDistance = m_editorUtils.FloatField("GroundSnapDistance", extension.GroundAttractDistance, HelpEnabled);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                m_editorUtils.Heading("RoadTools");
                m_editorUtils.InlineHelp("RoadTools", HelpEnabled);
                EditorGUI.indentLevel++;
                if (m_editorUtils.Button("LevelIntersectionTangents", GUILayout.Width(300)))
                {
                    extension.LevelIntersectionTangents();
                }
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                GUI.enabled = extension.Spline.gameObject.activeInHierarchy;
                {
                    m_editorUtils.Heading("RoadRenderingSettings");
                    m_editorUtils.InlineHelp("RoadRenderingSettings", HelpEnabled);
                    EditorGUI.indentLevel++;
                    extension.RoadProfile = (GeNaRoadProfile)m_editorUtils.ObjectField("RoadProfile", extension.RoadProfile, typeof(GeNaRoadProfile), true, HelpEnabled);
                    if (extension.RoadProfile != null)
                    {
                        if (m_roadProfileEditor == null)
                            m_roadProfileEditor = CreateEditor(extension.RoadProfile);
                        GeNaRoadProfileEditor.SetProfile(extension.RoadProfile, (GeNaRoadProfileEditor)m_roadProfileEditor);
                        m_roadProfileEditor.OnInspectorGUI();
                    }
                    EditorGUI.indentLevel--;
                }
                GUI.enabled = defaultGUIEnabled;
                if (m_editorUtils.Button("BakeRoad", HelpEnabled))
                {
                    if (EditorUtility.DisplayDialog(m_editorUtils.GetTextValue("BakeTitleRoad"), m_editorUtils.GetTextValue("BakeMessageRoad"), "Ok"))
                        extension.Bake(true);
                    GUIUtility.ExitGUI();
                }
                if (extension.HasBakedRoads())
                {
                    EditorGUILayout.Space(3);
                    if (m_editorUtils.Button("DeleteBakedRoad", HelpEnabled))
                    {
                        if (EditorUtility.DisplayDialog(m_editorUtils.GetTextValue("DeleteBakeTitleRoad"), m_editorUtils.GetTextValue("DeleteBakeMessageRoad"), "Ok", "Cancel"))
                        {
                            extension.DeleteBakedRoad(true);
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
        private void DrawCurveDirecton(GeNaCurve geNaCurve)
        {
            Vector3 forward = (geNaCurve.P3 - geNaCurve.P0).normalized;
            GeNaSample geNaSample = geNaCurve.GetSample(0.45f);
            DrawArrow(geNaSample.Location, forward);
            geNaSample = geNaCurve.GetSample(0.5f);
            DrawArrow(geNaSample.Location, forward);
            geNaSample = geNaCurve.GetSample(0.55f);
            DrawArrow(geNaSample.Location, forward);
        }
        private void DrawArrow(Vector3 position, Vector3 direction)
        {
            direction.Normalize();
            Vector3 right = Vector3.Cross(Vector3.up, direction).normalized;
            Handles.DrawLine(position, position + (-direction + right) * 0.75f);
            Handles.DrawLine(position, position + (-direction - right) * 0.75f);
        }
        [MenuItem("GameObject/GeNa/Add Road Spline", false, 17)]
        public static void AddRoadSpline(MenuCommand command)
        {
            Spline spline = Spline.CreateSpline("Road Spline");
            if (spline != null)
            {
                Undo.RegisterCreatedObjectUndo(spline.gameObject, $"[{PWApp.CONF.Name}] Created '{spline.gameObject.name}'");
                GeNaCarveExtension carve = spline.AddExtension<GeNaCarveExtension>();
                carve.name = "Carve";
                GeNaClearCollidersExtension clearColliders = spline.AddExtension<GeNaClearCollidersExtension>();
                GeNaClearDetailsExtension clearDetails = spline.AddExtension<GeNaClearDetailsExtension>();
                GeNaClearTreesExtension clearTrees = spline.AddExtension<GeNaClearTreesExtension>();
                GeNaTerrainExtension terrainTexture = spline.AddExtension<GeNaTerrainExtension>();
                GeNaRoadExtension roads = spline.AddExtension<GeNaRoadExtension>();
                roads.GroundAttractDistance = 0.0f;
                roads.name = "Road";
                Selection.activeGameObject = spline.gameObject;
                carve.Width = roads.Width * 1.2f;
                clearDetails.Width = roads.Width;
                clearTrees.Width = roads.Width;
                terrainTexture.Width = roads.Width;
                if (terrainTexture != null)
                {
                    terrainTexture.name = "Texture";
                    terrainTexture.EffectType = EffectType.Texture;
                    terrainTexture.Width = roads.Width;
                }
                if (clearDetails != null)
                {
                    clearDetails.name = "Clear Details/Grass";
                    clearDetails.Width = roads.Width;
                }
                if (clearTrees != null)
                {
                    clearTrees.name = "Clear Trees";
                    clearTrees.Width = roads.Width;
                }
                if (clearColliders != null)
                {
                    clearColliders.name = "Clear Colliders";
                    clearColliders.Width = roads.Width * 2.0f;
                }
            }
        }
    }
}