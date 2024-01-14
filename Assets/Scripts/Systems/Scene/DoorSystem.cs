using System;
using GDD.PUN;
using UnityEngine;

namespace GDD
{
    public class DoorSystem : TriggerHandle<string>
    {
        private GameManager GM;
        private bool is_MasterClient;
        private void Start()
        {
            GM = GameManager.Instance;
        }

        private void OnTriggerEnter(Collider other)
        {
            //Get PunCharacterHealth
            PunCharacterHealth _punCharacterHealth;
            bool has_punCharacterHealth = other.TryGetComponent<PunCharacterHealth>(out _punCharacterHealth);

            //Get Layer Target
            Transform layer = other.transform.parent;

            //Check PunCharacterHealth
            if (!has_punCharacterHealth)
            {
                //print($"{other.transform.name} : Pun is null!!!!!!!!!!!!!!!!!!");
                return;
            }
            
            //Check Client
            is_MasterClient = _punCharacterHealth.CharacterSystem.isMasterClient;
            if(!is_MasterClient)
                return;
            
            if (layer == GM.player_layer)
            {
                //print("Character Take Damage");
            }
            else
            {
                //Debug.LogError("Not Found HealthSystem Component");
            }
        }
    }
}