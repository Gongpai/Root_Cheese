using System;
using GDD.PUN;

namespace GDD
{
    public class MultiplayerSelectChapterSystem : SelectChapterSystem
    {
        private PunRoomManager PRM;
        private PunNetworkManager PNM;

        private void OnEnable()
        {
            PNM = PunNetworkManager.Instance;
            PNM.OnJoinRoomAction += SetChapterWhenJointRoom;
        }
        
        protected override void Start()
        {
            base.Start();

            PRM = PunRoomManager.Instance;
            PRM.SelectChapterCallback += SelectChapterCallback;
            GM.selectChapter = 0;
            m_moveMultiObject.OnSelect.AddListener(SetChapter);
        }

        protected override void Update()
        {
            
        }

        protected override void SetChapter(int value)
        {
            base.SetChapter(value);
            PRM.CreateChapterSelect(value);
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
        }

        private void OnDisable()
        {
            PNM.OnJoinRoomAction -= SetChapterWhenJointRoom;
            PRM.SelectChapterCallback -= SelectChapterCallback;
        }
    }
}