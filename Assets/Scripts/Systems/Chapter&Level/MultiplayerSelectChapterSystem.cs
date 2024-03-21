using System;
using GDD.PUN;

namespace GDD
{
    public class MultiplayerSelectChapterSystem : SelectChapterSystem
    {
        private PunRoomManager PRM;
        private PunNetworkManager PNM;

        protected override void Awake()
        {
            base.Awake();
            GM.selectChapter = 0;
        }
        
        private void OnEnable()
        {
            PNM = PunNetworkManager.Instance;
            PNM.OnJoinRoomAction += SetChapterWhenJointRoom;
            PRM = PunRoomManager.Instance;
            PRM.SelectChapterCallback += SelectChapterCallback;
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            
        }

        protected override void SetChapter(int value)
        {
            base.SetChapter(value);
            PRM.CreateChapterSelect(value);
            GM.selectChapter = value;
            PunLevelManager.Instance.UpdateLevelName();
            m_chapter.text = $"Chapter {GM.selectChapter + 1}";
        }

        private void SetChapterWhenJointRoom()
        {
            PRM.CreateChapterSelect(GM.selectChapter);
        }

        private void SelectChapterCallback(int value)
        {
            GM.selectChapter = value;
            m_chapter.text = $"Chapter {GM.selectChapter + 1}";
            print($"Call Back Chapter : {value}");

            if (value != m_moveMultiObject.select)
            {
                m_moveMultiObject.SetSelect(GM.selectChapter);
            }
        }

        private void OnDisable()
        {
            PNM.OnJoinRoomAction -= SetChapterWhenJointRoom;
            PRM.SelectChapterCallback -= SelectChapterCallback;
        }
    }
}