using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


namespace IGDC
{
    public class Prey : MonoBehaviour
    {



        [Header("Turn")]
        [SerializeField] private float m_MaxTurnDuration = 8;
        [SerializeField] private float speed = 1;
        private float m_TurnDuration;
        private Vector2 direction;



        [SerializeField]
        private int m_EatAmountToSpawn = 2;
        private int m_EatAmount = 0;


        private SpriteRenderer spriteRenderer;


        private Transform m_Indicator;


        private List<Collider2D> m_PlantCollider = new List<Collider2D>();


        private void OnEnable()
        {
            ChangeDirection();
        }


        private void Awake()
        {
            m_TurnDuration = m_MaxTurnDuration;
            m_Indicator = transform.GetChild(0);
            m_Indicator.gameObject.SetActive(false);
      
            ChangeDirection();


            spriteRenderer = GetComponent<SpriteRenderer>();
        }


        private void Update()
        {


            if (m_PlantCollider.Count > 0)
            {
                direction = (m_PlantCollider[m_PlantCollider.Count - 1].transform.position - transform.position).normalized;
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
            if (directionChoose == 1)
            {
                direction = Vector2.right;
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (directionChoose == 2)
            {
                direction = Vector2.left;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            if (directionChoose == 3)
            {
                
                direction = Vector2.up;
            }
            if (directionChoose == 4)
            {
                direction = Vector2.down;
              
            }

            if (previousDirection == direction)
            {
                ChangeDirection();
            }

            m_TurnDuration = m_MaxTurnDuration;
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {


            if (collision.gameObject.tag == "Plants")
            {

                EcoSystemsManager.Instance.RemovePlants(collision.gameObject.GetComponent<Plant>());
                m_EatAmount++;
                if (m_EatAmountToSpawn / 2 == m_EatAmount)
                {
                    EcoSystemsManager.Instance.CreatePrey(transform.position);

                }
                if (m_EatAmountToSpawn == m_EatAmount)
                {
                    EcoSystemsManager.Instance.CreatePrey(transform.position);
                    EcoSystemsManager.Instance.CreatePrey(transform.position);
                    m_EatAmount = 0;
                }


                ShowAndHideIndicator();
                m_TurnDuration = m_MaxTurnDuration;
            }


            if (collision.gameObject.tag == "Prey")
            {

                ChangeDirection();
            }
        }


       
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Plants")
            {

                m_PlantCollider.Add(collision);

            }

        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Plants")
            {
                m_PlantCollider.Remove(collision);
            }

        }



        private void ShowAndHideIndicator()
        {

            if (m_EatAmount == 0)
            {
                m_Indicator.gameObject.SetActive(false);
            }
            else if (m_EatAmount == 1)
            {
                m_Indicator.gameObject.SetActive(true);
            }
            else
            {
                m_Indicator.gameObject.SetActive(false);
            }


        }

        private void OnDisable()
        {
            m_EatAmount = 0;
            ShowAndHideIndicator();
            
        }

    }
}