using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputManager : MonoBehaviour {
    //Estados
    enum State { Free, Selected }


    //Parâmetros da Unity
    public LayerMask SelectableLayers;



    //Variáveis de estado
    private State state;
    public Unit Selection { get; private set; }


    //Funções chamadas pela Unity
	void Start () {
		
	}
	void Update () {
        if(Input.GetMouseButtonDown(0))
        {
            SelectByMousePos();
        }
	}

    private void SelectByMousePos()
    {
        var hit = CastClickRay();
        //Se o raio colidiu com alguma objeto
        if (hit.collider != null)
        {
            var unit = hit.collider.GetComponent<Unit>();
            //Se esse objeto possui um componente de unidade
            if (unit != Selection)
            {
                if (unit.Selectable)
                {
                    if (Selection != null)
                        Selection.Deselect();
                    if (unit != null)
                        unit.Select();
                    Selection = unit;
                }
            }
        }
        else
        {
            Selection.Deselect();
            Selection = null;
        }
    }

    //Funções Helper
    private RaycastHit CastClickRay()
    {
        //Variável que vai guardar as informações de colisão do raio
        RaycastHit hit;
        //Posição atual do mouse
        Vector3 MousePosition = Input.mousePosition;
        //Raio a ser lançado
        Ray r = Camera.main.ScreenPointToRay(MousePosition);

        //Chamada do método
        Physics.Raycast(r, out hit, float.PositiveInfinity, SelectableLayers.value);

        return hit;
    }
}
