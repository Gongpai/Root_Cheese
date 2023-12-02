namespace GDD
{
    public interface IWeapon
    {
        float damage { get; }
        float rate { get; }
        int shot { get; }
        float power { get; }
        float bullet_spawn_distance { get; }
        BulletShotSurroundMode surroundMode { get; }
    }
}