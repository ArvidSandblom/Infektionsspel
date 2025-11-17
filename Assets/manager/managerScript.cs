using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class manager : MonoBehaviour
{
    public GameObject human;
    public GameObject factory;
    public GameObject stats;
    public int initialPopulation = 50;
    public int factoryCount = 4;
    private Vector3 basePos;
    
    public Vector3 factoryPlacementDirection;    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        for (int i = 0; i < factoryCount; i++)
        {
            basePos = new Vector3(Random.Range(3.5f, 6f), Random.Range(1f ,-1f), 0);
            float angle = i * (360f / factoryCount);
            Quaternion rot = Quaternion.Euler(0, 0, angle - 45f);
            Vector3 pos = rot * basePos;

            Instantiate(factory, pos, Quaternion.identity);
        }

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
