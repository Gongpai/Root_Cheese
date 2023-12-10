using UnityEngine;

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
        
        public BulletShotMode bulletShotMode { get => _config.bulletShotMode; }

        private readonly WeaponConfig _config;

        public Weapon(WeaponConfig weaponConfig)
        {
            _config = weaponConfig;
        }
        
        public float shield { get => 0; }
        public float effect_health { get => 0; }
        public GameObject attachmentObject { get => null; }
        public float attachmentSpinSpeed { get => 0; }
        public float attachmentDamage { get => 0; }
    }
}