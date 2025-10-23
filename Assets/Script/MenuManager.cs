using UnityEngine;
using UnityEngine.SceneManagement;

namespace IGDC
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_MainMenu;
        [SerializeField] private GameObject m_OptionMenu;
        [SerializeField] private GameObject m_CreditMenu;


        [SerializeField] private AudioSource m_AudioSourceEffects;
        [SerializeField] private AudioSource m_AudioSourceSound;

        [SerializeField] private AudioClip m_ClickSound;

        private void Awake()
        {
            m_AudioSourceEffects.clip = m_ClickSound;
            m_MainMenu.SetActive(true);
            m_CreditMenu.SetActive(false);
            m_OptionMenu.SetActive(false);
        }
        public void StartGame()
        {
            m_AudioSourceEffects.Play();
            SceneManager.LoadScene(2);
            Debug.Log("start");
        }

        public void Quit()
        {
            m_AudioSourceEffects.Play();
            Application.Quit();

        }

        public void Back()
        {
            m_AudioSourceEffects.Play();
            m_MainMenu.SetActive(true);
            m_CreditMenu.SetActive(false);
            m_OptionMenu.SetActive(false);
        }

        public void Credit()
        {
            m_AudioSourceEffects.Play();
            m_MainMenu.SetActive(false);
            m_CreditMenu.SetActive(true);
            m_OptionMenu.SetActive(false);
        }

        public void Option()
        {
            m_AudioSourceEffects.Play();
            m_MainMenu.SetActive(false);
            m_CreditMenu.SetActive(false);
            m_OptionMenu.SetActive(true);
        }

        public void MuteSound()
        {
            m_AudioSourceSound.mute = true;   
        }

        public void UnmuteSound()
        {
            m_AudioSourceSound.mute = false;
        }


        public void MuteEffect()
        {
            m_AudioSourceEffects.mute = true;
        }

        public void UnmuteEffect()
        {
            m_AudioSourceEffects.mute = false;
        }

        public void ClickSound()
        {
            m_AudioSourceEffects.Play();
        }



    }
}
