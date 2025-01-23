using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Video;

public class MainPlayerController : MonoBehaviour
{
    private List <Transform> segements = new List<Transform>();
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
    public int initialSize = 4;

    //snake head direction.
    private Vector2 lastDirection ; 
    // snake body
    [SerializeField] private GameObject segmentPrefab;

    
    void Start()
    {

        ResetGame();
        rb2d = GetComponent<Rigidbody2D>();
        targetPosition = transform.position;
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
        if (Input.GetKeyDown(KeyCode.W)&& lastDirection != Vector2.down)
        {
            direction = Vector2.up;
            lastDirection = direction;
        }
        else if (Input.GetKeyDown(KeyCode.S) && lastDirection != Vector2.up)
        {
            direction = Vector2.down;
            lastDirection = direction;
        }
        else if (Input.GetKeyDown(KeyCode.A) && lastDirection != Vector2.right)
        {
            direction = Vector2.left;
            lastDirection = direction;
        }
        else if (Input.GetKeyDown(KeyCode.D) && lastDirection != Vector2.left)
        {
            direction = Vector2.right;
            lastDirection = direction;
        }
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Snap the player's position to the nearest grid point
            targetPosition = new Vector2(
                Mathf.Round(transform.position.x) + direction.x,
                Mathf.Round(transform.position.y) + direction.y
                );
                Debug.Log($"traget position : {targetPosition} ");
        }
                lastDirection = direction;

    }

    private void MoveBody(){
        for( int i  = segements.Count - 1; i > 0; i--){
            segements[i].position = segements[i-1].position;
        }
        transform.position = targetPosition;
        
    }

    public void AddSegment(){
        Transform newSegment = Instantiate(segmentPrefab, segements[segements.Count - 1].position, Quaternion.identity).transform;
        segements.Add(newSegment);
    }

        

        void FixedUpdate()
        {
            for(int i = segements.Count - 1; i > 0; i--){
                segements[i].position = segements[i -1 ].position;
            }
            rb2d.velocity = direction * moveSpeed;
            Teleport();
            MoveBody();

        }

        private void GrowSnake(){
            Transform segement = Instantiate(this.PrefabSegement);
            segement.position = segements[segements.Count - 50].position - (Vector3)direction;
            segements.Add(segement);
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Food"){
                
                //GrowSnake();
                AddSegment(); 
                scoreController.IncreaseScore(10);
                Debug.Log("food");
            }
            else if (other.tag == "Obstacle"){
                
                //ResetGame();
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

            for(int i = 1; i < this.initialSize; i++)
                {
                    segements.Add(Instantiate(this.PrefabSegement));
                }
            this.transform.position = Vector3.zero;

        }

}

    
