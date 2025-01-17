using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    //private Vector2 direction = Vector2.right;
    //public float speed = 5f;


    //void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.W))
    //    {
    //        direction = Vector2.up;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        direction = Vector2.down;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        direction = Vector2.left;
    //    }
    //    else if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        direction = Vector2.right;
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    this.transform.position = new Vector3(
    //        Mathf.Round(this.transform.position.x) + direction.x * speed * Time.deltaTime,
    //        Mathf.Round(this.transform.position.y) + direction.y * speed * Time.deltaTime,
    //        0.00f
    //     );

    //}
    


    private List <Transform> segements; 
    public float moveSpeed = 5f;        
    private Vector2 direction = Vector2.right;   
    private Rigidbody2D rb2d;
    private Vector2 targetPosition;
    public Transform PrefabSegement;
    void Start()
    {
        segements = new List<Transform>();
        segements.Add(this.transform);
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            direction = Vector2.up;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direction = Vector2.down;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            direction = Vector2.left;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direction = Vector2.right;
        }
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Snap the player's position to the nearest grid point
            targetPosition = new Vector2(
                Mathf.Round(transform.position.x) + direction.x,
                Mathf.Round(transform.position.y) + direction.y
                );
        }
    }

        void FixedUpdate()
        {

            rb2d.velocity = direction * moveSpeed;
        }

        private void GrowSnake(){
            Transform segement = Instantiate(this.PrefabSegement);
            segement.position = segements[segements.Count - 1].position;
            segements.Add(segement);
        }

}

    
