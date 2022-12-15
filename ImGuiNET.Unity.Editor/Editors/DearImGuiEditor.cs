using System.Text;

using UnityEngine;
using UnityEditor;

namespace ImGuiNET.Unity.Editor
{
    [CustomEditor(typeof(DearImGui))]
    internal class DearImGuiEditor : UnityEditor.Editor
    {
        private SerializedProperty _doGlobalLayout;

        private SerializedProperty _camera;
        private SerializedProperty _renderFeature;
        private SerializedProperty _commandBufferMode;

        private SerializedProperty _renderer;
        private SerializedProperty _platform;

        private SerializedProperty _initialConfiguration;
        private SerializedProperty _fontAtlasConfiguration;
        private SerializedProperty _iniSettings;

        private SerializedProperty _shaders;
        private SerializedProperty _style;
        private SerializedProperty _cursorShapes;

        private readonly StringBuilder _messages = new StringBuilder();

        private void OnEnable()
        {
            _doGlobalLayout = serializedObject.FindProperty("_doGlobalLayout");
            _camera = serializedObject.FindProperty("_camera");
            _renderFeature = serializedObject.FindProperty("_renderFeature");
            _renderer = serializedObject.FindProperty("_rendererType");
            _platform = serializedObject.FindProperty("_platformType");
            _commandBufferMode = serializedObject.FindProperty("_commandBufferMode");
            _initialConfiguration = serializedObject.FindProperty("_initialConfiguration");
            _fontAtlasConfiguration = serializedObject.FindProperty("_fontAtlasConfiguration");
            _iniSettings = serializedObject.FindProperty("_iniSettings");
            _shaders = serializedObject.FindProperty("_shaders");
            _style = serializedObject.FindProperty("_style");
            _cursorShapes = serializedObject.FindProperty("_cursorShapes");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            CheckRequirements();

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(_doGlobalLayout);
            if (RenderUtils.IsUsingURP())
                EditorGUILayout.PropertyField(_renderFeature);
            EditorGUILayout.PropertyField(_camera);
            EditorGUILayout.PropertyField(_renderer);
            EditorGUILayout.PropertyField(_platform);
            EditorGUILayout.PropertyField(_commandBufferMode);
            EditorGUILayout.PropertyField(_initialConfiguration);
            EditorGUILayout.PropertyField(_fontAtlasConfiguration);
            EditorGUILayout.PropertyField(_iniSettings);
            EditorGUILayout.PropertyField(_shaders);
            EditorGUILayout.PropertyField(_style);
            EditorGUILayout.PropertyField(_cursorShapes);

            var changed = EditorGUI.EndChangeCheck();
            if (changed)
                serializedObject.ApplyModifiedProperties();

            if (!Application.isPlaying)
                return;

            var reload = GUILayout.Button("Reload");
            if (changed || reload)
                (target as DearImGui)?.Reload();
        }

        private void CheckRequirements()
        {
            _messages.Clear();
            if (_camera.objectReferenceValue == null)
                _messages.AppendLine("Must assign a Camera.");
            if (RenderUtils.IsUsingURP() && _renderFeature.objectReferenceValue == null)
                _messages.AppendLine("Must assign a RenderFeature when using the URP.");
            if (!Platform.IsAvailable((Platform.Type)_platform.enumValueIndex))
                _messages.AppendLine("Platform not available.");
            if (_messages.Length > 0)
                EditorGUILayout.HelpBox(_messages.ToString(), MessageType.Error);
        }
    }
}
