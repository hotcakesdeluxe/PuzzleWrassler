using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PHL.Common.Todo
{
    public class ToDoEditor : EditorWindow
    {
        private FileSystemWatcher _watcher;
        private FileInfo[] _files;
        private TodoData _data;

        private string _searchString = "";
        public string SearchString
        {
            get { return _searchString; }
            set
            {
                if (value != _searchString)
                {
                    _searchString = value;
                    RefreshEntriesToShow();
                }
            }
        }

        private string _newTagName = "";
        private Vector2 _sidebarScroll;
        private Vector2 _mainAreaScroll;
        private int _currentTag = -1;
        private TodoEntry[] _entriesToShow;
        
        private float SidebarWidth
        {
            get { return position.width / 8f; }
        }

        private string[] Tags
        {
            get
            {
                if (_data != null && _data.tagsList.Count > 0)
                {
                    return _data.tagsList.ToArray();
                }
                else
                {
                    return new string[] { "TODO" };
                }
            }
        }

        [MenuItem("Window/PHL/ToDo")]
        public static void Init()
        {
            ToDoEditor window = GetWindow<ToDoEditor>();
            window.minSize = new Vector2(400, 250);
            Texture icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/PHLCommon/ToDo/Editor/Images/ToDoIcon.png");
            window.titleContent = new GUIContent("ToDo", icon);
            window.Show();
        }

        private void OnEnable()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }

            RefreshFiles();

            _data = LoadData();
            RefreshEntriesToShow();

            if (_data.autoScan)
            {
                _watcher = new FileSystemWatcher(Application.dataPath, "*.cs");
                _watcher.Changed += OnChanged;
                _watcher.Deleted += OnDeleted;
                _watcher.Renamed += OnRenamed;
                _watcher.Created += OnCreated;

                _watcher.EnableRaisingEvents = true;
                _watcher.IncludeSubdirectories = true;
            }
        }

        private void OnGUI()
        {
            if (_data == null)
            {
                _data = new TodoData();
                GUILayout.Label("No data loaded", EditorStyles.centeredGreyMiniLabel);
                return;
            }

            Toolbar();

            using (new HorizontalBlock())
            {
                Sidebar();
                MainArea();
            }
        }
        
        public static TodoData LoadData()
        {
            string serializedData = EditorPrefs.GetString("PHL.ToDo.Data." + Application.dataPath, "");

            TodoData todoData = new TodoData();
            todoData.autoScan = true;

            if (!string.IsNullOrEmpty(serializedData))
            {
                EditorJsonUtility.FromJsonOverwrite(serializedData, todoData);
            }

            return todoData;
        }

        public static void SaveData(TodoData data)
        {
            EditorPrefs.SetString("PHL.ToDo.Data." + Application.dataPath, EditorJsonUtility.ToJson(data));
        }
        
        #region GUI

        private void Toolbar()
        {
            using (new HorizontalBlock(EditorStyles.toolbar))
            {
                EditorGUI.BeginChangeCheck();
                _data.autoScan = GUILayout.Toggle(_data.autoScan, "Auto Scan", EditorStyles.toolbarButton);
                if(EditorGUI.EndChangeCheck())
                {
                    SaveData(_data);
                }

                if (GUILayout.Button("Force scan", EditorStyles.toolbarButton))
                {
                    ScanAllFiles();
                }

                GUILayout.FlexibleSpace();
                SearchString = SearchField(SearchString, GUILayout.Width(250));
            }
        }
        
        private void Sidebar()
        {
            using (new VerticalBlock(GUI.skin.box, GUILayout.Width(SidebarWidth), GUILayout.ExpandHeight(true)))
            {
                using (new ScrollviewBlock(ref _sidebarScroll))
                {
                    TagField(-1);

                    for (var i = 0; i < _data.tagsCount; i++)
                    {
                        TagField(i);
                    }
                }

                AddTagField();
            }
        }

        private void MainArea()
        {
            using (new VerticalBlock(GUI.skin.box, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)))
            {
                using (new ScrollviewBlock(ref _mainAreaScroll))
                {
                    for (var i = 0; i < _entriesToShow.Length; i++)
                    {
                        EntryField(i);
                    }
                }
            }
        }

        private void TagField(int index)
        {
            Event e = Event.current;
            string tag = index == -1 ? "All" : _data.tagsList[index];
            using (new HorizontalBlock(EditorStyles.helpBox))
            {
                using (new ColoredBlock(index == _currentTag ? Color.green : Color.white))
                {
                    GUILayout.Label(tag);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(_data.GetCountByTag(index).ToString());
                }
                if (index != -1 && index != 0)
                {
                    if (GUILayout.Button("x", EditorStyles.miniButton))
                    {
                        EditorApplication.delayCall += () =>
                        {
                            _data.RemoveTag(index);
                            SaveData(_data);
                            Repaint();
                        };
                    }
                }
            }

            Rect rect = GUILayoutUtility.GetLastRect();

            if (e.isMouse && e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
            {
                SetCurrentTag(index);
            }
        }

        private void AddTagField()
        {
            using (new HorizontalBlock(EditorStyles.helpBox))
            {
                _newTagName = EditorGUILayout.TextField(_newTagName);
                if (GUILayout.Button("Add", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
                {
                    _data.AddTag(_newTagName);
                    SaveData(_data);
                    _newTagName = "";
                    GUI.FocusControl(null);
                }
            }
        }

        private void EntryField(int index)
        {
            TodoEntry entry = _entriesToShow[index];
            using (new VerticalBlock(EditorStyles.helpBox))
            {
                using (new HorizontalBlock())
                {
                    GUILayout.Label(entry.Text, EditorStyles.largeLabel);
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(entry.PathToShow, EditorStyles.miniBoldLabel);
                }
            }

            Event e = Event.current;
            Rect rect = GUILayoutUtility.GetLastRect();

            if (e.isMouse && e.type == EventType.MouseDown && rect.Contains(e.mousePosition) && e.clickCount == 2)
            EditorApplication.delayCall += () =>
            {
                UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(entry.File, entry.Line);
            };
        } 
        #endregion

        #region Files envents handlers
        private void OnChanged(object obj, FileSystemEventArgs e)
        {
            EditorApplication.delayCall += () => ScanFile(e.FullPath);
        }

        private void OnCreated(object obj, FileSystemEventArgs e)
        {
            EditorApplication.delayCall += () => ScanFile(e.FullPath);
        }

        private void OnDeleted(object obj, FileSystemEventArgs e)
        {
            EditorApplication.delayCall += () =>
            {
                _data.entries.RemoveAll(en => en.File == e.FullPath);
                SaveData(_data);
            };
        }

        private void OnRenamed(object obj, FileSystemEventArgs e)
        {
            EditorApplication.delayCall += () => ScanFile(e.FullPath);
        } 
        #endregion

        #region Files Helpers

        private void ScanAllFiles()
        {
            RefreshFiles();
            foreach (var file in _files.Where(file => file.Exists))
            {
                ScanFile(file.FullName);
            }
        }

        private void ScanFile(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if (!file.Exists)
            {
                return;
            }

            List<TodoEntry> entries = new List<TodoEntry>();
            _data.entries.RemoveAll(e => e.File == filePath);

            ScriptsParser parser = new ScriptsParser(filePath, Tags);

            entries.AddRange(parser.Parse());
            var temp = entries.Except(_data.entries);
            _data.entries.AddRange(temp);

            SaveData(_data);
        }

        private void RefreshFiles()
        {
            if(_data == null)
            {
                _data = new TodoData();
            }

            _data.entries = new List<TodoEntry>();

            var assetsDir = new DirectoryInfo(Application.dataPath);

            _files =
                assetsDir.GetFiles("*.cs", SearchOption.AllDirectories)
                    .Concat(assetsDir.GetFiles("*.js", SearchOption.AllDirectories))
                    .ToArray();
        }

        #endregion

        #region UI helpers

        private void RefreshEntriesToShow()
        {
            if (_currentTag == -1)
            {
                _entriesToShow = _data.entries.ToArray();
            }
            else if (_currentTag >= 0)
            {
                _entriesToShow = _data.entries.Where(e => e.Tag == _data.tagsList[_currentTag]).ToArray();
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                TodoEntry[] temp = _entriesToShow;
                _entriesToShow = temp.Where(e => e.Text.Contains(_searchString)).ToArray();
            }
        }

        private void SetCurrentTag(int index)
        {
            EditorApplication.delayCall += () =>
            {
                _currentTag = index;
                RefreshEntriesToShow();
                Repaint();
            };
        }

        public static string AssetsRelativePath(string absolutePath)
        {
            if (absolutePath.StartsWith(Application.dataPath))
            {
                return "Assets" + absolutePath.Substring(Application.dataPath.Length);
            }
            else
            {
                throw new System.ArgumentException("Full path does not contain the current project's Assets folder", "absolutePath");
            }
        }

        private string SearchField(string searchStr, params GUILayoutOption[] options)
        {
            searchStr = GUILayout.TextField(searchStr, "ToolbarSeachTextField", options);
            if (GUILayout.Button("", "ToolbarSeachCancelButton"))
            {
                searchStr = "";
                GUI.FocusControl(null);
            }
            return searchStr;
        }

        #endregion
    }
}