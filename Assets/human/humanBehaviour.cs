using System.Collections;
using UnityEngine;

public class humanBehaviour : MonoBehaviour
{
    public float speed = 0.5f;
    public int infectionChance = 30;
    public float incubationDuration = 5.0f;
    private string currentStatus = "Healthy";
    private bool isAlive = true;
    private float directionX;
    private float directionY;
    private float changeDirectionInterval = 2.0f;
    public GameObject stats;
    public SpriteRenderer human;

    void Awake()
    {
        stats = GameObject.Find("stats");
    }
    private void Start()
    {
        directionX = Random.Range(-1, 2);
        directionY = Random.Range(-1, 2);
    }

    // Update is called once per frame
    void Update()
    {

        changeDirectionInterval -= Time.deltaTime;
        if (changeDirectionInterval <= 0)
        {
            directionX = Random.Range(-1, 2);
            directionY = Random.Range(-1, 2);
            changeDirectionInterval = 2.0f; // Reset interval | Ändra vid behov
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
            transform.Translate(new Vector3(directionX, directionY, 0).normalized * speed * Time.deltaTime);

        }
    }
    void OnTriggerEnter2D(Collider2D humanCollision)
    {
        if (humanCollision.gameObject.tag == "Human")
        {
            //Behåll transform position för att nyttja i changeDirection
            if (humanCollision == isAlive)
            {                
                changeDirection();                                    
                reproduce(currentStatus, humanCollision.gameObject.GetComponent<humanBehaviour>().currentStatus);

                if(humanCollision.GetComponent<humanBehaviour>().currentStatus == "Infected")
                {                    
                    infect();
                }
            }
        }
    }

    /*void changeDirection2(Vector3 myPos, Vector3 otherPos, bool status)//status = isAlive (Om de är döda ska de inte ändra riktning) || Lite osäker på denna, kan behöva justeras eller bytas men detta i framtid
    {
        //Hitta avståndet mellan de två objekten
        Vector2 distance = myPos - otherPos;
        if (status == false)
        {

            if (distance == Vector2.zero)
            {
                //Om de är på exakt samma position välj en slumpmässig riktning
                distance = Random.insideUnitCircle.normalized;
            }
            else
            {
                distance = distance.normalized;
            }

            //Lägg till en liten slumpmässig marginal för att undvika exakt motsatt riktning för bättre spridning
            Vector2 marginal = Random.insideUnitCircle.normalized * 0.2f;
            Vector2 newDirection = (distance + marginal).normalized;

            directionX = newDirection.x;
            directionY = newDirection.y;

            float separationDistance = 0.05f; // Justera detta värde efter behov
            transform.position = myPos + (Vector3)(newDirection * separationDistance); //Skapa lite avstånd för att undvika att de fastnar ihop
        }
    }*/

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
    void reproduce(string status1, string status2)
    {
        int reproduceChance = 25;
        if (isAlive && status1 == "Healthy" && status2 == "Healthy")
        {
            int reproduce = Random.Range(0, 101);
            if (reproduce < reproduceChance)
            {
                
                Instantiate(this.gameObject, new Vector3(0, -0.5f, 0), Quaternion.identity);
            }
        }
        else if (isAlive && status1 == "Immune" || status1 == "Healthy" && status2 == "Immune" || status2 == "Healthy")
        {
            int reproduce = Random.Range(0, 101);
            if (reproduce < reproduceChance)
            {
                GameObject newHuman = Instantiate(this.gameObject, new Vector3(0, -0.5f, 0), Quaternion.identity);
                newHuman.GetComponent<humanBehaviour>().currentStatus = "Immune";

            }
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

}
