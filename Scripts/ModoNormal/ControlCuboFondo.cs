using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCuboFondo : MonoBehaviour
{
    //Referencias
    private ControlGeneral cGeneral;
    private ControlTurnos cTurnos;

    void Awake() {
        //Inicializar referencias
        cGeneral = GameObject.Find("Circle").GetComponent<ControlGeneral>();
        cTurnos = GameObject.Find("Circle").GetComponent<ControlTurnos>();
    }

    public void recuperarPiezas(){
        GameObject.Find("Circle").GetComponent<ControlGeneral>().recuperarPiezas();
    }

    public void finDeAnimacion(){
        GetComponent<Animator>().SetBool("JugadorPerdido",false);
        cGeneral.piezaCaida = false;
        cTurnos.reordenarTurnos();

        if (cTurnos.turno==1){
            cTurnos.turno = 100;
        }else{
            cTurnos.turno--;
        }
    }
}
