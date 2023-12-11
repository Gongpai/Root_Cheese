using UnityEngine;

namespace GDD
{
    public interface IWeapon
    {
        //Name
        public string mainName { get; }
        public string mainAttachmentName { get; }
        public string secAttachmentName { get; }
        
        //Weapon Config
        public GameObject bulletObject { get; }
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