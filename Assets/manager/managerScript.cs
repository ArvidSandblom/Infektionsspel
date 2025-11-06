using UnityEngine;

public class manager : MonoBehaviour
{
    public GameObject human;
    public int alive = 0;
    public int infected = 0;
    public int dead = 0;
    public int immune = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int startHumans = 50;
        for (int i = 0; i < startHumans; i++)
        {
            Instantiate(human);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
