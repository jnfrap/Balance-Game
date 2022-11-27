using TMPro;
using UnityEngine;

public class ControlCronometro : MonoBehaviour
{

    GameObject hijo;
    TextMeshProUGUI texto;
    public float segundos = 3;
    public float segundosF;
    private bool unavez = true;

    public bool activado = false;

    void Awake() {
        segundosF = segundos+1;
        hijo = this.transform.GetChild(0).gameObject;
        texto = hijo.GetComponent<TextMeshProUGUI>();
        if (segundosF == 0){
            texto.SetText("");
        }else{
            texto.SetText(segundosF +"");
        }
    }

    void Update() {
        //Posicionar en pantalla
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.1f,Screen.height*0.85f,1));

        //Cronometro
        if (activado){
            if (unavez){
                if (segundosF!=segundos+1){
                    segundosF=segundos+1;
                }
            }
            unavez=false;

            segundosF -=Time.deltaTime;
            texto.SetText(""+(int)segundosF);
            if ((int)segundosF<=0){
                activado=false;
            }
        }else{
            unavez=true;
        }

        //Desactivar sprite si esta desactivado =================> (Sustituir por animacion de desaparicion) <=====================
        GetComponent<SpriteRenderer>().enabled=activado;
        if(!GetComponent<SpriteRenderer>().enabled){
            texto.SetText("");
        }
    }

}
