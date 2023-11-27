using System;
using UnityEngine;

namespace GDD
{
    public class TakeDamage : MonoBehaviour
    {
        public IWeapon _weapon { get; set; }
        private Bullet _bullet;
        private GameManager GM;
        
        //Character Owner
        private Transform _ownerLayer;

        public Transform ownerLayer
        {
            get => _ownerLayer;
            set => _ownerLayer = value;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            _bullet = GetComponent<Bullet>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision other)
        {
            Transform layer = other.transform.parent;
            if (layer != ownerLayer)
            {
                if (other.gameObject.GetComponent<CharacterSystem>() != null)
                {
                    OnTakeDamage(other.gameObject.GetComponent<CharacterSystem>());
                    print("TakeDamage : " + _weapon.damage);
                }
                else
                {
                    //Debug.LogError("Not Found HealthSystem Component");
                }
            }
            
            _bullet.ReturnToPool();
        }

        protected void OnTakeDamage(CharacterSystem characterSystem)
        {
            if (characterSystem.hp - _weapon.damage > 0)
                characterSystem.hp -= _weapon.damage;
            else
                characterSystem.hp = 0;
            
            print("Current Health : " + characterSystem.hp);
        }
    }
}