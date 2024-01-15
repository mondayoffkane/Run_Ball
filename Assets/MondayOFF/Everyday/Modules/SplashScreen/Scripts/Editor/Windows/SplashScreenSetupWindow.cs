using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace MondayOFF {
    public class SplashScreenSetupWindow : EditorWindow {
        const string SplashScenePath = "Assets/MondayOFF/Everyday/Modules/SplashScreen/Scenes/MondayOFFSplashScene.unity";
        const float WIDTH = 400f;
        const float HEIGHT = 450f;
        const float PADDING = 10f;
        private static Rect rect = default;
        private EverydaySettings _settings = default;
        private SerializedProperty _spriteSerializedObject = default;
        private List<SceneAsset> _sceneAssets = new List<SceneAsset>();
        private int _targetSceneIndex = 1;
        private Vector2 _scrollPos = Vector2.zero;


        [MenuItem("Everyday/Setup Splash Screen", false, 120)]
        private static void ShowWindow() {
            var assets = AssetDatabase.FindAssets("t:EverydaySettings");
            if (assets.Length != 1) {
                EverydayLogger.Info("Failed to locate EverydaySettings.asset");
                return;
            }

            rect = new Rect(100, 100, WIDTH, HEIGHT);
            var window = GetWindowWithRect<SplashScreenSetupWindow>(rect, true, "Set up splash screen logo");
            window._settings = AssetDatabase.LoadAssetAtPath<EverydaySettings>(AssetDatabase.GUIDToAssetPath(assets[0]));
            if (window._settings == null) {
                EverydayLogger.Info("Failed to cast asset to EverydaySettings");
                return;
            }
            window._spriteSerializedObject = new SerializedObject(window._settings).FindProperty("companyLogo");

            var sceneCount = SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < sceneCount; i++) {
                var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                window._sceneAssets.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath));
            }

            window.titleContent = new GUIContent("Setup Splash Screen");
            window.Show();
        }

        private void OnGUI() {
            GUILayout.BeginArea(new Rect(PADDING, PADDING, WIDTH - PADDING * 2, HEIGHT - PADDING * 2));
            // Logo
            EditorGUILayout.PropertyField(_spriteSerializedObject);
            EditorGUILayout.Space();

            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Save")) {
                EverydayLogger.Info("Saved logo sprite.");
                SaveSprite();
            } else if (GUILayout.Button("Open Splash Scene")) {
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) {
                    EditorSceneManager.OpenScene(SplashScenePath, UnityEditor.SceneManagement.OpenSceneMode.Single);
                }
            }
            EditorGUILayout.Space(16);

            // Scemes
            GUILayout.Label("Scenes to include in build:", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Check Scene to launch after splash");

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
            for (int i = 0; i < _sceneAssets.Count; ++i) {
                EditorGUILayout.BeginHorizontal();
                if (EditorGUILayout.Toggle(i == _targetSceneIndex, GUILayout.MaxWidth(32f))) {
                    if (_sceneAssets[i] != null && AssetDatabase.GetAssetPath(_sceneAssets[i]) != SplashScenePath) {
                        _targetSceneIndex = i;
                    } else {
                        EverydayLogger.Warn("Selected item is null or Splash Scene! Please select different scene.");
                        _targetSceneIndex = -1;
                    }
                }
                EditorGUILayout.LabelField($"{i}.", EditorStyles.boldLabel, GUILayout.MaxWidth(32f));
                _sceneAssets[i] = (SceneAsset)EditorGUILayout.ObjectField(_sceneAssets[i], typeof(SceneAsset), false);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            if (GUILayout.Button("Add Splash Scene to index 0")) {
                var splashScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(SplashScenePath);
                if (splashScene == null) {
                    EverydayLogger.Warn($"SplashScene not found at {SplashScenePath}!");
                } else {
                    if (_sceneAssets.Contains(splashScene)) {
                        EverydayLogger.Warn($"SplashScene is already included in the build");
                    } else {
                        if (_sceneAssets[0] == null) {
                            _sceneAssets[0] = splashScene;
                        } else {
                            _sceneAssets.Insert(0, splashScene);
                        }
                        _targetSceneIndex = 1;
                    }
                }
            } else if (GUILayout.Button("Apply To Build Settings")) {
                SetEditorBuildSettingsScenes();
                if (0 <= _targetSceneIndex && _targetSceneIndex < _sceneAssets.Count) {
                    _settings.gameSceneName = _sceneAssets[_targetSceneIndex].name;
                }
            }

            GUILayout.Space(8);

            if (GUILayout.Button("Close")) {
                Close();
            }
            GUILayout.EndArea();
        }

        private void SaveSprite() {
            _settings.companyLogo = _spriteSerializedObject.objectReferenceValue as Sprite;
            EditorUtility.SetDirty(_settings);
        }

        private void SetEditorBuildSettingsScenes() {
            // Find valid Scene paths and make a list of EditorBuildSettingsScene
            List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
            foreach (var sceneAsset in _sceneAssets) {
                string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
                if (!string.IsNullOrEmpty(scenePath))
                    editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
            }

            // Set the Build Settings window Scene list
            EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();

            EverydayLogger.Info("Build Settings Scene list has been updated.");
        }
    }
}
