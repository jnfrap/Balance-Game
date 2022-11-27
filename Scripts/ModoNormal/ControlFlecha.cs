using UnityEngine;
using UnityEngine.EventSystems;

public class ControlFlecha : MonoBehaviour
{

    private Animator animator;

    //0 = Izquierda, 1 = Derecha, -1 = Desaparece
    public int posicion = -1;

    public bool pulsado = false;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //Animacion
        //-1 Desaparece
        //1 Aparece
        //2 Idle

        if (posicion==0){
            //Animacion
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("FlechaAparece") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                animator.SetInteger("estado", 2);
            }else{
                animator.SetInteger("estado", 1);
            }

            //Posicionar en pantalla
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.9f,Screen.height*0.15f,10));
            //Voltear
            if (transform.localScale.x>0){
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }else if (posicion==1){
            //Animacion
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("FlechaAparece") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1){
                animator.SetInteger("estado", 2);
            }else{
                animator.SetInteger("estado", 1);
            }

            //Posicionar en pantalla
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width*0.1f,Screen.height*0.15f,10));
            //Voltear
            if (transform.localScale.x<0){
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }else if (posicion==-1){
            animator.SetInteger("estado", -1);
        }
    }

    public void OnPointerDown(PointerEventData eventData){
        pulsado = true;
        GameObject[] piezas = GameObject.FindGameObjectsWithTag("Pieza");
        GameObject piezaActual = piezas[0];
        for (int i=0;piezas.Length>i;i++){
            if (piezas[i].GetComponent<ControlPieza>().timeSinceInitialization<piezaActual.GetComponent<ControlPieza>().timeSinceInitialization){
                piezaActual = piezas[i];
            }
        }

        piezaActual.GetComponent<ControlPieza>().girarPieza();
    }

    public void OnPointerUp(PointerEventData eventData){
        pulsado = false;
    }
}
