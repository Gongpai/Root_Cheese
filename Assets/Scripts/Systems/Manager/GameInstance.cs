using System;
using System.Collections.Generic;

namespace GDD
{
    public class PlayerInfo
    {
        public string playerName = "Kong NaJa";
        public int age = 22;
        public string date = "11/03/2544";
    }
    [Serializable]
    public class GameInstance
    {
        //Character State
        public float HP;
        public float maxHP;
        public float shield;
        public int EXP;
        public int maxEXP;
        public int updateEXP;
        public int level;
        
        //Game Level
        public int sceneLevel;
        public int chapter;
        
        //Skill
        public int mainSkill = -1;
        public int mainAttachmentSkill = -1;
        public int secondaryAttachmentSkill = -1;
        public WeaponAttachmentStats weaponAttachmentStats = new WeaponAttachmentStats();
        public WeaponConfigStats weaponConfigStats = new WeaponConfigStats();
    }
}