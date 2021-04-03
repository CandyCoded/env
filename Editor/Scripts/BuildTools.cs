// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

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

            if (File.Exists(env.runtimeFilePath))
            {

                throw new Exception($"{env.runtimeFilePath} already exists. Remove this file before continuing.");

            }

            if (!Directory.Exists(env.resourcesDirPath))
            {

                Directory.CreateDirectory(env.resourcesDirPath);

                _resourcesDirCreated = true;

            }

            FileUtil.CopyFileOrDirectory(env.editorFilePath, env.runtimeFilePath);

        }

    }

}
