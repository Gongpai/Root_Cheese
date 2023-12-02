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
        
        public float bullet_spawn_distance
        {
            get => _config.bullet_spawn_distance;
        }
        
        public float power { get => _config.power; }
        public BulletShotSurroundMode surroundMode { get => _config.surroundMode; }

        private readonly WeaponConfig _config;

        public Weapon(WeaponConfig weaponConfig)
        {
            _config = weaponConfig;
        }
    }
}