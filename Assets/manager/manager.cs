using UnityEngine;

public class manager : MonoBehaviour
{
    public GameObject human;    

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
