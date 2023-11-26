using System.Collections;
using System.Collections.Generic;
using GDD;
using UnityEngine;

namespace GDD
{
    public class GameManager : DontDestroy_Singleton<GameManager>
    {
        [SerializeField] private List<GameObject> m_characterLayer;

        private GameObject _bullet_pool;
        
        public GameObject Get_Bullet_Pool
        {
            get
            {
                if (_bullet_pool == null)
                {
                    _bullet_pool = new GameObject("Bullet_Pool");
                    _bullet_pool.transform.position = Vector3.zero;
                }

                return _bullet_pool;
            }
        }

        public List<GameObject> characterLayer
        {
            get => m_characterLayer;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}