using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GDD.Helper;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace GDD
{
    public class RandomSkill : MonoBehaviour
    {
        private WeaponSystem _weaponSystem;
        private WeaponAttachment _weaponAttachment;
        private string[,] _paths;
        
        private void Start()
        {
            _weaponSystem = GetComponent<WeaponSystem>();
            /*
            _weaponAttachment = Resources.Load<WeaponAttachment>("Presets/Player/SimplesecondaryWeaponAttachment");
            
            print("Weapon : " + _weaponAttachment.attachmentName);
*/
            _paths = ResourcesManager.GetAllResourcePathSeparateFolder("/Resources/Presets/Player");
            
            //SkillConfigPath
            ResourcesPath _skillConfigPath = ScriptableObject.CreateInstance<ResourcesPath>();
            _skillConfigPath.paths = ArrayHelper.GetRow(_paths, 0).Where(x => !string.IsNullOrEmpty(x)).ToList();
            AssetDatabase.CreateAsset(_skillConfigPath, "Assets/Resources/Resources_Data/SkillConfigPath.asset");
            
            //SkillUpgradePath
            ResourcesPath _skillUpgradePath = ScriptableObject.CreateInstance<ResourcesPath>();
            _skillUpgradePath.paths = ArrayHelper.GetRow(_paths, 1).Where(x => !string.IsNullOrEmpty(x)).ToList();
            AssetDatabase.CreateAsset(_skillUpgradePath, "Assets/Resources/Resources_Data/SkillUpgradePath.asset");
        }

        private void ApplySkill()
        {
            
        }
    }
}