using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ScenesChange : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.M))
		{
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "2DMap") {
			    SceneManager.LoadScene("DemoScene");
            }
            else{
                SceneManager.LoadScene("2DMap");
            }
		}
    }
}
