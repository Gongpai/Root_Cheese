using UnityEngine;

namespace GDD
{
    public interface IWeapon
    {
        //Weapon Config
        float damage { get; }
        float rate { get; }
        int shot { get; }
        float power { get; }
        float bullet_spawn_distance { get; }
        BulletShotSurroundMode surroundMode { get; }
        BulletShotMode bulletShotMode { get; }
        
        //Attachment
        float shield { get; }
        float effect_health { get; }
        GameObject attachmentObject { get; }
        float attachmentSpinSpeed { get; }
        float attachmentDamage { get; }
    }
}