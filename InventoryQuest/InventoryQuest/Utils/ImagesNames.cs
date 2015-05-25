using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Debug = UnityEngine.Debug;

namespace InventoryQuest.Utils
{
    public static class ImagesNames
    {
        public static List<NamedList<string>> ItemsImageNames;

        /// <summary>
        ///     List of all supported gfx files extensions.
        /// </summary>
        public static readonly string[] GFX_EXTENSIONS = { "png", "jpg" };

        /// <summary>
        ///     List of all possible root path to look in.
        /// </summary>
        public static readonly string[] ROOT_PATHS = { "", "../", "../../" };

        private static string _PathRoot = "";
        private static string _PathGfx = "gfx";
        private static string _PathGfxGUI = "gui";
        private static string _PathGfxItems = "items";

        static ImagesNames()
        {
            Init();
            ItemsImageNames = GetAllImagesNames();
        }

        /// <summary>
        ///     Root path to directory with all resources.
        /// </summary>
        public static string PathRoot
        {
            get { return _PathRoot; }
            private set { _PathRoot = value; }
        }

        /// <summary>
        ///     Path to directory inside root path to directory with all gfx resources.
        /// </summary>
        public static string PathGfx
        {
            get { return _PathGfx; }
            private set { _PathGfx = value; }
        }

        /// <summary>
        ///     Path to directory inside gfx path with all GUI graphics.
        /// </summary>
        public static string PathGfxGUI
        {
            get { return _PathGfxGUI; }
            private set { _PathGfxGUI = value; }
        }

        /// <summary>
        ///     Path to directory inside gfx path with all items graphics.
        /// </summary>
        public static string PathGfxItems
        {
            get { return _PathGfxItems; }
            set { _PathGfxItems = value; }
        }

        /// <summary>
        ///     Was resources folder found successfully.
        /// </summary>
        public static bool FoundResourcesFolder { get; private set; }

        /// <summary>
        ///     How many resources was used that was found in GUI folder
        /// </summary>
        public static int FoundResourcesGUI { get; private set; }

        /// <summary>
        ///     How many resources was used that was found in Item folder
        /// </summary>
        public static int FoundResourcesItems { get; private set; }

        public static List<NamedList<string>> GetAllImagesNames()
        {
            ItemsImageNames = new List<NamedList<string>>();
            List<string> names = GetAllFiles("/Sprites/items", false).ToList();
            names.RemoveAt(names.Count - 1);
            List<string> weaponsNames = GetAllFiles("/Sprites/items/weapon", false).ToList();
            for (var i = 0; i < names.Count; i++)
            {
                var nameList = new NamedList<string>(names[i]);
                var images = GetAllFiles("/Sprites/items/" + names[i]);
                foreach (var item in images)
                {
                    nameList.List.Add(Path.GetFileNameWithoutExtension(item));
                    nameList.FullNameList.Add(Path.GetDirectoryName(item) + "/" + Path.GetFileNameWithoutExtension(item));
                }
                ItemsImageNames.Add(nameList);
            }
            for (var i = 0; i < weaponsNames.Count; i++)
            {
                var nameList = new NamedList<string>(weaponsNames[i]);
                foreach (var item in GetAllFiles("/Sprites/items/weapon/" + weaponsNames[i]))
                {
                    nameList.List.Add(Path.GetFileNameWithoutExtension(item));
                    nameList.FullNameList.Add(Path.GetDirectoryName(item) + "/" + Path.GetFileNameWithoutExtension(item));
                }
                ItemsImageNames.Add(nameList);
            }
            return ItemsImageNames;
        }

        public static ImageIDPair ResolveImage(string type, string item)
        {
            if (ItemsImageNames.Count != 0)
            {
                var image = new ImageIDPair();
                image.ImageIDType = ItemsImageNames.FindIndex(x => x.Name == type);

                if (ItemsImageNames[image.ImageIDType].List.Count != 0)
                {
                    image.ImageIDItem = ItemsImageNames[image.ImageIDType].List.FindIndex(x => x == item);
                    return image;
                }
                return new ImageIDPair(image.ImageIDType, -1);
            }
            return new ImageIDPair(-1, -1);
        }

        /// <summary>
        ///     Initialize the resource manager.
        /// </summary>
        public static void Init()
        {

        }

        public static string[] GetAllFiles(string path, bool searchFiles = true)
        {
            var fullPath = FileUtility.GetResourcesDirectories();
            var extension = "";
            var files = new List<string>();
            if (searchFiles)
            {
                foreach (var localpath in fullPath)
                {
                    files.AddRange(Directory.GetFiles(localpath + path));
                }
            }
            else
            {
                foreach (var localpath in fullPath)
                {
                    string[] str = Directory.GetDirectories(localpath + path);
                    for (var i = 0; i < str.Count(); i++)
                    {
                        str[i] = new DirectoryInfo(str[i]).Name;
                    }
                    files.AddRange(str);
                }
            }
            return files.ToArray();
        }

        [Serializable]
        public class NamedList<T>
        {
            public NamedList()
            {
            }

            public NamedList(string name)
            {
                List = new List<T>();
                FullNameList = new List<T>();
                Name = name;
            }

            public string Name { get; set; }
            public List<T> List { get; set; }
            public List<T> FullNameList { get; set; }
        }
    }
}