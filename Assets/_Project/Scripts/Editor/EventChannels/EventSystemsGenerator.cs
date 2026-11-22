using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assemblies;

namespace ShroomDefense.Editor.EventChannels
{
    public class EventSystemGenerator : EditorWindow
    {
        private const string ASSET_MENU_PATH = "Events";
        private string _typeName = "";
        private string _assemblyName = "";
        private string _selectedType = "";
        private Assembly _selectedAssembly;

        private List<string> _availableTypes = new List<string>();

        private List<Assembly> _availableAssemblies = new List<Assembly>();
        private Dictionary<string, Assembly> _convertedAvailableAssemblies = new();


        private Vector2 _typeScrollPosition;
        private Vector2 _assemblyScrollPosition;

        private GUIStyle _listButtonStyle;

        [MenuItem("Tools/Selivura/Event systems/Generate Event Scripts")]
        public static void ShowWindow()
        {
            GetWindow<EventSystemGenerator>("Event System Generator");
        }

        private void OnEnable()
        {
            RefreshAvailableAssemblies();
            _listButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
            _listButtonStyle.alignment = TextAnchor.MiddleLeft;
        }

        private void RefreshAvailableAssemblies()
        {
            _availableAssemblies.Clear();

            var assemblies = CurrentAssemblies.GetLoadedAssemblies();

            _convertedAvailableAssemblies.Clear();
            foreach (var assembly in assemblies)
            {
                _convertedAvailableAssemblies.Add(assembly.GetName().Name, assembly);
            }

            _availableAssemblies.AddRange(assemblies);
        }

        private void RefreshAvailableTypes(Assembly assembly)
        {
            _availableTypes.Clear();

            var allTypes = assembly.GetTypes()
                .Select(type => type.FullName)
                .OrderBy(name => name)
                .ToList();

            _availableTypes.AddRange(allTypes);
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            GUILayout.Label("Select assembly first, then select type to generate EventChannel and EventListener scripts");
            //ASSEMBLY SEARCH
            string typedAssembly = EditorGUILayout.TextField("Search Assembly", _assemblyName);
            _assemblyName = typedAssembly;

            List<string> filteredAssemblies = new();

            if (_assemblyName == "")
            {
                filteredAssemblies = _convertedAvailableAssemblies.Keys.ToList();
            }
            else
            {
                filteredAssemblies =
                    _convertedAvailableAssemblies.Keys.Where(k => k.Contains(_assemblyName, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            _assemblyScrollPosition
               = EditorGUILayout.BeginScrollView(_assemblyScrollPosition, GUILayout.ExpandHeight(true), GUILayout.MinHeight(200));

            foreach (var assembly in filteredAssemblies)
            {
                if (GUILayout.Button(assembly, _listButtonStyle))
                {
                    if(_convertedAvailableAssemblies.TryGetValue(assembly, out _selectedAssembly))
                    {
                        RefreshAvailableTypes(_selectedAssembly);
                    }
                }
            }

            EditorGUILayout.EndScrollView();
            if (GUILayout.Button("Refresh Assembly List"))
            {
                RefreshAvailableAssemblies();
            }

            EditorGUILayout.Space();
            if (_selectedAssembly == null)
            {
                return;
            }
            EditorGUILayout.Space();
            GUILayout.Label($"Selected Assembly: {_selectedAssembly.GetName().Name}", EditorStyles.wordWrappedLabel);
            EditorGUILayout.Space();

            //TYPE SEARCH
            _typeName = EditorGUILayout.TextField("Search Type", _typeName);
            List<string> filteredTypes = new();

            EditorGUILayout.Space();

            if (string.IsNullOrEmpty(_typeName))
            {
                filteredTypes = _availableTypes;
            }
            else
            {
                filteredTypes = _availableTypes.Where(t => t.IndexOf(_typeName, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            }

            _typeScrollPosition = EditorGUILayout.BeginScrollView(_typeScrollPosition, GUILayout.ExpandHeight(true), GUILayout.MinHeight(200));

            foreach (var type in filteredTypes)
            {
                if (GUILayout.Button(type, _listButtonStyle))
                {
                    _selectedType = type;
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
            _selectedType = EditorGUILayout.TextField($"Generate for Type: ", _selectedType);
            GUILayout.Label($"Generated scripts will be located in:", EditorStyles.boldLabel);
            GUILayout.Label(GetSelectedFolderPath());
            GUILayout.Label("To change this path select folder in Project window", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            GUI.enabled = !string.IsNullOrEmpty(_selectedType);
            if (GUILayout.Button("Generate Event Scripts"))
            {
                GenerateScripts(_selectedType);
            }
            GUI.enabled = true;

            EditorGUILayout.Space();
            if (GUILayout.Button("Refresh Types List"))
            {
                RefreshAvailableTypes(_selectedAssembly);
            }
        }

        private void GenerateScripts(string fullTypeName)
        {
            try
            {
                string typeName = fullTypeName.Contains('.')
                    ? fullTypeName.Substring(fullTypeName.LastIndexOf('.') + 1)
                    : fullTypeName;

                string namespaceName = fullTypeName.Contains('.')
                    ? fullTypeName.Substring(0, fullTypeName.LastIndexOf('.'))
                    : "";

                GenerateEventChannelScript(typeName, namespaceName, fullTypeName);
                GenerateEventListenerScript(typeName, namespaceName, fullTypeName);

                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("Success",
                    $"Generated EventChannel{typeName} and EventListener{typeName} scripts!", "OK");
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("Error",
                    $"Failed to generate scripts: {e.Message}", "OK");
            }
        }

        private void GenerateEventChannelScript(string typeName, string namespaceName, string fullTypeName)
        {
            string scriptContent;
            if (EditorSettings.projectGenerationRootNamespace != "")
            {
                scriptContent =
                $@"using System.Collections.Generic;
                using UnityEngine;
                using SelivuraLib.EventChannels;

                namespace {EditorSettings.projectGenerationRootNamespace}
                {{
                    [CreateAssetMenu(menuName = ""{ASSET_MENU_PATH}/{typeName} EventChannel"", fileName = ""{typeName}EventChannel"")]
                    public class {typeName}EventChannel : EventChannel<{fullTypeName}>  {{}}
                }}";
            }
            else
            {
                scriptContent =
                $@"using System.Collections.Generic;
                using UnityEngine;
                using SelivuraLib.EventChannels;

                [CreateAssetMenu(menuName = ""{ASSET_MENU_PATH}/{typeName} EventChannel"", fileName = ""{typeName}EventChannel"")]
                public class {typeName}EventChannel : EventChannel<{fullTypeName}>  {{}}
                ";
            }

            string path = Path.Combine(GetSelectedFolderPath(), $"{typeName}EventChannel.cs");
            File.WriteAllText(path, scriptContent);
        }

        private void GenerateEventListenerScript(string typeName, string namespaceName, string fullTypeName)
        {
            string scriptContent;
            if (EditorSettings.projectGenerationRootNamespace != "")
            {
                scriptContent =
                $@"using UnityEngine;
                using UnityEngine.Events;
                using SelivuraLib.EventChannels;

                namespace {EditorSettings.projectGenerationRootNamespace}
                {{
                    public class {typeName}EventListener : EventListener<{fullTypeName}> {{}}
                }}";
            }
            else
            {
                scriptContent =
             $@"using UnityEngine;
                using UnityEngine.Events;
                using SelivuraLib.EventChannels;
                
                public class {typeName}EventListener : EventListener<{fullTypeName}> {{}}";
            }

            string path = Path.Combine(GetSelectedFolderPath(), $"{typeName}EventListener.cs");
            File.WriteAllText(path, scriptContent);
        }

        private string GetSelectedFolderPath()
        {
            if (Selection.assetGUIDs.Length > 0)
            {
                string selectedPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);

                if (File.Exists(Path.Combine(Application.dataPath, "..", selectedPath)))
                {
                    return Path.GetDirectoryName(selectedPath);
                }

                else if (Directory.Exists(Path.Combine(Application.dataPath, "..", selectedPath)))
                {
                    return selectedPath;
                }
            }

            return "Assets";
        }
    }
}