using System;
using System.IO;
using GDD.Helper;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace GDD
{
    public static class ResourcesManager
    {
        // "/Resources/Presets/Player"
#if UNITY_EDITOR
        public static string[] GetAllResourcePath(string path)
        {
            var info = new DirectoryInfo(Application.dataPath + path);
            var folder = info.GetDirectories("**");

            string resourcePath = path.Remove(0, 11);
            resourcePath += "/";

            string[] _allpaths = new string[0];
            int r_index = 0;
            foreach (var dir in folder)
            {
                //Debug.Log("Dir : " + dir.Name);
                //Debug.Log("R Dir " + resourcePath);

                var R_Data = Resources.LoadAll(resourcePath + dir.Name);
                Array.Resize(ref _allpaths, _allpaths.Length + R_Data.Length);
                //Debug.Log("Path Size : " + _allpaths.Length);


                for (int i = 0; i < R_Data.Length; i++)
                {
                    var assetPath = AssetDatabase.GetAssetPath(R_Data[i]);
                    _allpaths[r_index] = assetPath;
                    r_index++;
                    // "/Resources/Presets/Player/" + dir.Name + "/" + r.name + r.GetType()
                }
            }

            return _allpaths;
        }

        public static string[,] GetAllResourcePathSeparateFolder(string path)
        {
            var info = new DirectoryInfo(Application.dataPath + path);
            var folder = info.GetDirectories("**");
            short index_length = 0;

            string resourcePath = path.Remove(0, 11);
            resourcePath += "/";

            string[,] _allpaths = new string[folder.Length, 0];

            for (int f_index = 0; f_index < folder.Length; f_index++)
            {
                //Debug.Log("Dir : " + dir.Name);
                //Debug.Log("R Dir " + resourcePath);

                var R_Data = Resources.LoadAll(resourcePath + folder[f_index].Name);

                if (index_length < R_Data.Length)
                {
                    index_length = (short)R_Data.Length;
                    _allpaths = (string[,])ArrayHelper.ResizeArray(_allpaths, new int[] { folder.Length, index_length});
                }
                //Array.Resize(ref _allpaths, _allpaths.Length + R_Data.Length);
                //Debug.Log("Path Size : " + _allpaths.Length);


                for (int i = 0; i < R_Data.Length; i++)
                {
                    var assetPath = AssetDatabase.GetAssetPath(R_Data[i]);
                    _allpaths[f_index, i] = assetPath;
                    // "/Resources/Presets/Player/" + dir.Name + "/" + r.name + r.GetType()
                }
            }

            return _allpaths;
        }
#endif
    }
}