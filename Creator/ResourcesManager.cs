﻿
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InventoryQuest.Utils;

namespace Creator
{
    class ResourcesManager
    {

        /// <summary>
        /// List of all possible root path to look in.
        /// </summary>
        public static readonly string[] ROOT_PATHS = new string[] { @"..\..\..\Assets\Resources" };


        /// <summary>
        /// List of all supported gfx files extensions.
        /// </summary>
        public static readonly string[] GFX_EXTENSIONS = new string[] { "png", "jpg" };

        /// <summary>
        /// List of all supported sfx files extensions.
        /// </summary>
        public static readonly string[] SFX_EXTENSIONS = new string[] { "mp3", "wav" };

        private static string _PathRoot = "";
        /// <summary>
        /// Root path to directory with all resources.
        /// </summary>
        public static string PathRoot
        {
            get { return ResourcesManager._PathRoot; }
            private set { ResourcesManager._PathRoot = value; }
        }

        private static string _PathGfx = "Sprites";
        /// <summary>
        /// Path to directory inside root path to directory with all gfx resources.
        /// </summary>
        public static string PathGfx
        {
            get { return ResourcesManager._PathGfx; }
            private set { ResourcesManager._PathGfx = value; }
        }

        private static string _PathSfx = "Sounds";
        /// <summary>
        /// Path to directory inside root path to directory with all gfx resources.
        /// </summary>
        public static string PathSfx
        {
            get { return ResourcesManager._PathSfx; }
            private set { ResourcesManager._PathSfx = value; }
        }

        private static string _PathGfxGUI = "gui";
        /// <summary>
        /// Path to directory inside gfx path with all GUI graphics.
        /// </summary>
        public static string PathGfxGUI
        {
            get { return ResourcesManager._PathGfxGUI; }
            private set { ResourcesManager._PathGfxGUI = value; }
        }

        private static string _PathItems = "items";
        /// <summary>
        /// Path to directory inside gfx path with all items graphics.
        /// </summary>
        public static string PathItems
        {
            get { return ResourcesManager._PathItems; }
            set { ResourcesManager._PathItems = value; }
        }

        private static bool _FoundResourcesFolder = false;

        /// <summary>
        /// Was resources folder found successfully.
        /// </summary>
        public static bool FoundResourcesFolder
        {
            get { return ResourcesManager._FoundResourcesFolder; }
            private set { ResourcesManager._FoundResourcesFolder = value; }
        }

        private static int _FoundResourcesGUI = 0;
        /// <summary>
        /// How many resources was used that was found in GUI folder
        /// </summary>
        public static int FoundResourcesGUI
        {
            get { return ResourcesManager._FoundResourcesGUI; }
            private set { ResourcesManager._FoundResourcesGUI = value; }
        }

        private static int _FoundResourcesItems = 0;
        /// <summary>
        /// How many resources was used that was found in Item folder
        /// </summary>
        public static int FoundResourcesItems
        {
            get { return ResourcesManager._FoundResourcesItems; }
            private set { ResourcesManager._FoundResourcesItems = value; }
        }

        /// <summary>
        /// Initialize the resource manager.
        /// </summary>
        static ResourcesManager()
        {
            PathRoot = System.IO.Directory.GetCurrentDirectory();

            FindRoot();
        }

        /// <summary>
        /// Find root directory of resources.
        /// </summary>
        private static void FindRoot(bool isGFX = true)
        {
            string lookFor;
            if (isGFX)
            {
                // Use gfx folder as default existing directory
                lookFor = PathGfx;
            }
            else
            {
                lookFor = PathSfx;
            }

            for (int i = 0; i < ROOT_PATHS.Length; i++)
            {
                string path = Path.Combine(PathRoot, ROOT_PATHS[i], lookFor);
                if (Directory.Exists(path))
                {
                    PathRoot = Path.Combine(PathRoot, ROOT_PATHS[i]);
                    FoundResourcesFolder = true;

                    break;
                }
            }
        }


        /// <summary>
        /// Returns every file using given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string[] GetAllFiles(string path, bool searchFiles = true, bool isGFX = true)
        {
            string fullPath = Path.Combine(PathRoot, isGFX ? PathGfx : PathSfx, PathItems, path);
            Console.WriteLine(Path.GetFullPath(fullPath));
            string extension = "";
            List<string> files = new List<string>();
            if (searchFiles)
            {
                if (isGFX)
                {
                    for (int i = 0; i < GFX_EXTENSIONS.Length; i++)
                    {
                        extension = @"*." + GFX_EXTENSIONS[i];

                        files.AddRange(Directory.GetFiles(fullPath, extension));
                    }
                }
                else
                {
                    for (int i = 0; i < SFX_EXTENSIONS.Length; i++)
                    {
                        extension = @"*." + SFX_EXTENSIONS[i];

                        files.AddRange(Directory.GetFiles(fullPath, extension));
                    }
                }
            }
            else
            {
                var str = Directory.GetDirectories(fullPath);
                for (int i = 0; i < str.Count(); i++)
                {
                    str[i] = new DirectoryInfo(str[i]).Name;
                }
                files.AddRange(str);
            }
            return files.ToArray();
        }
    }
}
