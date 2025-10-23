using TMPro;
using UnityEngine;

namespace IGDC
{
    public class HighScoreManager : MonoBehaviour
    {
        
        private float m_HighScore;
        private float m_HigestTime;

        private void Awake()
        {

            DontDestroyOnLoad(gameObject);
            
        }




        public void SetHighScore(float highScore)
        {
            if (m_HighScore < highScore)
            {
                m_HighScore = highScore;
            
            }
        }
        public void SetHigestTime(float higestTime)
        {
            if(m_HigestTime<higestTime)
                m_HigestTime = higestTime;
        }

        public float GetHigestTime()
        {
            return m_HigestTime;
        }

        public float GetHighScore()
        {
        
            return m_HighScore;
        }
    }
}
