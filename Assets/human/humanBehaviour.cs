using System.Collections;
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
        //Ny slumpmässig riktning vid start
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
                transform.Translate(new Vector3(directionX * -1, directionY, 0).normalized * speed * Time.deltaTime);
            }
            if (transform.position.x <= -8.5f && directionX < 0)
            {
                transform.Translate(new Vector3(directionX * 1, directionY, 0).normalized * speed * Time.deltaTime);
            }
            if (transform.position.y >= 4.5f && directionY > 0)
            {
                transform.Translate(new Vector3(directionX, directionY * -1, 0).normalized * speed * Time.deltaTime);
            }
            if (transform.position.y <= -4.5f && directionY < 0)
            {
                transform.Translate(new Vector3(directionX, directionY * 1, 0).normalized * speed * Time.deltaTime);
            }                        
            transform.Translate(new Vector3(directionX, directionY, 0).normalized * speed * Time.deltaTime); 
            
        }
    }
    void OnTriggerEnter2D(Collider2D infectionSpread)
    {
        if (infectionSpread.gameObject.tag == "Human")
        {
            //Behåll transform position för att nyttja i changeDirection
            changeDirection(transform.position, infectionSpread.transform.position, isAlive);


            if (currentStatus == "Healthy")
            {
                infect();
            }
        }
    }
    void changeDirection(Vector3 myPos, Vector3 otherPos, bool status)//status = isAlive (Om de är döda ska de inte ändra riktning)
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
    }
    void infect()
    {
        float infect = Random.Range(0.0f, 1.0f);
        if (infect < infectionChance)
        {
            currentStatus = "Infected";
            //managerScript.infected += 1;
        }

    }
    IEnumerator DieOrImmune()

    {
        float immunityChance = 0.2f;
        if (currentStatus == "Infected")
        {
            new WaitForSeconds(incubationDuration); //Vänta för infektionens inkubationstid
            float outcome = Random.Range(0.0f, 1.0f);
            if (outcome < immunityChance)
            {
                currentStatus = "Immune";
                yield return null;
            }
            else
            {
                new WaitForSeconds(2.0f); //Ytterligare tid innan död
                currentStatus = "Dead";
                isAlive = false;                
                yield return null;
            }
            
        }        
    }    
}
