using UnityEngine;
using UnityEngine.UI;


public class FPS : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float deltaTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;

        float fps = 1.0f / deltaTime;

        transform.GetComponent<Text>().text = "FPS: " + Mathf.Ceil(fps);
    }
}
