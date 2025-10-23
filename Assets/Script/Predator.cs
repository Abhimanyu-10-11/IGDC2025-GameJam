using System.Collections.Generic;
using UnityEngine;
namespace IGDC
{
    public class Predator : MonoBehaviour
    {


        [SerializeField] private float m_MaxTurnDuration = 4;
        [SerializeField] private float speed = 2;
        private float m_TurnDuration;
        private Vector2 direction;


       
        private int m_EatAmountToSpawn;
        private int m_TotalEatAmount = 0;

        private List<Collider2D> m_ColliderCollidePrey = new List<Collider2D>();
        private Collider2D m_ColliderCollideSnake = new Collider2D();

        private SnakeMovement snakeMovement;

        private List<Transform> m_Indicator = new List<Transform>();
        private int m_EatenPreyStored;

        private CircleCollider2D m_ThisObjectTriggerCollider;
        private float m_CircleColliderInitialRadius;

        private void OnEnable()
        {
            ChangeDirection();
        }

        private void Awake()
        {
            m_TurnDuration = m_MaxTurnDuration;
            m_EatAmountToSpawn = transform.childCount;
            m_Indicator.Clear();
            for (int i = 0; i < m_EatAmountToSpawn-1; i++)
            {
                Transform t = transform.GetChild(i);
                t.gameObject.SetActive(false);
                m_Indicator.Add(t);
            }

            m_ThisObjectTriggerCollider = GetComponent<CircleCollider2D>();
            m_CircleColliderInitialRadius = m_ThisObjectTriggerCollider.radius;

            ChangeDirection();
        }

        private void Start()
        {
            snakeMovement = FindAnyObjectByType<SnakeMovement>();
        }

       

        private void Update()
        {

            if (m_ColliderCollideSnake != null)
            {
                speed = snakeMovement.GetNormalSpeed() - 2f;
                direction = (m_ColliderCollideSnake.transform.position - transform.position).normalized;
                
            }
            else if (m_ColliderCollidePrey.Count > 0)
            {
                speed = 2;
                direction = (m_ColliderCollidePrey[m_ColliderCollidePrey.Count - 1].transform.position - transform.position).normalized;

            }
            else
            {

                if (m_TurnDuration > 0)
                {
                    m_TurnDuration -= Time.deltaTime;
                }
                else
                {

                    ChangeDirection();

                }


            }
            if (transform.position.x >= EcoSystemsManager.m_XMaxSpawnPosition || transform.position.x <= 0)
            {
                direction = -direction;
                m_TurnDuration = 1.5f;
                //ChangeDirection();

            }

            if (transform.position.y >= EcoSystemsManager.m_YMaxSpawnPosition || transform.position.y <= 0)
            {
                direction = -direction;
                m_TurnDuration = 1.5f;

            }
            this.transform.Translate(direction * Time.deltaTime * speed);
        }



        private void ChangeDirection()
        {
            Vector2 previousDirection = direction;
            int directionChoose = Random.Range(1, 5);
            if (directionChoose == 1 && previousDirection != Vector2.right)
            {
                direction = Vector2.right;
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (directionChoose == 2 && previousDirection != Vector2.left)
            {
                direction = Vector2.left;
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            else if (directionChoose == 3 && previousDirection != Vector2.up)
            {

                direction = Vector2.up;
            }
            else if (directionChoose == 4 && previousDirection != Vector2.down)
            {
                direction = Vector2.down;

            }
            else
            {
                ChangeDirection();
            }

          



            m_TurnDuration = m_MaxTurnDuration;
        }



        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Prey")
            {

                if (m_EatenPreyStored < m_EatAmountToSpawn)
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

                m_TurnDuration = m_MaxTurnDuration;
                m_TotalEatAmount++;
                if (m_TotalEatAmount % 2 == 0)
                {
                    EcoSystemsManager.Instance.CreatePlants(transform.position);

                }

                if (m_TotalEatAmount == m_EatAmountToSpawn)
                {
                    m_EatenPreyStored = 0;
                    for (int i = 0; i < m_Indicator.Count; i++)
                    {
                            m_Indicator[i].gameObject.SetActive(false);
                    }
                    EcoSystemsManager.Instance.CreatePredator(transform.position);
                    m_TotalEatAmount = 0;
                }
            }

            if (collision.gameObject.tag == "Predator")
            {
                ChangeDirection();
            }

            if (collision.gameObject.tag == "Plants")
            {
                ChangeDirection();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.tag == "Prey")
            {
                m_ColliderCollidePrey.Add(collision);
            }

            if (collision.gameObject.tag == "Snake")
            {
                m_ColliderCollideSnake = collision;
                m_ThisObjectTriggerCollider.radius = m_CircleColliderInitialRadius * 2;


            }


        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Prey")
            {
                m_ColliderCollidePrey.Remove(collision);
            }
            if (collision.gameObject.tag == "Snake")
            {
                m_ColliderCollideSnake = null;
                m_ThisObjectTriggerCollider.radius = m_CircleColliderInitialRadius;
            }

        }


        private void OnDisable()
        {
            m_TotalEatAmount = 0;
            for (int i = 0; i < m_Indicator.Count; i++)
            {
                
                   m_Indicator[i].gameObject.SetActive(false);
            }
        }

        
    }
}