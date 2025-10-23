
using System.Collections.Generic;
using UnityEngine;


namespace IGDC
{
    public class SnakeMovement : MonoBehaviour
    {
        [SerializeField] private float MAX_SPEED = 10f;
        [SerializeField] private float NOR_SPEED = 2f;
        private float speed = 0;
        [SerializeField] private Transform segmentPrefab;
        private Vector2 direction = Vector2.right;
        private List<Transform> segments;

        private Snake snake;
        private int m_snakeInitialSize = 4;

        private float m_SnakeEnegyAmount;

        private void Awake()
        {
            segments = new List<Transform>();
            segments.Add(this.transform);
            
                Grow(m_snakeInitialSize);

            snake = GetComponent<Snake>();
            speed = NOR_SPEED;
        }

        private void OnEnable()
        {
            snake.SetSnakeEneryAmount += SetSnakeEneryAmount;
        }

        private void OnDisable()
        {
            snake.SetSnakeEneryAmount -= SetSnakeEneryAmount;
        }

       

        void Update()
        {




            if ((Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.UpArrow)) && direction != Vector2.down)
                direction = Vector2.up;
            if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && direction != Vector2.up)
                direction = Vector2.down;
            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && direction != Vector2.right)
                direction = Vector2.left;
            if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && direction != Vector2.left)
                direction = Vector2.right;


            Vector3 positionForPredatorSpawn = new Vector3(1,1,1);
            bool isOutOfBound = false;
            if (transform.position.x > EcoSystemsManager.m_XMaxSpawnPosition)
            {
                transform.position = new Vector3(1, transform.position.y, transform.position.z);
                positionForPredatorSpawn = new Vector3(1+1, transform.position.y, transform.position.z);
                isOutOfBound = true;

            }
            if (transform.position.x < 0)
            {
                transform.position = new Vector3(EcoSystemsManager.m_XMaxSpawnPosition, transform.position.y, transform.position.z);
                positionForPredatorSpawn = new Vector3(EcoSystemsManager.m_XMaxSpawnPosition-1, transform.position.y, transform.position.z);
                isOutOfBound = true;

            }

            if (transform.position.y > EcoSystemsManager.m_YMaxSpawnPosition)
            {
                transform.position = new Vector3(transform.position.x, 1, transform.position.z);
                positionForPredatorSpawn = new Vector3(transform.position.x, 1+1, transform.position.z);
                isOutOfBound = true;

            }
            if (transform.position.y < 0)
            {
                transform.position = new Vector3(transform.position.x, EcoSystemsManager.m_YMaxSpawnPosition, transform.position.z);
                positionForPredatorSpawn = new Vector3(transform.position.x, EcoSystemsManager.m_YMaxSpawnPosition-1, transform.position.z);
                isOutOfBound = true;

            }
            if (isOutOfBound)
            {
                EcoSystemsManager.Instance.SetInitializedPredatorTimer(true,positionForPredatorSpawn);
                isOutOfBound = false;
            }


        }

        void FixedUpdate()
        {
            for (int i = segments.Count - 1; i > 0; i--)
            {
                segments[i].position = segments[i - 1].position;
            }

            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetMouseButton(1)) && m_SnakeEnegyAmount > 0)
            {
                speed = MAX_SPEED;
                snake.ReduceSnakeEnery();
                snake.m_IsShiftPressed = true;
            }
            else
            {
                speed = NOR_SPEED;
                snake.m_IsShiftPressed = false;
            }


            this.transform.Translate(direction * Time.deltaTime * speed);
        }

        public void Grow(int size)
        {
            for (int i = 0; i < size; i++)
            {
                Transform segment = Instantiate(segmentPrefab);
                segment.position = segments[segments.Count - 1].position;
                segments.Add(segment);
            }

           
        }

        private void SetSnakeEneryAmount(float amount)
        {
            m_SnakeEnegyAmount = amount;
        }

        public Vector3 GetSnakesPlantSpawnPoint()
        {
            return segments[segments.Count - 1].position;
        }

        public void SetUpSpeed(float maxSpeed, float norSpeed)
        {
            this.NOR_SPEED = norSpeed;
            this.MAX_SPEED = maxSpeed;
        }

        public float GetMaxSpeed()
        {
            return MAX_SPEED;
        }

        public float GetNormalSpeed()
        {
            return NOR_SPEED;
        }
    }
}