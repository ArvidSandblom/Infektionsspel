using UnityEngine;

public class humanBehaviour : MonoBehaviour
{
    public float speed = 0.5f;
    //Vet ej om detta behövs: private string[] infectionStatus = { "Healthy", "Infected", "Dead", "Immune" };
    public float infectionChance = 0.3f;
    public float incubationDuration = 5.0f;
    private string currentStatus = "Healthy";
    private bool isAlive = true;
    private float directionX;
    private float directionY;
    private float changeDirectionInterval = 2.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        changeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        
        changeDirectionInterval -= Time.deltaTime;
        if (changeDirectionInterval <= 0)
        {            
            directionX = Random.Range(-1.0f, 1.0f);
            directionY = Random.Range(-1.0f, 1.0f);
            changeDirectionInterval = 2.0f;
        }

        if (isAlive)
        {
            transform.Translate(new Vector3(directionX, directionY, 0).normalized * speed * Time.deltaTime);
            transform.Translate(new Vector3(directionX, directionY, 0).normalized * speed * Time.deltaTime);
        }
    }
    void OnTriggerEnter2D(Collider2D infectionSpread)
    {        
        if (infectionSpread.gameObject.tag == "Human")
        {
            changeDirection(/*transform.position, infectionSpread.transform.position*/);
            

            /*if (currentStatus == "Healthy")
            {
                float randomValue = Random.Range(0.0f, 1.0f);
                if (randomValue < infectionChance)
                {
                    
                                        
                }
            }*/
        }
    }    
    void changeDirection(/*Vector3 myPos, Vector3 otherPos*/)
    {
        directionX = Random.Range(-1.0f, 1.0f);
        directionY = Random.Range(-1.0f, 1.0f);        
    }
}
