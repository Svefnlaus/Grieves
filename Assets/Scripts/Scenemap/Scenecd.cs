using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenecd : MonoBehaviour
{
    // Start is called before the first frame update
    public float changetime;
    public int scenebuild;
    

    // Update is called once per frame
    void Update()
    {
        changetime -= Time.deltaTime;
        if (changetime <= 0)
        {
            SceneManager.LoadScene(scenebuild, LoadSceneMode.Single);
        }
       
        
    }
}
