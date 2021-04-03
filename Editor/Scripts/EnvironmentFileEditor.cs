// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CandyCoded.env.Editor
{

    public class EnvironmentFileEditor : EditorWindow
    {

        private Vector2 scrollPosition;

        private List<Tuple<string, string>> tempConfig = new List<Tuple<string, string>>();

        private void Update()
        {

            if (EditorApplication.isPlaying && !EditorApplication.isPaused)
            {

                Repaint();

            }

        }

        private void OnEnable()
        {

            LoadEnvironmentFile();

        }

        private void OnGUI()
        {

            if (!File.Exists(env.editorFilePath))
            {

                if (GUILayout.Button("Create .env File"))
                {

                    try
                    {

                        File.WriteAllText(env.editorFilePath,
                            env.SerializeEnvironmentDictionary(
                                new Dictionary<string, string> { { "VARIABLE_NAME", "VALUE" } }));

                        LoadEnvironmentFile();

                    }
                    catch (Exception err)
                    {

                        EditorUtility.DisplayDialog("Error", err.Message, "Ok");

                    }

                }

                return;

            }

            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            for (var i = 0; i < tempConfig.Count; i += 1)
            {

                GUILayout.BeginHorizontal();

                var key = GUILayout.TextField(tempConfig[i].Item1, GUILayout.ExpandWidth(false),
                    GUILayout.Width(position.width * 0.25f));

                var value = GUILayout.TextField(tempConfig[i].Item2, GUILayout.ExpandWidth(true));

                if (tempConfig.Count == 1)
                {

                    GUI.enabled = false;

                }

                if (GUILayout.Button("-", GUILayout.ExpandWidth(false)))
                {

                    tempConfig.RemoveAt(i);

                    continue;

                }

                if (tempConfig.Count == 1)
                {

                    GUI.enabled = true;

                }

                if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                {

                    tempConfig.Insert(i + 1, new Tuple<string, string>("", ""));

                    continue;

                }

                GUILayout.EndHorizontal();

                if (!key.Equals(tempConfig[i].Item1) || !value.Equals(tempConfig[i].Item2))
                {

                    tempConfig[i] = new Tuple<string, string>(key, value);

                }

            }

            if (GUILayout.Button("Save Changes"))
            {

                try
                {

                    File.WriteAllText(env.editorFilePath, env.SerializeEnvironmentDictionary(
                        tempConfig.ToDictionary(item => item.Item1, item => item.Item2)));

                }
                catch (Exception err)
                {

                    EditorUtility.DisplayDialog("Error", err.Message, "Ok");

                }

            }

            if (GUILayout.Button("Revert Changes"))
            {

                LoadEnvironmentFile();

            }

            if (GUILayout.Button("Delete .env File"))
            {

                if (EditorUtility.DisplayDialog("Confirm", $"Delete {env.editorFilePath}?", "Ok", "Cancel"))
                {

                    File.Delete(env.editorFilePath);

                }

            }

            GUILayout.EndScrollView();

        }

        [MenuItem("Window/CandyCoded/Environment File Editor")]
        public static void ShowWindow()
        {

            GetWindow(typeof(EnvironmentFileEditor), false, "Environment File Editor", true);

        }

        private void LoadEnvironmentFile()
        {

            if (File.Exists(env.editorFilePath))
            {

                tempConfig = env.ParseEnvironmentFile().Select(item => new Tuple<string, string>(item.Key, item.Value))
                    .ToList();

            }

        }

    }

}
#endif
