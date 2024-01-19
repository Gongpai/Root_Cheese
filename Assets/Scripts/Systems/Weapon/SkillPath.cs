using UnityEngine;

namespace GDD
{
    public class SkillPath
    {
        public ResourcesPath _skillConfigPath;
        public ResourcesPath _skillUpgradePath;

        public ScriptableObject GetSkillFromPath(int skills, int index)
        {
            Debug.Log("Index In UI : " + index);
            switch (skills)
            {
                case 0:
                    Debug.Log($"Path is : {_skillConfigPath.paths[index]}");
                    return Resources.Load<WeaponConfig>(_skillConfigPath.paths[index]);
                    break;
                case 1:
                    Debug.Log($"Path is : {_skillConfigPath.paths[index]}");
                    return Resources.Load<WeaponAttachment>(_skillConfigPath.paths[index]);
                    break;
                case 2:
                    Debug.Log($"Path is : {_skillUpgradePath.paths[index]}");
                   return Resources.Load<MainSkillUpgrade>(_skillUpgradePath.paths[index]);
                    break;
                case 3:
                    Debug.Log($"Path is : {_skillUpgradePath.paths[index]}");
                    return Resources.Load<AttachmentSkillUpgrade>(_skillUpgradePath.paths[index]);
                    break;
                default:
                    return null;
                break;
            }
        }
    }
}