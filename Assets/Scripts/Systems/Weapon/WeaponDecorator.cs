namespace GDD
{
    public class WeaponDecorator : IWeapon
    {
        private readonly IWeapon _decoratedWeapon;
        private readonly WeaponAttachment _attachment;

        public WeaponDecorator(IWeapon weapon, WeaponAttachment attachment)
        {
            _attachment = attachment;
            _decoratedWeapon = weapon;
        }
        
        public float damage
        {
            get => _decoratedWeapon.damage + _attachment.damage;
        }

        public float rate
        {
            get => _decoratedWeapon.rate + _attachment.rate;
        }

        public int shot
        {
            get => _decoratedWeapon.shot + _attachment.shot;
        }
    }
}