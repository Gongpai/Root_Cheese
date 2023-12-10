using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GDD.Helper;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GDD
{
    public class RandomSkill : MonoBehaviour
    {
        [SerializeField] private bool is_overrideResourcesPath = true;
        private WeaponSystem _weaponSystem;
        private WeaponAttachment _weaponAttachment;
        private string[,] _paths;
        private string r_skillConfigPath = "Assets/Resources/Resources_Data/SkillConfigPath.asset";
        private string r_skillUpgradePath = "Assets/Resources/Resources_Data/SkillUpgradePath.asset";
        private ResourcesPath _skillConfigPath;
        private ResourcesPath _skillUpgradePath;
        private List<Tuple<WeaponConfig, WeaponAttachment>> _baseSkills = new List<Tuple<WeaponConfig, WeaponAttachment>>();
        private List<Tuple<MainSkillUpgrade, AttachmentSkillUpgrade>> _upgradeSkills = new List<Tuple<MainSkillUpgrade, AttachmentSkillUpgrade>>();

        public List<Tuple<WeaponConfig, WeaponAttachment>> baseSkills
        {
            get => _baseSkills;
        }

        public List<Tuple<MainSkillUpgrade, AttachmentSkillUpgrade>> upgradeSkills
        {
            get => _upgradeSkills;
        }
        
        private void OnEnable()
        {
            _weaponSystem = GetComponent<WeaponSystem>();
            /*
            _weaponAttachment = Resources.Load<WeaponAttachment>("Presets/Player/SimplesecondaryWeaponAttachment");

            print("Weapon : " + _weaponAttachment.attachmentName);
*/
            _paths = ResourcesManager.GetAllResourcePathSeparateFolder("/Resources/Presets/Player");

            //SkillConfigPath
            _skillConfigPath = Resources.Load<ResourcesPath>("Resources_Data/SkillConfigPath");
            if (_skillConfigPath == null || is_overrideResourcesPath)
            {
                //print("Skill Con Create");
                _skillConfigPath = ScriptableObject.CreateInstance<ResourcesPath>();
                _skillConfigPath.paths = ArrayHelper.GetRow(_paths, 0).Where(x => !string.IsNullOrEmpty(x)).ToList();
                AssetDatabase.CreateAsset(_skillConfigPath, r_skillConfigPath);
                AssetDatabase.SaveAssets();
            }
            print("Skill Con = null : " + (_skillConfigPath == null));

            //SkillUpgradePath
            _skillUpgradePath = Resources.Load<ResourcesPath>("Resources_Data/SkillUpgradePath");
            if (_skillUpgradePath == null || is_overrideResourcesPath)
            {
                //print("Skill Up Create");
                _skillUpgradePath = ScriptableObject.CreateInstance<ResourcesPath>();
                _skillUpgradePath.paths = ArrayHelper.GetRow(_paths, 1).Where(x => !string.IsNullOrEmpty(x)).ToList();
                AssetDatabase.CreateAsset(_skillUpgradePath, r_skillUpgradePath);
                AssetDatabase.SaveAssets();
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

        public List<Tuple<WeaponConfig, WeaponAttachment>> RandomBaseSkill(int count)
        {
            List<Tuple<WeaponConfig, WeaponAttachment>> _weaponConfigs = new List<Tuple<WeaponConfig, WeaponAttachment>>();
            List<string> skillpaths = new List<string>();
            skillpaths = _skillConfigPath.paths.ToArray().Clone().ConvertTo<string[]>().ToList();
            //Array.Copy(_skillConfigPath.paths.ToArray(), skillpaths.ToArray(), _skillConfigPath.paths.Count);
            
            for (int i = 0; i < count; i++)
            {
                int i_random = Random.Range(0, skillpaths.Count - 1);
                string path = skillpaths[i_random].Remove(0, 17).Split(".")[0];

                WeaponConfig _config = Resources.Load<WeaponConfig>(path);
                if (_config == null)
                    _weaponConfigs.Add(new Tuple<WeaponConfig, WeaponAttachment>(null, Resources.Load<WeaponAttachment>(path)));
                else
                    _weaponConfigs.Add(new Tuple<WeaponConfig, WeaponAttachment>(_config, null));
                
                skillpaths.RemoveAt(i_random);
                print("Ramdom Count : " + skillpaths.Count);
            }
            return _weaponConfigs;
        }
        
        public List<Tuple<MainSkillUpgrade, AttachmentSkillUpgrade>> RandomUpgradeSkill(int count)
        {
            List<Tuple<MainSkillUpgrade, AttachmentSkillUpgrade>> _weaponUpgrade = new List<Tuple<MainSkillUpgrade, AttachmentSkillUpgrade>>();
            List<string> skillpaths = new List<string>();
            skillpaths = _skillUpgradePath.paths.ToArray().Clone().ConvertTo<string[]>().ToList();
            //Array.Copy(_skillConfigPath.paths.ToArray(), skillpaths.ToArray(), _skillConfigPath.paths.Count);
            
            for (int i = 0; i < count; i++)
            {
                int i_random = Random.Range(0, skillpaths.Count - 1);
                string path = skillpaths[i_random].Remove(0, 17).Split(".")[0];

                MainSkillUpgrade _upgrade = Resources.Load<MainSkillUpgrade>(path);
                if (_upgrade == null)
                    _weaponUpgrade.Add(new Tuple<MainSkillUpgrade, AttachmentSkillUpgrade>(null, Resources.Load<AttachmentSkillUpgrade>(path)));
                else
                    _weaponUpgrade.Add(new Tuple<MainSkillUpgrade, AttachmentSkillUpgrade>(_upgrade, null));
                
                skillpaths.RemoveAt(i_random);
                print("Ramdom Count : " + skillpaths.Count);
            }
            return _weaponUpgrade;
        }
        
        private void ApplySkill()
        {
            
        }
    }
}