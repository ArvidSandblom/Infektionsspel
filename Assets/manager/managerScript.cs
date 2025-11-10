using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class manager : MonoBehaviour
{
    public GameObject human;
    public GameObject stats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        int startHumans = 49;
        for (int i = 0; i < startHumans; i++)
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
