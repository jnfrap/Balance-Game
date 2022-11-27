using TMPro;
using UnityEngine;

public class SpinCuboFondo : MonoBehaviour
{
    //private GameObject debug;
    private Touch screenTouch;

    void Start()
    {
        //debug = GameObject.Find("DEBUG");
    }


    void Update()
    {

        if (Input.touchCount == 1)
        {
            screenTouch = Input.GetTouch(0);

            if (screenTouch.phase == TouchPhase.Moved)
            {
                transform.Rotate(0f, -screenTouch.deltaPosition.x, 0f);
            }

            if (screenTouch.phase == TouchPhase.Ended)
            {
                //isActive = false;
            }
        }else{
            transform.Rotate(0f,-Time.deltaTime*10f*screenTouch.deltaPosition.x, 0f);
        }




        //debug.GetComponent<TextMeshProUGUI>().SetText(-screenTouch.deltaPosition.x+"");

    }
}
