using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace InventoryQuest.Utils
{
    public static class ResourcesNames
    {
        public static List<NamedList<string>> ItemsImageNames;
        public static List<NamedList<string>> ItemsSoundsNames;
        public static List<NamedList<string>> MonstersImageNames;

        /// <summary>
        ///     List of all possible root path to look in.
        /// </summary>
        public static readonly string[] ROOT_PATHS = { "", "../", "../../" };

        private static string _PathRoot = "";
        private static string _PathGfx = "gfx";
        private static string _PathGfxGUI = "gui";
        private static string _PathGfxItems = "items";

        static ResourcesNames()
        {
            Init();
#if CREATOR
            ItemsImageNames = GetAllImagesNames();
            MonstersImageNames = GetAllMonstersNames();
            ItemsSoundsNames = GetAllItemsSoundNames();
#else
            if (Application.isEditor)
            {
                ItemsImageNames = GetAllImagesNames();
                MonstersImageNames = GetAllMonstersNames();
                ItemsSoundsNames = GetAllItemsSoundNames();
                FileList.Instance.ItemsImageNames = ItemsImageNames;
                FileList.Instance.MonstersImageNames = MonstersImageNames;
                FileList.Instance.ItemsSoundsNames = ItemsSoundsNames;
                FileList.Save();
            }
            else
            {
                ItemsImageNames = FileList.Instance.ItemsImageNames;
                MonstersImageNames = FileList.Instance.MonstersImageNames;
                ItemsSoundsNames = FileList.Instance.ItemsSoundsNames;

            }
#endif
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
#if !CREATOR
            foreach (var imageName in ItemsImageNames)
            {
                for (int i = 0; i < imageName.FullNameList.Count; i++)
                {
                    imageName.FullNameList[i] = FileUtility.AssetsRelativePath(imageName.FullNameList[i]);
                }
            }
#endif
            return ItemsImageNames;
        }

        public static List<NamedList<string>> GetAllMonstersNames()
        {
            var ImageNames = new List<NamedList<string>>();
            List<string> names = GetAllFiles("/Sprites/portraits", false).ToList();
            for (var i = 0; i < names.Count; i++)
            {
                var nameList = new NamedList<string>(names[i]);
                var images = GetAllFiles("/Sprites/portraits/" + names[i]);
                foreach (var item in images)
                {
                    nameList.List.Add(Path.GetFileNameWithoutExtension(item));
                    nameList.FullNameList.Add(Path.GetDirectoryName(item) + "/" + Path.GetFileNameWithoutExtension(item));
                }
                ImageNames.Add(nameList);
            }
#if !CREATOR
            foreach (var imageName in ImageNames)
            {
                for (int i = 0; i < imageName.FullNameList.Count; i++)
                {
                    imageName.FullNameList[i] = FileUtility.AssetsRelativePath(imageName.FullNameList[i]);
                }
            }
#endif
            return ImageNames;
        }

        public static List<NamedList<string>> GetAllItemsSoundNames()
        {
            var ImageNames = new List<NamedList<string>>();
            List<string> names = GetAllFiles("/Sounds", false).ToList();
            for (var i = 0; i < names.Count; i++)
            {
                var nameList = new NamedList<string>(names[i]);
                var images = GetAllFiles("/Sounds/" + names[i]);
                foreach (var item in images)
                {
                    nameList.List.Add(Path.GetFileNameWithoutExtension(item));
                    nameList.FullNameList.Add(Path.GetDirectoryName(item) + "/" + Path.GetFileNameWithoutExtension(item));
                }
                ImageNames.Add(nameList);
            }
#if !CREATOR
            foreach (var imageName in ImageNames)
            {
                for (int i = 0; i < imageName.FullNameList.Count; i++)
                {
                    imageName.FullNameList[i] = FileUtility.AssetsRelativePath(imageName.FullNameList[i]);
                }
            }
#endif
            return ImageNames;
        }

        public static ImageIDPair ResolveItemsImage(string type, string item)
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

        public static ImageIDPair ResolveMonstersImage(string type, string item)
        {
            if (MonstersImageNames.Count != 0)
            {
                var image = new ImageIDPair();
                image.ImageIDType = MonstersImageNames.FindIndex(x => x.Name == type);

                if (MonstersImageNames[image.ImageIDType].List.Count != 0)
                {
                    image.ImageIDItem = MonstersImageNames[image.ImageIDType].List.FindIndex(x => x == item);
                    return image;
                }
                return new ImageIDPair(image.ImageIDType, -1);
            }
            return new ImageIDPair(-1, -1);
        }

        public static ImageIDPair ResolveItemsSound(string type, string item)
        {
            if (ItemsSoundsNames.Count != 0)
            {
                var image = new ImageIDPair();
                image.ImageIDType = ItemsSoundsNames.FindIndex(x => x.Name == type);

                if (ItemsSoundsNames[image.ImageIDType].List.Count != 0)
                {
                    image.ImageIDItem = ItemsSoundsNames[image.ImageIDType].List.FindIndex(x => x == item);
                    return image;
                }
                return new ImageIDPair(image.ImageIDType, -1);
            }
            return new ImageIDPair(-1, -1);
        }

        /// <summary>
        ///     Initialize the resource manager.
        /// </summary>
        static void Init()
        {
#if CREATOR
            PathRoot = Directory.GetCurrentDirectory();
            string unityAssetsPath = "..\\..\\..\\Assets\\Resources";
            PathRoot = Path.GetFullPath(Path.Combine(PathRoot, unityAssetsPath));
#endif
        }

        public static string[] GetAllFiles(string path, bool searchFiles = true)
        {
#if CREATOR
            string[] fullPath = { PathRoot };
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
            files.RemoveAll(x => x.EndsWith("meta"));
            return files.ToArray();

#else
            var fullPath = FileUtility.GetResourcesDirectories();
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
#endif
        }

        [Serializable]
        public class FileList
        {
            public static FileList Instance { get; private set; }

            static FileList()
            {
                if (!Application.isEditor)
                {
                    var data = Load();
                    Instance = data;
                }
                else
                {
                    Instance = new FileList();
                }
            }

            public List<NamedList<string>> ItemsImageNames;
            public List<NamedList<string>> ItemsSoundsNames;
            public List<NamedList<string>> MonstersImageNames;


            public static void Save()
            {
                BinaryFilesOperations.Save(Instance, "Assets/StreamingAssets/resources.fl");
            }

            static FileList Load()
            {
                return BinaryFilesOperations.Load<FileList>("InventoryQuest_Data/StreamingAssets/resources.fl");
            }
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