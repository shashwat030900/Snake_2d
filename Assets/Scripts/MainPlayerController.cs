using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{
    private List <Transform> segements; 
    public float moveSpeed = 5f;        
    private Vector2 direction = Vector2.right;   
    private Rigidbody2D rb2d;
    private Vector2 targetPosition;
    public Transform PrefabSegement;
    public bool isShieldActive = false; 
    public bool isScoreBoostActive = false;
    public bool isSpeedBoostActive = false;
    private float originalSpeed;
    public PowerUps powerUps;
    public ScoreController scoreController;
    void Start()
    {
        segements = new List<Transform>();
        segements.Add(this.transform);
        rb2d = GetComponent<Rigidbody2D>();
    }
    void Awake()
    {
        
    }

    void Update()
    {
        SnakeMovement();
        
    }

    private void SnakeMovement()
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
            for(int i = segements.Count - 1; i > 0; i--){
                segements[i].position = segements[i -1 ].position; 
            }
            rb2d.velocity = direction * moveSpeed;
            Teleport();
        }

        private void GrowSnake(){
            Transform segement = Instantiate(this.PrefabSegement);
            segement.position = segements[segements.Count - 1].position;
            segements.Add(segement);
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Food"){
                
                GrowSnake(); 
                scoreController.IncreaseScore(10);
                Debug.Log("food");
            }
            else if (other.tag == "Obstacle"){
                
                ResetGame();
                Debug.Log("Obstacle");
            }
            else if(other.tag == "Shield"){
                
                Destroy(other.gameObject);
                powerUps.ActivateSheild();
                Debug.Log("Shield");
            }

            else if(other.tag == "ScoreBoost")
            {
                Destroy(other.gameObject);
                powerUps.ActivateScoreBoost(); 
                scoreController.IncreaseScore(15);
                Debug.Log("Score"); 
            }
            else if (other.tag == "SpeedUp")
            {
                Destroy(other.gameObject);
                powerUps.ActivateSpeedUp();
                Debug.Log("Speed");
            }

        }

        private void Teleport()
        {
            float screenHeight = Camera.main.orthographicSize;
            float screenWidth = screenHeight * Camera.main.aspect;

            Vector3 position = transform.position;

            if (position.x > screenWidth){
                position.x = -screenWidth;
            }
            else if (position.x < -screenWidth){
                position.x = screenWidth;
            }

            if(position.y > screenHeight){
                position.y = -screenHeight;
            }

            else if (position.y < -screenHeight){
                position.y = screenHeight;
            }
            transform.position = position;
        }

        private void ResetGame()
        {
            for (int i = 1; i < segements.Count; i++){
                Destroy(segements[i].gameObject);
            }

            segements.Clear();
            segements.Add(this.transform);
            this.transform.position = Vector3.zero;
        }

}

    
