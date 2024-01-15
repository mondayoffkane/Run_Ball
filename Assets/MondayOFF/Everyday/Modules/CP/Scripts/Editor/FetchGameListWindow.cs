using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using UnityEngine;
using UnityEditor;

namespace MondayOFF
{
    [System.Serializable]
    internal class StoreLookupData
    {
        public AppInfo[] results;
    }

    [System.Serializable]
    internal class AppInfo
    {
        public bool shouldInclude;
        public string bundleId;
        public string trackId;
        public string trackName;
    }

    internal enum SortStatus
    {
        NONE = 0,
        ASC = 1,
        DESC = 2
    }

    internal class FetchGameListWindow : EditorWindow
    {
        const string QUERY_URL = "https://itunes.apple.com/lookup/?entity=software&id=1448226898&limit=200";
        const string AppStoreUrlBase = "https://apps.apple.com/app/id{0}";
        // ex/ https://apps.apple.com/app/id1575647531
        const string GooglePlayUrlBase = "https://play.google.com/store/apps/details?id={0}";
        // ex/ https://play.google.com/store/apps/details?id=com.mondayoff.tank
        CancellationTokenSource _cancellationSource = default;
        CancellationToken _cancellationToken = default;
        List<AppInfo> _gamesList = default;
        List<int> _indexBuffer = default;
        CPVideoList _cpVideoList = default;
        Vector2 _scrollPos = default;
        GUIStyle _titleStyle = default;
        GUIStyle _horizontalLineStyle;
        GUILayoutOption _heightSingleLine = GUILayout.Height(EditorGUIUtility.singleLineHeight);
        GUILayoutOption _widthSingleLine = GUILayout.Width(EditorGUIUtility.singleLineHeight);
        GUILayoutOption _width20 = GUILayout.Width(20f);
        GUILayoutOption _width55 = GUILayout.Width(55f);
        GUILayoutOption _width75 = GUILayout.Width(75f);
        GUILayoutOption _width180 = GUILayout.Width(180f);
        GUIStyle _iconButton = default;
        Texture2D _androidTexture = default;
        Texture2D _appleTexture = default;
        bool _isReady = false;
        SortStatus _selectionSort = SortStatus.NONE;
        SortStatus _nameSort = SortStatus.NONE;

        private string PlayStoreUrlBuilder(string bundleId)
        {
            return string.Format(GooglePlayUrlBase, bundleId);
        }

        private string AppStoreUrlBuilder(string trackId)
        {
            return string.Format(AppStoreUrlBase, trackId.ToString());
        }

        private string SortStatusToString(SortStatus status)
        {
            switch (status)
            {
                case SortStatus.ASC:
                    return "↑";
                case SortStatus.DESC:
                    return "↓";
            }
            return "";
        }

        private void NextStatus(ref SortStatus status)
        {
            status = (SortStatus)(((int)status + 1) % 3);
        }

        private void OnGUI()
        {
            if (!_isReady)
            {
                var loading = "Loading";
                for (int i = 0; i < ((int)EditorApplication.timeSinceStartup) % 4; ++i)
                {
                    loading += ".";
                }
                GUILayout.Label(loading);
                return;
            }

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            GUILayout.Label("Selected games will be prepared for CP object by pressing submit button.");

            GUILayout.Space(3);

            if (GUILayout.Button("Apply", _width180))
            {
                if (EditorUtility.DisplayDialog("Overwrite CP", "Do you want to overwrite CP list?\n(Videos must be referenced manually)", "Ok", "Cancel"))
                {
                    _cpVideoList.list.Clear();
                    _cpVideoList.list = null;
                    List<VideoAndUrl> newList = new List<VideoAndUrl>();
                    foreach (var item in _gamesList)
                    {
                        if (item.shouldInclude)
                        {
                            var urls = new VideoAndUrl();
                            urls.SetData(item.trackName, AppStoreUrlBuilder(item.trackId), PlayStoreUrlBuilder(item.bundleId));
                            newList.Add(urls);
                        }
                    }

                    _cpVideoList.list = newList;
                    EditorUtility.SetDirty(_cpVideoList);
                    this.Close();
                    return;
                }
            }
            GUILayout.Space(3);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Select All", _width75))
            {
                foreach (var item in _gamesList)
                {
                    item.shouldInclude = true;
                }
            }
            if (GUILayout.Button("Deselect All", _width75))
            {
                foreach (var item in _gamesList)
                {
                    item.shouldInclude = false;
                }
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Label("", _width20);

            GUILayout.Space(3);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button($"Select {SortStatusToString(_selectionSort)}", _titleStyle, _width75))
            {
                NextStatus(ref _selectionSort);
                _nameSort = SortStatus.NONE;
                switch (_selectionSort)
                {
                    case SortStatus.ASC:
                        _indexBuffer.Sort((first, second) =>
                        {
                            if (_gamesList[first].shouldInclude && !_gamesList[second].shouldInclude)
                            {
                                return -1;
                            }
                            else if (!_gamesList[first].shouldInclude && _gamesList[second].shouldInclude)
                            {
                                return 1;
                            }
                            return _gamesList[first].trackName.CompareTo(_gamesList[second].trackName);
                        });
                        break;

                    case SortStatus.DESC:
                        _indexBuffer.Sort((first, second) =>
                        {
                            if (_gamesList[first].shouldInclude && !_gamesList[second].shouldInclude)
                            {
                                return 1;
                            }
                            else if (!_gamesList[first].shouldInclude && _gamesList[second].shouldInclude)
                            {
                                return -1;
                            }
                            return _gamesList[first].trackName.CompareTo(_gamesList[second].trackName);
                        });
                        break;
                    case SortStatus.NONE:
                        _indexBuffer.Sort((first, second) => first - second);
                        break;
                }
            }
            if (GUILayout.Button($"Name {SortStatusToString(_nameSort)}", _titleStyle, _width180))
            {
                NextStatus(ref _nameSort);
                _selectionSort = SortStatus.NONE;

                switch (_nameSort)
                {
                    case SortStatus.ASC:
                        _indexBuffer.Sort((first, second) =>
                        {
                            return _gamesList[first].trackName.CompareTo(_gamesList[second].trackName);
                        });
                        break;

                    case SortStatus.DESC:
                        _indexBuffer.Sort((first, second) =>
                        {
                            return -_gamesList[first].trackName.CompareTo(_gamesList[second].trackName);
                        });
                        break;
                    case SortStatus.NONE:
                        _indexBuffer.Sort((first, second) => first - second);
                        break;
                }
            }
            GUILayout.Label("Bundle ID", _titleStyle, _width180);
            GUILayout.Label("AppStore ID", _titleStyle, _width180);
            GUILayout.Label("Store Link", _titleStyle, _width180);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            var prevColor = GUI.color;
            GUI.color = Color.gray;
            GUILayout.Box(GUIContent.none, _horizontalLineStyle);
            GUI.color = prevColor;

            GUILayout.Space(3);

            for (int i = 0; i < _indexBuffer.Count; ++i)
            {
                var item = _gamesList[_indexBuffer[i]];

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("", _width20);
                item.shouldInclude = EditorGUILayout.Toggle(item.shouldInclude, _width55);

                GUILayout.TextField(item.trackName, _width180);
                GUILayout.TextField(item.bundleId, _width180);
                GUILayout.TextField(item.trackId.ToString(), _width180);

                if (GUILayout.Button(_appleTexture, _iconButton, _widthSingleLine, _heightSingleLine))
                {
                    Application.OpenURL(AppStoreUrlBuilder(item.trackId));
                }
                if (GUILayout.Button(_androidTexture, _iconButton, _widthSingleLine, _heightSingleLine))
                {
                    Application.OpenURL(PlayStoreUrlBuilder(item.bundleId));
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private async void OnEnable()
        {
            this.minSize = new Vector2(750f, 400f);

            _horizontalLineStyle = new GUIStyle();
            _horizontalLineStyle.normal.background = EditorGUIUtility.whiteTexture;
            _horizontalLineStyle.margin = new RectOffset(0, 0, 4, 4);
            _horizontalLineStyle.fixedHeight = 1;

            _iconButton = new GUIStyle(GUI.skin.button);
            _iconButton.padding = new RectOffset(-2, -2, -2, -2);

            _titleStyle = new GUIStyle(GUI.skin.label);
            _titleStyle.fontSize += 3;
            _titleStyle.fontStyle = FontStyle.Bold;

            _androidTexture = new Texture2D(32, 32);
            _androidTexture.LoadImage(System.Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAA7FJREFUeF7tmlnITVEUx3+fkEQiQ2SeMosQ8uTFG6HkSXkxU6ZS5kKJSAhJeSIZHuRNnkSUzJLMQzIXD+apf9bJcbtf5559zz5f7tnr7Z6z1l1r/ffae6/h1FFwqiu4/wQAQgQUHIGwBQoeAOEQDFsgbIGCI5DHFoh0/EqJtatcKjV5ANAUWARsTWUZLAV2AV9SyqVizwMAGTQNuAtcqdC6IcBA4HCF/M5seQEgPRuAlUBXYBwwFGhvlr8GrgFngSfARmA18NPZswoF8wJA5mgbzDDHG9dj3w+LkkPA9gp9qIotDwBaA3ttG6Qx9hgwG3iXRigtr28AOgJngP5pDTP+O8B44LmjfKKYTwCaAeeA4WbFKaAbMDjBqpvAA2Ci8elsGAN8SvTGgcEnANGhJ7PuAX2AAcCtBDt1A9wAbgP9jHczsMLBv0QRXwC0Ax4Bzc2C78B6u9qmJ1h1FLgKrAOaGK9WvyfwItGjlAy+AFgA7ExpSxL7Eh83gy8ATgCTkzxK+f4kMCmlTCK7LwCuV3DYJRpXwqAzQWdIpuQLgEGAboEsSTWBDsdMqVoAWgI7LLU9zZ8C5jOwBmiRqaXw0Q5SFVcqrCYA5y3D/OCqq1oAVK3NjynfBKwCXgFtXY2qR04ZoW6XtQZwxLYPmOOqq1oAlKn1jSm/BIwCXpqxrnaVk3trxZOSq9ExhvtAb1dF1QKgBKdXTLnub2V+PgG4CIyI6XwMdA8AOCIQIsARuEgsbIFwBoRDMNwC4Rosch6gXn88C7tsSYrPROgCMDIGuhovPVxvs2rzgCMl3d791sn1CYDqj7kxh9V7mNpQAHQGjgPDAK2MDHnjORVuA6hlPtaGKVOApw0FQKS3FfDefiiqfEZANC2STpXBaYeu/2BV7RYoB7wvAKJyONNxmQ8ABIpKVoVolqQtpvlApuQLAJWnewBNhkSdyvQH1OP7au/V5SmdHqn+f2bv1Q6fZwOT/wKAUiO3AMtKHurq0hUm6mJT4TiLTvuFmXpb5s98RUCpqm3A4pKHyiA1AhNpZBaBEbEpgrTqXikA4BXev38eIqDoW0B9fM0M4qRh50N7oM9m1NyM025AM0avlNcZoDmhcvaIlLrqEPxmD/TJjAqreHdXU2TVGl4pLwCkRzP+WTY0mWnJUtw5JTkHgQ7AAWB5rX0kJWeV8OhbgfrS2UaAoiFKkLyuvv48rwjw7oirggCAK3K1IhcioFZW0tWPEAGuyNWKXIiAWllJVz9CBLgiVytyvwHG0MpB45UqXAAAAABJRU5ErkJggg=="));
            _androidTexture.Apply();

            _appleTexture = new Texture2D(32, 32);
            _appleTexture.LoadImage(System.Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAAAXNSR0IArs4c6QAAA8pJREFUeF7tmlmoTVEYx3/XPMSDMk9lVoYiIjwbboSEzCVvpvLiSSR5IF4IJZJEKaUUbyQpRQqRKfOUqczTRf9aajsdZ+9z1re2fe7Z39u9e61v/b//+tZa33DqqHGpq3H7yQnIPaDGGciPQI07QH4J5kcgPwI1zkDWj0BToCHkHmWRgLbAAmAu0BfoB3wLRULWCJgE7AZ6O4NfA51DekGWCFgJbAeaRHb7AjAm1O5Lb1YImAocL4JnA7C+sRPQDrgJdC0w9AvQH3jc2AlYA2wtYuRGYF1I47NyBC4CIwsMPQNMDHn7/1nvf98Bcv+XQMsIAboLFgLvQ+9+FjygG/AE+AVcBbYAh9zfadgf/BVoBvQCOgKfgTvAp4hl+q5nThfdffd/eaUCoMFAa+ANcAt4GIKRUEdA4FcB0wpud7n1aWAfcAr4GjFKEd9iYDYwoMDYn8ANYD+wB/hgRYY1Ac0Bvd262VvEgHwKnAReAOOA8YBi/ziRp8wHzscNTPLdkgC56zFA4Wxo0XGa6bzIay1LAo4Ac7zQlDf5uQuUvI6DFQGLgAPl4fca/QCYAlz30mKUC+gNvw309AWTcL7ujLHAvYTjSw6z8AC5vdw/LdHO6/I0EQsCjgKzTNDEK9HTOTl+WPIRvgQod38EKKJLQ6a7tNlsLV8COgHPCooYZuAKFH0HurjI0GwNXwKGuBjeDFAJRSLa3NN8CRgNqGyVhuioKa8wFV8CRgCXTBH9W9k7l1SZVoh9CegD3E2JAC0zFLhmuZ4vAarhvwJaWYIqoUslMpXKzMSXAAFRmjrIDFFpRYr/BwI6DiZiQYAqOPNM0CRTshdYlmxo/CgLApYCApWmqFeguoO3WBDQw5WzkhQzvAFHFMjzVrs7qGK9FgRo8bPAhIpRVD7xrXsZVFitSKwIWOLqdRWB8JikSvJwnyqyFQFtXH6u3CBNWQ7s9FnQigBhWAts9gFT5lwVRlQ+/1jmvL+GWxKgoEgxQVqVoRXADh/jNdeSAOmb4SrDvrji5l8GlIj9iBsY992aAK2n4qiKpKFEzRQZf8VigRAEtAfU8VVvP4So6bLNSnEIAoRNrbFzQAcroE6Pgh/9gMpMQhEggKOAE0Cxp1FurB6hAii1yNRGG+Zq/Uqxi8lhQPFGpuoBcTvRHdgE1LuUWbUDtc/UHC320xd1i/XDCOUXIlDtNnWGdwEH4xar5HtID4jikWHKFaLd4CR4VXVWZziYpEVAMAN8FecE+DJY7fNzD6j2HfTFn3uAL4PVPj/3gGrfQV/8uQf4Mljt838DSCeDQef+rx4AAAAASUVORK5CYII="));
            _appleTexture.Apply();

            var cpVidList = AssetDatabase.FindAssets("t:CPVideoList");

            if (cpVidList.Length != 1)
            {
                EverydayLogger.Error("There are zero or more than two Objects! " + cpVidList.Length);
                this.Close();
                return;
            }
            _cpVideoList = AssetDatabase.LoadAssetAtPath<CPVideoList>(AssetDatabase.GUIDToAssetPath(cpVidList[0]));

            _cancellationSource = new CancellationTokenSource();
            _cancellationToken = _cancellationSource.Token;

            using (HttpClient client = new HttpClient())
            {
                var getRequest = new HttpRequestMessage(HttpMethod.Get, QUERY_URL);
                var response = await client.SendAsync(getRequest, _cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    _cancellationToken.Register(() => response.Content.Dispose());
                    try
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var data = JsonUtility.FromJson<StoreLookupData>(content);
                        _gamesList = new List<AppInfo>(data.results);
                        _gamesList.RemoveAll(item => string.IsNullOrEmpty(item.bundleId));

                        _indexBuffer = new List<int>(_gamesList.Count);
                        for (int i = 0; i < _gamesList.Count; ++i)
                        {
                            var item = _gamesList[i];
                            foreach (var cp in _cpVideoList.list)
                            {
                                string id =
#if UNITY_IOS
                                        item.trackId.ToString()
#else
                                        item.bundleId
#endif
                                ;
                                if (cp.url.Contains(id))
                                {
                                    item.shouldInclude = true;
                                }
                            }

                            _indexBuffer.Add(i);
                        }

                        // _gamesList.Sort((a, b) => string.Compare(a.trackName, b.trackName));

                        _isReady = true;
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                else
                {
                    EverydayLogger.Error("Failed to fetch game list from AppStore");
                    this.Close();
                }
            }
        }

        private void OnDisable()
        {
            _isReady = false;

            _cancellationSource?.Cancel();
            _cancellationSource?.Dispose();

            _cancellationSource = null;

            _gamesList = null;

            if (_androidTexture != null) DestroyImmediate(_androidTexture);
            _androidTexture = null;
            if (_appleTexture != null) DestroyImmediate(_appleTexture);
            _appleTexture = null;
        }
    }
}