namespace GDD
{
    public class Weapon : IWeapon
    {
        public float damage
        {
            get => _config.damage;
        }
        public float rate
        {
            get => _config.rate;
        }
        public int shot
        {
            get => _config.shot;
        }

        private readonly WeaponConfig _config;

        public Weapon(WeaponConfig weaponConfig)
        {
            _config = weaponConfig;
        }
    }
}