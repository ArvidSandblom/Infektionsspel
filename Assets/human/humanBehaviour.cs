using System.Collections;
using UnityEngine;

public class humanBehaviour : MonoBehaviour
{
    public float speed = 1f;
    public int infectionChance = 60;
    public float incubationDuration = 20f;
    public string currentStatus = "Healthy";
    private bool isAlive = true;
    private float directionX;
    private float directionY;
    private float changeDirectionInterval = 2.0f;
    public GameObject stats;
    public SpriteRenderer human;
    private bool atFactory = false;
    private bool movingToFactory = false;
    Vector3[] factoryPositions;

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
        directionX = Random.Range(-1, 2);
        directionY = Random.Range(-1, 2);
        StartCoroutine(workwork());
        factoryPositions = new Vector3[]
        {
            
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (!atFactory || !movingToFactory)
        {
            changeDirectionInterval -= Time.deltaTime;
            if (changeDirectionInterval <= 0)
            {
                directionX = Random.Range(-1, 2);
                directionY = Random.Range(-1, 2);
                changeDirectionInterval = 2.0f; // Reset interval | Ändra vid behov
            }
        }

            if (isAlive)
            {
                //Rörelse

                if (transform.position.x >= 8.5f && directionX > 0)
                {
                    directionX *= -1;
                }
                if (transform.position.x <= -8.5f && directionX < 0)
                {
                    directionX *= -1;
                }
                if (transform.position.y >= 4.5f && directionY > 0)
                {
                    directionY *= -1;
                }
                if (transform.position.y <= -4.5f && directionY < 0)
                {
                    directionY *= -1;
                }
               /* if (currentStatus == "Infected")//Move towards uninfected humans
                {
                    speed = 2f;
                }
                else
                {
                    speed = 1f;
                }*/

                transform.Translate(new Vector3(directionX, directionY, 0).normalized * speed * Time.deltaTime);

            }
        
    }
    void OnTriggerEnter2D(Collider2D humanCollision)
    {
        if (humanCollision.gameObject.tag == "Human" && !movingToFactory || !atFactory)
        {
            //Behåll transform position för att nyttja i changeDirection
            if (humanCollision == isAlive)
            {                
                changeDirection();                                    
                /*if (humanCollision.gameObject.GetComponent<humanBehaviour>().isAlive)
                reproduce(currentStatus, humanCollision.gameObject.GetComponent<humanBehaviour>().currentStatus);*/

                if(humanCollision.GetComponent<humanBehaviour>().currentStatus == "Infected" && currentStatus == "Healthy")
                {                    
                    infect();
                }
            }
        }
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
        if (infect < infectionChance)
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
            yield return new WaitForSeconds(15f);

            Transform factory = getNearestFactory();
            if (factory == null) yield break; 

            movingToFactory = true;
            speed = 2f;

            Vector3 targetPos = factory.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
            while (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                if (!isAlive) yield break;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
                yield return null;
            }
           
            atFactory = true;
            movingToFactory = false;
            isAlive = false;

            yield return new WaitForSeconds(5f);

            
            isAlive = true;
            atFactory = false;
            movingToFactory = false;
            speed = 1f;
            directionX = Random.Range(-1, 2);
            directionY = Random.Range(-1, 2);
        }
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
    Transform getNearestFactory()
    {
        GameObject[] factories = GameObject.FindGameObjectsWithTag("Factory");
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

}    
    /*void reproduce(string status1, string status2)
    {
        int reproduceChance = 25;
        if (isAlive && status1 == "Healthy" && status2 == "Healthy")
        {
            int reproduce = Random.Range(0, 101);
            if (reproduce < reproduceChance)
            {

                Instantiate(this.gameObject, new Vector3(0, -0.5f, 0), Quaternion.identity);
                StartCoroutine(addDelay(20));

            }
        }
        else if (isAlive && status1 == "Immune" || status1 == "Healthy" && status2 == "Immune" || status2 == "Healthy")
        {
            int reproduce = Random.Range(0, 101);
            if (reproduce < reproduceChance)
            {
                GameObject newHuman = Instantiate(this.gameObject, new Vector3(0, -0.5f, 0), Quaternion.identity);
                newHuman.GetComponent<humanBehaviour>().currentStatus = "Immune";
                StartCoroutine(addDelay(20));

            }
        }
    }*/