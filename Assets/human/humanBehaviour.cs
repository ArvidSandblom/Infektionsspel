using System.Collections;
using UnityEngine;

public class humanBehaviour : MonoBehaviour
{
    public float speed = 1f;
    public int infectionChance = 60;
    public float incubationDuration = 20f;
    public string currentStatus = "Healthy";
    private bool isAlive = true;
    private bool canCollide = true;
    private float directionX;
    private float directionY;
    public GameObject stats;
    public SpriteRenderer human;
    private bool isSafe = false;
    float wanderTimer = 0f;
    private BoxCollider2D boxCollider;


    void Awake()
    {        
        stats = GameObject.Find("stats");
        int infectionRoll = Random.Range(0, 101);
        if (infectionRoll < 10)
        {
            currentStatus = "Infected";
            human.color = Color.red;
            stats.GetComponent<statisticsManager>().infectedCount++;
            stats.GetComponent<statisticsManager>().healthyCount--;
            StartCoroutine(DieOrImmune());
        }
    }
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        directionX = Random.Range(-1, 2);
        directionY = Random.Range(-1, 2);
        StartCoroutine(workwork());        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive || isSafe) return; 

        wanderTimer -= Time.deltaTime;
       
        
        if (wanderTimer <= 0)
        {
            SetRandomDirection();
            wanderTimer = Random.Range(1f, 3f);
        }

        MoveInBounds();

        

    }
    void OnTriggerEnter2D(Collider2D humanCollision)
    {
        if (humanCollision.gameObject.tag == "Human" && !isSafe)
        {

            if (humanCollision == isAlive)
            {                
                changeDirection();                                    


                if(humanCollision.GetComponent<humanBehaviour>().currentStatus == "Infected" && currentStatus == "Healthy" && !canCollide)
                {                    
                    infect();
                }
            }
        }
    }
    void SetRandomDirection()
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        directionX = dir.x;
        directionY = dir.y;
    }
    void MoveInBounds()
    {
        Vector3 pos = transform.position;

        if (pos.x > 8.5f || pos.x < -8.5f) directionX *= -1;
        if (pos.y > 4.5f || pos.y < -4.5f) directionY *= -1;

        transform.Translate(new Vector3(directionX, directionY) * speed * Time.deltaTime);
    }


    void changeDirection()
    {

        if (isAlive)
        {
            directionX *= -1;
            directionY *= -1;
        }
    }
    void infect()
    {
        float infect = Random.Range(0, 101);
        if (infect < infectionChance && !isSafe)
        {
            currentStatus = "Infected";
            stats.GetComponent<statisticsManager>().infectedCount++;
            stats.GetComponent<statisticsManager>().healthyCount--;
            human.color = Color.red;
            StartCoroutine(DieOrImmune());
        }

    }    
    IEnumerator workwork()
    {
        while (true)
        {

            
            if (!isAlive) yield break;
            yield return new WaitForSeconds(15f); //ändra till 15 efter testning
            Vector3 previousPos = transform.position;

            Transform factory = getNearestFactory();
            if (factory == null) yield break; 
            
            boxCollider.enabled = false;
            canCollide = false;
            isSafe = true;
            speed = 2f;

            

            Vector3 targetPos = factory.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            while (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                if (!isAlive) yield break;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                yield return null;
            }
           

            yield return new WaitForSeconds(3f);

            int infectRoll = Random.Range(0, 101);
            if (infectRoll < 15 && currentStatus == "Healthy")
            {
                currentStatus = "Infected";
                human.color = Color.red;
                stats.GetComponent<statisticsManager>().infectedCount++;
                stats.GetComponent<statisticsManager>().healthyCount--;
                StartCoroutine(DieOrImmune());
            }



            transform.position = Vector3.MoveTowards(transform.position, previousPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, previousPos) < 0.1f)
            {
                SetRandomDirection();
            }

            yield return new WaitForSeconds(4f);
            speed = 1f;
            boxCollider.enabled = true;
            isSafe = false;
            canCollide = true;
        }
    }    
    Transform getNearestFactory()
    {
        GameObject[] factories = GameObject.FindGameObjectsWithTag("factory");
        Transform nearest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject f in factories)
        {
            float dist = Vector3.Distance(currentPos, f.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = f.transform;
            }
        }

        return nearest;
    }
    IEnumerator DieOrImmune()
    {
        int immunityChance = 20;
        if (currentStatus == "Infected")
        {
            yield return new WaitForSeconds(incubationDuration); //Vänta för infektionens inkubationstid
            int outcome = Random.Range(0, 101);
            if (outcome < immunityChance)
            {
                currentStatus = "Immune";
                human.color = Color.green;            
                stats.GetComponent<statisticsManager>().immuneCount++;
                stats.GetComponent<statisticsManager>().infectedCount--;
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(2.0f); //Ytterligare tid innan död
                currentStatus = "Dead";
                isAlive = false;
                human.color = Color.black;
                stats.GetComponent<statisticsManager>().deadCount++;
                stats.GetComponent<statisticsManager>().infectedCount--;
                yield return null;
            }

        }
    }

}