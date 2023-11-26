namespace GDD
{
    public interface IWeapon
    {
        float damage { get; }
        float rate { get; }
        int shot { get; }
    }
}