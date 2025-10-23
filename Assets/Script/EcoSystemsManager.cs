using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
namespace IGDC
{
    public class EcoSystemsManager : MonoBehaviour
    {

        [SerializeField]
        public static int m_XMaxSpawnPosition = 35;
        [SerializeField]
        public static int m_YMaxSpawnPosition = 20;

        [SerializeField] private int m_InitalNumberOfPlants;
        [SerializeField] private int m_InitalNumberOfPrey;
        [SerializeField] private int m_InitalNumberOfPredator;


        [SerializeField] private List<GameObject> m_Preys = new List<GameObject>();
        [SerializeField] private List<GameObject> m_Plants = new List<GameObject>();
        [SerializeField] private List<GameObject> m_Predators = new List<GameObject>();


        [SerializeField] private AudioSource m_AudioSourceEffect;
        [SerializeField] private AudioClip m_PredatorAppearClip;

        /// <summary>
        /// AllPrey.count means number of prey in map
        /// </summary>
        private List<Plant> AllPlants = new List<Plant>();

        private List<Predator> AllPredator = new List<Predator>();

        private List<Prey> AllPrey = new List<Prey>();



        private List<Plant> StoredPlants = new List<Plant>();
        private List<Prey> StoredPrey = new List<Prey>();
   


        [SerializeField] private int m_NumberOfPreyToSpawnPredator = 15;


        

       

        public static EcoSystemsManager Instance;


        private bool m_InitializedPredatorTimer = false;
        private bool m_BeacuseOfOutOfBoundPredator = false;
        private float m_PredatorCreateTimer = 0f;
        private float m_MaxTimeToCreatePredator = 5f;


        private Vector3 m_PredatorSpawnPosition;
        [SerializeField] private List<Transform> m_SpawnLocationForPredator = new List<Transform>();

        public delegate void SetCountDelegate(int count);

        public SetCountDelegate SetPreyCount;
        public SetCountDelegate SetPredatorCount;
        public SetCountDelegate SetPlantCount;

        private void Awake()
        {
            if (Instance == null)
            {

                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }



            for (int i = 0; i < m_InitalNumberOfPlants; i++)
            {
                Vector3 randomPosition = new Vector3(
                   Random.Range(1f, m_XMaxSpawnPosition),  // X-axis
                   Random.Range(1f, m_YMaxSpawnPosition),     // Y-axis
                   Random.Range(0f, 0f)   // Z-axis
                   );

                int randPery = Random.Range(0, m_Plants.Count);
                GameObject plantGo = Instantiate(m_Plants[randPery]);
                plantGo.transform.position = randomPosition;
                AllPlants.Add(plantGo.GetComponent<Plant>());
            }

            for (int i = 0; i < m_InitalNumberOfPrey; i++)
            {
                Vector3 randomPosition = new Vector3(
                   Random.Range(1f, m_XMaxSpawnPosition),  // X-axis
                   Random.Range(1f, m_YMaxSpawnPosition),     // Y-axis
                   Random.Range(0f, 0f)   // Z-axis
                   );
                int randPery = Random.Range(0, m_Preys.Count);
                GameObject preyGo = Instantiate(m_Preys[randPery]);
                preyGo.transform.position = randomPosition;
                AllPrey.Add(preyGo.GetComponent<Prey>());


            }

            for (int i = 0; i < m_InitalNumberOfPredator; i++)
            {
                Vector3 randomPosition = new Vector3(
                     Random.Range(1f, m_XMaxSpawnPosition),  // X-axis
                     Random.Range(1f, m_YMaxSpawnPosition),     // Y-axis
                     Random.Range(0f, 0f)   // Z-axis
                     );
                int randPery = Random.Range(0, m_Predators.Count);
                GameObject predatorGo = Instantiate(m_Predators[randPery]);
                predatorGo.transform.position = randomPosition;
                AllPredator.Add(predatorGo.GetComponent<Predator>());

            }


           
        }

        private void Start()
        {
            SetPreyCount(AllPrey.Count);
            SetPredatorCount(AllPredator.Count);
            SetPlantCount(AllPlants.Count);
        }



        private void Update()
        {

            if (m_InitializedPredatorTimer)
            {
                m_PredatorCreateTimer -= Time.deltaTime;
                if (m_PredatorCreateTimer <= 0)
                {

                    CreatePredator(m_PredatorSpawnPosition);
                    m_PredatorCreateTimer = 0;
                    m_InitializedPredatorTimer = false;
                    m_BeacuseOfOutOfBoundPredator = false;
                }
            }

        }

        public void SetInitializedPredatorTimer(bool init,Vector3 position)
        {
            m_PredatorCreateTimer = m_MaxTimeToCreatePredator;
            m_PredatorSpawnPosition = position;
            m_InitializedPredatorTimer = init;
            m_BeacuseOfOutOfBoundPredator = init;
        }


        public void CreatePrey(Vector3 postion)
        {

            int count = StoredPrey.Count;
            if (count > 0)
            {

                Prey prey = StoredPrey[count - 1];
                StoredPrey.Remove(prey);
                AllPrey.Add(prey.GetComponent<Prey>());
                prey.gameObject.SetActive(true);
                prey.gameObject.transform.position = postion;

            }
            else
            {
                int randPery = Random.Range(0, m_Preys.Count);
                GameObject preyGo = Instantiate(m_Preys[randPery]);
                preyGo.transform.position = postion;
                AllPrey.Add(preyGo.GetComponent<Prey>());

            }


            
            if (AllPrey.Count > m_NumberOfPreyToSpawnPredator && !m_InitializedPredatorTimer)
            {
                StartTimerToCreatePredator();
            }


            SetPreyCount(AllPrey.Count);

        }


        private void StartTimerToCreatePredator()
        {
            m_PredatorCreateTimer = m_MaxTimeToCreatePredator;

            int rand = Random.Range(0, m_SpawnLocationForPredator.Count);
            m_PredatorSpawnPosition = m_SpawnLocationForPredator[rand].position;
            m_InitializedPredatorTimer = true;
            m_BeacuseOfOutOfBoundPredator = false;
        }

        public void CreatePredator(Vector3 postion)
        {
          

                int randPery = Random.Range(0, m_Predators.Count);
                GameObject predatorGo = Instantiate(m_Predators[randPery]);
                predatorGo.transform.position = postion;
                AllPredator.Add(predatorGo.GetComponent<Predator>());


            SetPredatorCount(AllPredator.Count);

            m_AudioSourceEffect.clip = m_PredatorAppearClip;
            m_AudioSourceEffect.Play();

        }


        public void CreatePlants(Vector3 postion)
        {
            

            int count = StoredPlants.Count;
            if (count > 0)
            {

                Plant plant = StoredPlants[count - 1];
                plant.gameObject.transform.position = postion;
                plant.gameObject.SetActive(true);
                StoredPlants.Remove(plant);
                AllPlants.Add(plant.GetComponent<Plant>());
            }
            else
            {

                int randPery = Random.Range(0, m_Plants.Count);
                GameObject plantGo = Instantiate(m_Plants[randPery]);
                plantGo.transform.position = postion;
                AllPlants.Add(plantGo.GetComponent<Plant>());
            }
            SetPlantCount(AllPlants.Count);

        }


        public void RemovePrey(Prey obj)
        {

            if (AllPrey.Count <= m_NumberOfPreyToSpawnPredator && m_InitializedPredatorTimer && !m_BeacuseOfOutOfBoundPredator)
            {
                m_InitializedPredatorTimer = false;
                m_PredatorCreateTimer = 0;

            }


            AllPrey.Remove(obj);
            StoredPrey.Add(obj);
            obj.gameObject.SetActive(false);

            SetPreyCount(AllPrey.Count);
        }

        public void RemovePlants(Plant obj)
        {

            AllPlants.Remove(obj);
            StoredPlants.Add(obj);
            obj.gameObject.SetActive(false);

            SetPlantCount(AllPlants.Count);

        }

        public void RemovePredator(Predator obj)
        {
            AllPredator.Remove(obj);
           // StoredPredator.Add(obj);
            Destroy(obj.gameObject);    
            SetPredatorCount(AllPredator.Count);


        }



    }
}
