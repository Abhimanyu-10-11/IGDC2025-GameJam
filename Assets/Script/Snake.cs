
using System.Collections.Generic;
using TMPro;
using UnityEngine;
namespace IGDC
{
    public class Snake : MonoBehaviour
    {



        private int m_SnakeEatTotalAmount;
        private float m_SnakeEnergy;
        private int m_MaxSnakeEnergy = 10;


        private int m_NumberOfPreyToStore;
        private int m_EatenPreyStored;




        private HUDUIManager m_HudUiManager;
        public bool m_IsShiftPressed = false;

        private SnakeMovement m_Movement;


        private List<Transform> m_Indicator = new List<Transform>();


        private int m_GrowSize = 2;
        private int m_ScoreTillWhichSnakeGrow = 10;


        public delegate void GetSnakeValueDlegate(float value);
        public GetSnakeValueDlegate SetSnakeTotalEatAmount;
        public GetSnakeValueDlegate SetSnakeEneryAmount;


        [SerializeField] private AudioSource m_AudioSourceEffect;
        [SerializeField] private AudioClip m_KilledBYPredatorClip;
        [SerializeField] private AudioClip m_SnakeEatingClip;

        private void Awake()
        {
            Time.timeScale = 1f;
            m_Movement = GetComponent<SnakeMovement>();
            m_NumberOfPreyToStore = transform.childCount;
            m_Indicator.Clear();
            for (int i = 0; i < m_NumberOfPreyToStore; i++)
            {
                Transform t = transform.GetChild(i);
                t.gameObject.SetActive(false);
                m_Indicator.Add(t);
            }

        
        }

        private void Start()
        {
           
            if (m_HudUiManager == null)
            {
                m_HudUiManager = FindAnyObjectByType<HUDUIManager>();
            }

        }
       




        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                if (m_EatenPreyStored > 0)
                {
                    m_EatenPreyStored--;
                    if (m_SnakeEatTotalAmount % 1 == 0)
                        EcoSystemsManager.Instance.CreatePlants(m_Movement.GetSnakesPlantSpawnPoint());

                    for (int i = 0; i < m_Indicator.Count; i++)
                    {
                        if (i < m_EatenPreyStored)
                            m_Indicator[i].gameObject.SetActive(true);
                        else
                            m_Indicator[i].gameObject.SetActive(false);
                    }

                }


            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Prey")
            {

                if (m_EatenPreyStored < m_NumberOfPreyToStore)
                {
                    m_EatenPreyStored++;
                    for (int i = 0; i < m_Indicator.Count; i++)
                    {
                        if (i < m_EatenPreyStored)
                            m_Indicator[i].gameObject.SetActive(true);
                        else
                            m_Indicator[i].gameObject.SetActive(false);
                    }


                }

                EcoSystemsManager.Instance.RemovePrey(collision.gameObject.GetComponent<Prey>());
                m_SnakeEatTotalAmount++;

                m_AudioSourceEffect.clip = m_SnakeEatingClip; 
                m_AudioSourceEffect.Play();

                SetSnakeTotalEatAmount(m_SnakeEatTotalAmount);

                if (m_SnakeEnergy > m_MaxSnakeEnergy)
                {
                    m_SnakeEnergy = m_MaxSnakeEnergy;
                    SetSnakeEneryAmount(m_SnakeEnergy);

                }
                else
                {
                    m_SnakeEnergy += 0.5f;
                    SetSnakeEneryAmount(m_SnakeEnergy);
                }


                if (GetSnakeEatAmount() % 5 == 0)
                {
                    float maxSpeed = m_Movement.GetMaxSpeed()+0.5f;
                    float norSpeed = m_Movement.GetNormalSpeed() + 0.5f ;
                    if (maxSpeed >= 25)
                    {
                        maxSpeed = 25;
                    }
                    if (norSpeed >= 20)
                    {
                        norSpeed = 20;
                    }
                    m_Movement.SetUpSpeed(maxSpeed, norSpeed);

                }

                if (GetSnakeEatAmount() <= m_ScoreTillWhichSnakeGrow)
                {
                    m_Movement.Grow(m_GrowSize);
                }




            }

            if (collision.gameObject.tag == "Predator" && m_IsShiftPressed)
            {

                if (m_SnakeEnergy > 0)
                {
                    EcoSystemsManager.Instance.RemovePredator(collision.gameObject.GetComponent<Predator>());

                }
            }
            if (collision.gameObject.tag == "Predator" && !m_IsShiftPressed)
            {
                m_AudioSourceEffect.clip = m_KilledBYPredatorClip;
                m_AudioSourceEffect.Play();
                m_HudUiManager.GameOver("GAME OVER \n \"Killed By Predator\"");
            }
        }


       
        private int GetSnakeEatAmount()
        {
            return m_SnakeEatTotalAmount;
        }



        public void ReduceSnakeEnery()
        {
            m_SnakeEnergy -= (Time.deltaTime*1.5f);
            SetSnakeEneryAmount(m_SnakeEnergy);
        }

    }
}