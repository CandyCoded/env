// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CandyCoded.env.Editor
{

    public class BuildTools : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {

        private bool _resourcesDirCreated;

        public void OnPostprocessBuild(BuildReport report)
        {

            FileUtil.DeleteFileOrDirectory(_resourcesDirCreated ? env.resourcesDirPath : env.runtimeFilePath);

            _resourcesDirCreated = false;

        }

        public int callbackOrder { get; }

        public void OnPreprocessBuild(BuildReport report)
        {

            if (!Directory.Exists(env.resourcesDirPath))
            {

                Directory.CreateDirectory(env.resourcesDirPath);

                _resourcesDirCreated = true;

            }
            
            if (File.Exists(env.runtimeFilePath))
            {   
                
                Debug.LogWarning($"{env.runtimeFilePath} already exists, using the existing file rather than copying current env.");
                
            }
            else if (File.Exists(env.editorFilePath))
            {

                FileUtil.CopyFileOrDirectory(env.editorFilePath, env.runtimeFilePath);

            }

        }

    }

}
