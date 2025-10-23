using UnityEngine;
using UnityEngine.SceneManagement;

namespace IGDC
{
    public class ManageAudio : MonoBehaviour
    {

        [SerializeField] private AudioSource m_AudioSourceBackGround;
        [SerializeField] private AudioSource m_AudioSourceEffects;


        [SerializeField]
        private AudioClip m_BackGround;
        [SerializeField]
        private AudioClip m_ClickSound;

        private void Awake()
        {

            m_AudioSourceBackGround.clip = m_BackGround;
        
        }

        private void Start()
        {
            m_AudioSourceBackGround.Play();
            m_AudioSourceBackGround.loop = true;
            m_AudioSourceBackGround.volume = 1.0f;
        }

        public void ClickSound()
        {
            m_AudioSourceEffects.clip = m_ClickSound;
            m_AudioSourceEffects.Play();

        }


        public void MuteSound()
        {
            m_AudioSourceBackGround.mute = true;
        }

        public void UnmuteSound()
        {
            m_AudioSourceBackGround.mute = false;
        }


        public void MuteEffect()
        {
            m_AudioSourceEffects.mute = true;
        }

        public void UnmuteEffect()
        {
            m_AudioSourceEffects.mute = false;
        }

       




    }
}
