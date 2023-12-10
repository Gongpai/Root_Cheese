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

        private void Start()
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
            //print("Skill Con = null : " + (_skillConfigPath == null));

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
            //print("Skill Up = null : " + (_skillUpgradePath == null));

            List<Tuple<WeaponConfig, WeaponAttachment>> _weaponConfigs = RandomMainSkill(3);
            foreach (var weaponConfig in _weaponConfigs)
            {
                if(weaponConfig.Item1 != null)
                    print("Weapon Con : " + weaponConfig.Item1.weaponName);
                else
                    print("Weapon Con : " + weaponConfig.Item2.name);
            }
            
            print("Pause");
        }

        public void OnRandomSkill(RandomSkillType _type, int count)
        {
            switch (_type)
            {
                case RandomSkillType.All:
                    break;
                case RandomSkillType.MainSkill:
                    break;
                case RandomSkillType.UpgradeSkill:
                    break;
            }
        }

        public List<Tuple<WeaponConfig, WeaponAttachment>> RandomMainSkill(int count)
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
        
        private void ApplySkill()
        {
            
        }
    }
}