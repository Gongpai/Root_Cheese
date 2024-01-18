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
        public float HP;
        public float maxHP;
        public float shield;
        public int EXP;
        public int maxEXP;
        public int updateEXP;
        public int level;

        public WeaponAttachmentStats weaponAttachmentStats = new WeaponAttachmentStats();
        public WeaponConfigStats weaponConfigStats = new WeaponConfigStats();
    }
}