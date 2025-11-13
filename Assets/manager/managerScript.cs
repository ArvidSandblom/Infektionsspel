using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class manager : MonoBehaviour
{
    public GameObject human;
    public GameObject factory;
    public GameObject stats;
    public int initialPopulation = 50;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        Instantiate(factory, new Vector3(Random.Range(4f, 5f), Random.Range(3.5f, 2.5f), 0), Quaternion.identity);
        Instantiate(factory, new Vector3(Random.Range(0.5f, -0.5f), Random.Range(2.5f, 3.5f), 0), Quaternion.identity);
        Instantiate(factory, new Vector3(Random.Range(-4f, -5f), Random.Range(3.5f, 2.5f), 0), Quaternion.identity);
        Instantiate(factory, new Vector3(Random.Range(-4f, -5f), Random.Range(0.5f, 1f), 0), Quaternion.identity);
        Instantiate(factory, new Vector3(Random.Range(4f, 5f), Random.Range(-3.5f, -2.5f), 0), Quaternion.identity);
        Instantiate(factory, new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-3.5f, -2.5f), 0), Quaternion.identity);
        Instantiate(factory, new Vector3(Random.Range(-4f, -5f), Random.Range(-3.5f, -2.5f), 0), Quaternion.identity);
        Instantiate(factory, new Vector3(Random.Range(4f, 5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity);
        for (int i = 0; i < initialPopulation; i++)
        {
            float startPosX = Random.Range(-8.0f, 8.0f);
            float startPosY = Random.Range(-4.0f, 4.0f);
            Instantiate(human, new Vector3(startPosX, startPosY, 0), Quaternion.identity);
            stats.GetComponent<statisticsManager>().healthyCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
