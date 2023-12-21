using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "NewCharacterFootStepAudioClipLists", menuName = "GDD/Character/CharacterFootStepAudioClipLists", order = 1)]
    public class CharacterFootStepAudioClipLists : ScriptableObject
    {
        [Header("Audio")]
        [SerializeField]private AudioClip m_LandingAudioClip;
        [SerializeField]private AudioClip[] m_FootstepAudioClips;
        [SerializeField][Range(0, 1)] private float m_FootstepAudioVolume = 0.5f;

        public AudioClip LandingAudioClip
        {
            get => m_LandingAudioClip;
        }

        public AudioClip[] FootstepAudioClips
        {
            get => m_FootstepAudioClips;
        }
        public float FootstepAudioVolume
        {
            get => m_FootstepAudioVolume;
        }
    }
}