using System;
using System.Collections.Generic;

namespace GDD
{
    [Serializable]
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
        public float HP = 100;
        public float maxHP = 100;
        public float shield = 100;
        public int EXP;
        public int maxEXP = 100;
        public int updateEXP;
        public int level;
        public float maxWalkSpeed = 3;
        
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