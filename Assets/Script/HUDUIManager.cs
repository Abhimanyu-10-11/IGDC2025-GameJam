using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace IGDC
{
    public class HUDUIManager : MonoBehaviour
    {


        [SerializeField] private TextMeshProUGUI m_ScoreText;
        [SerializeField] private TextMeshProUGUI m_GameOverScoreText;
        [SerializeField] private TextMeshProUGUI m_HighScoreAndTimeText;
        [SerializeField] private TextMeshProUGUI m_PreyText;
        [SerializeField] private TextMeshProUGUI m_PredatorText;
        [SerializeField] private TextMeshProUGUI m_PlantsText;
        [SerializeField] private TextMeshProUGUI m_GameOverReasonText;
       
        [SerializeField] private TextMeshProUGUI m_PreyTimer;
        [SerializeField] private TextMeshProUGUI m_PlantTimer;
        [SerializeField] private TextMeshProUGUI m_TotalTimer;
        

        [SerializeField] private Slider m_EnergySlider;
        [SerializeField] private Slider m_PreySlider;
        [SerializeField] private Slider m_PlantSlider;
        [SerializeField] private Slider m_PredatorSlider;

        [SerializeField] private GameObject m_GamePauseMenu;
        [SerializeField] private GameObject m_GameOverMenu;
        [SerializeField] private GameObject m_OptionMenu;
        [SerializeField] private GameObject m_GuideMenu;

        

        private Animator m_PreyAnimator;
        private Animator m_PlantAnimator;


        [SerializeField] private Snake snake;
        [SerializeField] private float m_MaxTimeForGameOver = 10;
        private float m_PreyTime = 10;
        private float m_PlantTime = 10;

        private int minPreyAmount = 5;
        private int maxPreyAmount = 15;

        private int minPlantAmount = 5;
        private int maxPlantAmount = 15;
        float m_Timer;


        private int prey ;
        private int plant ;
        private int predator ;
        private bool isPreyAmoutUpdate;
        private bool isPlantAmountUpdate;

        private float m_ScoreValue;

        [SerializeField] private AudioSource m_AudioSourceEffect;
        [SerializeField] private AudioClip m_SnakeDiedClip;

        private void OnEnable()
        {
            EcoSystemsManager.Instance.SetPreyCount += UpdatePrey;
            EcoSystemsManager.Instance.SetPlantCount += UpdatePlant;
            EcoSystemsManager.Instance.SetPredatorCount += UpdatePredator;

            snake.SetSnakeTotalEatAmount += SetSnakeTotalEatAmount;
            snake.SetSnakeEneryAmount += SetSnakeEneryAmount;
        }


        private void OnDisable()
        {
            EcoSystemsManager.Instance.SetPreyCount -= UpdatePrey;
            EcoSystemsManager.Instance.SetPlantCount -= UpdatePlant;
            EcoSystemsManager.Instance.SetPredatorCount -= UpdatePredator;
            snake.SetSnakeTotalEatAmount -= SetSnakeTotalEatAmount;
            snake.SetSnakeEneryAmount -= SetSnakeEneryAmount;
        }


        private void Awake()
        {
            m_PreyAnimator = m_PreySlider.GetComponent<Animator>();
            m_PlantAnimator = m_PlantSlider.GetComponent<Animator>();
            if (m_GamePauseMenu != null)
                m_GamePauseMenu.SetActive(false);
            if(m_GameOverMenu!=null)
                m_GameOverMenu.SetActive(false);
            if(m_OptionMenu!=null)
                m_OptionMenu.SetActive(false);
            if(m_GuideMenu!=null)
                m_GuideMenu.SetActive(false);
          
            m_PreyTimer.color = Color.red;
            m_PlantTimer.color = Color.red;

            m_EnergySlider.value = 0;
        }

        private HighScoreManager m_HighScoreManager;
        private void Start()
        {
            m_HighScoreManager = FindAnyObjectByType<HighScoreManager>();    
        }



        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && m_GamePauseMenu != null)
            {
                GamePause();
            }

            m_Timer += Time.deltaTime;
            m_TotalTimer.text = "Timer:" +m_Timer.ToString();
        }

       
        private void SetSnakeEneryAmount(float amount)
        {
            m_EnergySlider.value = amount;
        }

        private void SetSnakeTotalEatAmount(float amount)
        {
            m_ScoreValue = amount;
            m_ScoreText.text = "Score:" + amount ;
            m_GameOverScoreText.text = "Score " + amount + " Time: " + ((int)m_Timer).ToString();

        }


        public void GameOver(String gameOverReason)
        {
            m_GameOverReasonText.text = gameOverReason;
            Time.timeScale = 0f;
            m_GameOverMenu.SetActive(true);
            m_HighScoreManager.SetHighScore(m_ScoreValue);
            m_HighScoreManager.SetHigestTime(m_Timer);
            m_HighScoreAndTimeText.text = "High Score " + m_HighScoreManager.GetHighScore() + "\nHight Time " + m_HighScoreManager.GetHigestTime();

        }

        public void GamePause()
        {
            Time.timeScale = 0f;
            m_GamePauseMenu.SetActive(true);
        }


        public void UpdatePrey(int count)
        {
            prey = count;
            m_PreySlider.value = prey;
            m_PreyText.text = "Prey:" + prey;
            m_PreyAnimator.SetInteger("Flow",prey);
            isPreyAmoutUpdate = true;

        }
        public void UpdatePredator(int count)
        {
            predator = count;
            m_PredatorSlider.value = predator;
            m_PredatorText.text = "Predator:" + predator;
        }
        public void UpdatePlant(int count)
        {
            plant = count;
            m_PlantSlider.value = plant;
            m_PlantsText.text = "Plant:" + plant;
            m_PlantAnimator.SetInteger("Flow", plant);
            isPlantAmountUpdate = true;

        }


        private void FixedUpdate()
        {
           

            if (isPreyAmoutUpdate)
            {

                if (prey > maxPreyAmount || prey < minPreyAmount)
                {
                    m_PreyTime -= Time.deltaTime;
                    m_PreyTimer.text = "" + m_PreyTime;

                    if (m_PreyTime <= 0)
                    {
                        if (prey > maxPreyAmount)
                            GameOver("GAME OVER \n \"Too Much Prey\"");
                        else
                            GameOver("GAME OVER \n \"Too Little Prey\"");

                    }
                }
                else
                {
                    m_PreyTime = m_MaxTimeForGameOver;
                    m_PreyTimer.text = "";
                    isPreyAmoutUpdate = false;

                }
                


            }

            if (isPlantAmountUpdate)
            {

                if (plant > maxPlantAmount || plant < minPlantAmount)
                {

                    m_PlantTime -= Time.deltaTime;
                    m_PlantTimer.text = "" + m_PlantTime;


                    if (m_PlantTime <= 0)
                    {
                        if (plant > maxPlantAmount)
                            GameOver("GAME OVER \n \"Too Much Plant\"");
                        else
                            GameOver("GAME OVER \n \"Too Little Plant\"");
                    }
                }
                else
                {
                    m_PlantTime = m_MaxTimeForGameOver;
                    m_PlantTimer.text = "";
                    isPlantAmountUpdate = false;

                }
                

            }






        }

        public void Resume()
        {
            Time.timeScale = 1f;
            m_GamePauseMenu.SetActive(false);
        }


        public void Resart()
        {
            SceneManager.LoadScene(1);
        }


        public void BackToMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        public void Option()
        {
            m_OptionMenu.SetActive(true);
            m_GamePauseMenu.SetActive(false);
        }

        public void OptionMenuBack()
        {
            m_OptionMenu.SetActive(false);
            m_GamePauseMenu.SetActive(true);

        }

        public void Guide()
        {
            m_GuideMenu.SetActive(true);
            m_GameOverMenu.SetActive(false);
        }

        public void BackToGameOver()
        {
            m_GuideMenu.SetActive(false);
            m_GameOverMenu.SetActive(true);

        }

    }
}