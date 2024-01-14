namespace GDD
{
    public interface ICharacter
    {
        public float GetMaxHP();
        public void SetMaxHP(float maxHP);
        public float GetHP();
        public void SetHP(float hp);
        public float GetMaxShield();
        public float GetShield();
        public void SetShield(float shield);
        public int GetMaxEXP();
        public void SetMaxEXP(int maxEXP);
        public int GetEXP();
        public void AddEXP(int EXP);
        public void SetEXP(int EXP);
        public int GetLevel();
    }
}