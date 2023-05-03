using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace MondayOFF {
    [CustomEditor(typeof(CPVideoList))]
    internal class VideoListEditor : Editor {
        private SerializedProperty _property;
        private ReorderableList _list;

        private void OnEnable() {
            _property = serializedObject.FindProperty("list");
            _list = new ReorderableList(serializedObject, _property, true, true, true, true) {
                drawHeaderCallback = DrawListHeader,
                drawElementCallback = DrawListElement,
                elementHeight = 205,
                onAddCallback = list => {
                    _property.arraySize++;
                }
            };
        }

        private void DrawListHeader(Rect rect) {
            GUI.Label(rect, "Videos");
        }

        private void DrawListElement(Rect rect, int index, bool isActive, bool isFocused) {
            var item = _property.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, item);
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            if (GUILayout.Button("Open Game List Window")) {
                var gameListWindow = UnityEditor.EditorWindow.GetWindow<FetchGameListWindow>(true, "MondayOFF Games on AppStore");
            }
            GUILayout.Space(20f);

            if (GUILayout.Button("Export Current List")) {
                string msg = "CP LIST\n";
                for (int i = 0; i < _list.count; ++i) {
                    msg += $"\t{(_property.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue)}\n";
                }
                Debug.Log(msg);
            }
            GUILayout.Space(20f);

            var size = EditorGUILayout.DelayedIntField("Count", _list.count);
            var prevSize = _property.arraySize;
            if (EditorGUI.EndChangeCheck()) {
                _property.arraySize = size;
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            _list.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
            if(GUI.changed) {
                EditorUtility.SetDirty(target);
            }
        }
    }


    [CustomPropertyDrawer(typeof(VideoAndUrl))]
    internal class VideoListDrawer : PropertyDrawer {
        const float BUTTON_WIDTH = 50f;
        const float PADDING = 4f;

        GUIStyle style = new GUIStyle(GUI.skin.label) { richText = true };
        public override void OnGUI(Rect rect,
                            SerializedProperty property,
                            GUIContent label) {
            if (property.hasMultipleDifferentValues) {
                return;
            }

            var prevWidth = EditorGUIUtility.labelWidth;

            var nameRect = new Rect(rect);

            nameRect.xMax = rect.xMax;
            nameRect.height = EditorGUIUtility.singleLineHeight;

            nameRect.xMax *= .3f;
            EditorGUI.LabelField(nameRect, "<b>Video File</b>", style);

            nameRect.xMin = nameRect.xMax;
            nameRect.xMax = rect.xMax;

            var videoProperty = property.FindPropertyRelative("videoClip");
            EditorGUI.ObjectField(nameRect, videoProperty, typeof(UnityEngine.Video.VideoClip), GUIContent.none);

            nameRect.xMin = rect.xMin;

            nameRect.yMin += EditorGUIUtility.singleLineHeight * 1.5f;
            nameRect.yMax += EditorGUIUtility.singleLineHeight * 1.5f;

            EditorGUI.LabelField(nameRect, "<b>Game Name</b>    <i>(Optional)</i>", style);

            nameRect.yMin += EditorGUIUtility.singleLineHeight;
            nameRect.yMax += EditorGUIUtility.singleLineHeight;
            var nameProperty = property.FindPropertyRelative("name");
            nameProperty.stringValue = EditorGUI.TextField(nameRect, nameProperty.stringValue);


            nameRect.yMin += EditorGUIUtility.singleLineHeight * 1.5f;
            nameRect.yMax += EditorGUIUtility.singleLineHeight * 1.5f;
            EditorGUI.LabelField(nameRect, "<b>iOS Store Url</b>    <size=10><i>https://apps.apple.com/app/id<AppID></i></size>", style);

            nameRect.yMin += EditorGUIUtility.singleLineHeight;
            nameRect.yMax += EditorGUIUtility.singleLineHeight;
            nameRect.xMax = rect.xMax - BUTTON_WIDTH;
            var iOSUrlProperty = property.FindPropertyRelative("iosUrl");
            iOSUrlProperty.stringValue = EditorGUI.TextField(nameRect, iOSUrlProperty.stringValue);

            nameRect.xMin = nameRect.xMax + PADDING;
            nameRect.xMax = rect.xMax;
            if (GUI.Button(nameRect, "Go")) {
                Application.OpenURL(iOSUrlProperty.stringValue);
            }

            nameRect.yMin += EditorGUIUtility.singleLineHeight;
            nameRect.yMax += EditorGUIUtility.singleLineHeight;
            nameRect.xMin = rect.xMin;
            nameRect.xMax = rect.xMax;
            EditorGUI.LabelField(nameRect, "<b>Play Store Url</b>    <size=10><i>https://play.google.com/store/apps/details?id=<package_name></i></size>", style);

            nameRect.yMin += EditorGUIUtility.singleLineHeight;
            nameRect.yMax += EditorGUIUtility.singleLineHeight;
            nameRect.xMax = rect.xMax - BUTTON_WIDTH;
            var androidUrlProperty = property.FindPropertyRelative("androidUrl");
            androidUrlProperty.stringValue = EditorGUI.TextField(nameRect, androidUrlProperty.stringValue);

            nameRect.xMin = nameRect.xMax + PADDING;
            nameRect.xMax = rect.xMax;
            if (GUI.Button(nameRect, "Go")) {
                Application.OpenURL(androidUrlProperty.stringValue);
            }

            nameRect.yMin += EditorGUIUtility.singleLineHeight;
            nameRect.yMax += EditorGUIUtility.singleLineHeight;
            nameRect.xMin = rect.xMin;
            nameRect.xMax = rect.xMax;
            EditorGUI.LabelField(nameRect, "<b>Weight</b> <size=10><i>(Higher is better</i></size>)", style);

            nameRect.yMin += EditorGUIUtility.singleLineHeight;
            nameRect.yMax += EditorGUIUtility.singleLineHeight;

            var weightProperty = property.FindPropertyRelative("weight");
            weightProperty.floatValue = Mathf.Max(0f, EditorGUI.FloatField(nameRect, weightProperty.floatValue));

            nameRect.yMin += EditorGUIUtility.singleLineHeight;
            nameRect.yMax += EditorGUIUtility.singleLineHeight;

            if (GUI.Button(nameRect, "Remove")) {
                if (EditorUtility.DisplayDialog("Remove Video?", "This action cannot be undone.", "OK", "NO")) {
                    var list = (property.serializedObject.targetObject as CPVideoList).list;
                    var path = property.propertyPath;
                    var startingIndex = path.IndexOf('[');
                    var indexString = path.Substring(startingIndex + 1, path.Length - startingIndex - 2);
                    if (System.Int32.TryParse(indexString, out int index)) {
                        list.RemoveAt(index);
                        (property.serializedObject.targetObject as CPVideoList).list = list;
                        property.serializedObject.Update();
                        property.serializedObject.ApplyModifiedProperties();

                        EditorUtility.SetDirty(property.serializedObject.targetObject);
                    }
                }
            }

            EditorGUIUtility.labelWidth = prevWidth;
        }
    }
}