 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GDD.Helper;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Random = UnityEngine.Random;

namespace GDD
{
    public class RandomSkill : MonoBehaviour
    {
        [SerializeField] private bool is_overrideResourcesPath = true;
        [SerializeField] private string r_skillConfigPath = "Assets/Resources/Resources_Data/SkillConfigPath.asset";
        [SerializeField] private string r_skillUpgradePath = "Assets/Resources/Resources_Data/SkillUpgradePath.asset";
        private WeaponSystem _weaponSystem;
        private WeaponAttachment _weaponAttachment;
        private string[,] _paths;
        private ResourcesPath _skillConfigPath;
        private ResourcesPath _skillUpgradePath;
        private List<Tuple<WeaponConfig, WeaponAttachment, int>> _baseSkills = new List<Tuple<WeaponConfig, WeaponAttachment, int>>();
        private List<Tuple<MainSkillUpgrade, AttachmentSkillUpgrade, int>> _upgradeSkills = new List<Tuple<MainSkillUpgrade, AttachmentSkillUpgrade, int>>();

        public WeaponSystem weaponSystem
        {
            get => _weaponSystem;
            set => _weaponSystem = value;
        }
        
        public List<Tuple<WeaponConfig, WeaponAttachment, int>> baseSkills
        {
            get => _baseSkills;
        }

        public List<Tuple<MainSkillUpgrade, AttachmentSkillUpgrade, int>> upgradeSkills
        {
            get => _upgradeSkills;
        }

        public string skillConfigPath
        {
            get => r_skillConfigPath;
        }

        public string skillUpgradePath
        {
            get => r_skillUpgradePath;
        }
        
        public void OnInitialize()
        {
            /*
            _weaponAttachment = Resources.Load<WeaponAttachment>("Presets/Player/SimplesecondaryWeaponAttachment");

            print("Weapon : " + _weaponAttachment.attachmentName);
*/
#if UNITY_EDITOR
            _paths = ResourcesManager.GetAllResourcePathSeparateFolder("/Resources/Presets/Player");
#endif
            //SkillConfigPath
            _skillConfigPath = Resources.Load<ResourcesPath>("Resources_Data/SkillConfigPath");
            if (_skillConfigPath == null || is_overrideResourcesPath)
            {
                //print("Skill Con Create");
                _skillConfigPath = ScriptableObject.CreateInstance<ResourcesPath>();
                _skillConfigPath.paths = ArrayHelper.GetRow(_paths, 0).Where(x => !string.IsNullOrEmpty(x)).ToList();
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(_skillConfigPath, r_skillConfigPath);
                AssetDatabase.SaveAssets();
#endif
            }
            print("Skill Con = null : " + (_skillConfigPath == null));

            //SkillUpgradePath
            _skillUpgradePath = Resources.Load<ResourcesPath>("Resources_Data/SkillUpgradePath");
            if (_skillUpgradePath == null || is_overrideResourcesPath)
            {
                //print("Skill Up Create");
                _skillUpgradePath = ScriptableObject.CreateInstance<ResourcesPath>();
                _skillUpgradePath.paths = ArrayHelper.GetRow(_paths, 1).Where(x => !string.IsNullOrEmpty(x)).ToList();
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(_skillUpgradePath, r_skillUpgradePath);
                AssetDatabase.SaveAssets();
#endif
            }
            print("Skill Up = null : " + (_skillUpgradePath == null));
        }



        public void OnRandomSkill(RandomSkillType _type, int count)
        {
            int base_count = Random.Range(0, count);
            int upgrade_count = count - base_count;
            print("B : " + base_count + " || UP : " + upgrade_count);
            
            switch (_type)
            {
                case RandomSkillType.All:
                    _baseSkills = RandomBaseSkill(base_count);
                    
                    //Debug
                    foreach (var weaponConfig in _baseSkills)
                    {
                        if(weaponConfig.Item1 != null)
                            print("Weapon Con : " + weaponConfig.Item1.weaponName);
                        else
                            print("Weapon Con : " + weaponConfig.Item2.name);
                    }
                    
                    _upgradeSkills = RandomUpgradeSkill(upgrade_count);
                    
                    //Debug
                    foreach (var upgradeSkill in _upgradeSkills)
                    {
                        if(upgradeSkill.Item1 != null)
                            print("Weapon Up : " + upgradeSkill.Item1.name);
                        else
                            print("Weapon Up : " + upgradeSkill.Item2.name);
                    }
                    break;
                case RandomSkillType.BaseSkill:
                    _baseSkills = RandomBaseSkill(count);
                    break;
                case RandomSkillType.UpgradeSkill:
                    _upgradeSkills = RandomUpgradeSkill(count);
                    break;
            }
        }

        public List<Tuple<WeaponConfig, WeaponAttachment, int>> RandomBaseSkill(int count)
        {
            List<Tuple<WeaponConfig, WeaponAttachment, int>> _weaponConfigs = new List<Tuple<WeaponConfig, WeaponAttachment, int>>();
            List<string> skillpaths = new List<string>();
            List<string> skillpaths_dontRemove = new List<string>();
            
            //Copy Skill Paths
            skillpaths = _skillConfigPath.paths.ToArray().Clone().ConvertTo<string[]>().ToList();
            skillpaths_dontRemove = _skillConfigPath.paths.ToArray().Clone().ConvertTo<string[]>().ToList();
            
            print($"OR : {skillpaths.Count} | CP : {skillpaths_dontRemove.Count}");
            
            for (int i = 0; i < count; i++)
            {
                int i_random = Random.Range(0, skillpaths.Count - 1);
                string path = skillpaths[i_random];
                int ir_path = skillpaths_dontRemove.IndexOf(skillpaths[i_random]);
                
                print($"Random : {i_random}");
                print($"Index In Source Array : {ir_path}");

                WeaponConfig _config = Resources.Load<WeaponConfig>(path);
                if (_config == null)
                    _weaponConfigs.Add(new Tuple<WeaponConfig, WeaponAttachment, int>(null, Resources.Load<WeaponAttachment>(path), ir_path));
                else
                    _weaponConfigs.Add(new Tuple<WeaponConfig, WeaponAttachment, int>(_config, null, ir_path));
                
                skillpaths.RemoveAt(i_random);
                print("Random Count : " + skillpaths.Count);
            }
            return _weaponConfigs;
        }
        
        public List<Tuple<MainSkillUpgrade, AttachmentSkillUpgrade, int>> RandomUpgradeSkill(int count)
        {
            List<Tuple<MainSkillUpgrade, AttachmentSkillUpgrade, int>> _weaponUpgrade = new List<Tuple<MainSkillUpgrade, AttachmentSkillUpgrade, int>>();
            List<string> skillpaths = new List<string>();
            List<string> skillpaths_dontRemove = new List<string>();
            
            //Copy Skill Paths
            skillpaths = _skillUpgradePath.paths.ToArray().Clone().ConvertTo<string[]>().ToList();
            skillpaths_dontRemove = _skillUpgradePath.paths.ToArray().Clone().ConvertTo<string[]>().ToList();
            
            print($"OR : {skillpaths.Count} | CP : {skillpaths_dontRemove.Count}");
            
            for (int i = 0; i < count; i++)
            {
                int i_random = Random.Range(0, skillpaths.Count - 1);
                string path = skillpaths[i_random];
                int ir_path = skillpaths_dontRemove.IndexOf(skillpaths[i_random]);
                
                print($"UPSKILL Random : {i_random}");
                print($"UPSKILL Index In Source Array : {ir_path}");

                MainSkillUpgrade _upgrade = Resources.Load<MainSkillUpgrade>(path);
                if (_upgrade == null)
                    _weaponUpgrade.Add(new Tuple<MainSkillUpgrade, AttachmentSkillUpgrade, int>(null, Resources.Load<AttachmentSkillUpgrade>(path), ir_path));
                else
                    _weaponUpgrade.Add(new Tuple<MainSkillUpgrade, AttachmentSkillUpgrade, int>(_upgrade, null, ir_path));
                
                skillpaths.RemoveAt(i_random);
                print("Ramdom Count : " + skillpaths.Count);
            }
            return _weaponUpgrade;
        }
    }
}