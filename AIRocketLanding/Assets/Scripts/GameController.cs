using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
      Physics.IgnoreLayerCollision(6, 6, true);
        Change = !Render;
    }

    public static bool GetIsRender() {
        return IsRender;
    }
    
    public bool Render;
    private static bool IsRender;
    private bool Change = true;

    private void Update()
    {
        if (Render != Change)
        {
            Change = Render;
            IsRender = Render;
            List<GameObject> rootObjects = new List<GameObject>();
            Scene scene = SceneManager.GetActiveScene();
            scene.GetRootGameObjects(rootObjects);
            if (Render)
            {
                foreach (GameObject GO in rootObjects)
                {
                    Renderer Rndr = GO.GetComponent<Renderer>();
                    if (Rndr)
                    {
                        Rndr.enabled = true;
                    }



                    Renderer[] renderChildren = GO.GetComponentsInChildren<Renderer>();

                    int i = 0;
                    for (i = 0; i < renderChildren.Length; ++i)
                    {
                        renderChildren[i].enabled = true;
                    }

                }
            }
            else {
                foreach (GameObject GO in rootObjects)
                {
                    Renderer Rndr = GO.GetComponent<Renderer>();
                    if (Rndr)
                    {
                        Rndr.enabled = false;
                    }
                    Renderer[] renderChildren = GO.GetComponentsInChildren<Renderer>();

                    int i = 0;
                    for (i = 0; i < renderChildren.Length; ++i)
                    {
                        renderChildren[i].enabled = false;
                    }
                }
            }
        }
      
       
    }

}
