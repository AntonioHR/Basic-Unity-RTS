using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInputManager : MonoBehaviour {

    //Parâmetros da Unity
    public LayerMask SelectableLayers;
    public LayerMask TargetableLayers;



    //Variáveis de estado
    public Unit Selection { get; private set; }


    //Funções chamadas pela Unity
	void Start () {
		
	}
	void Update () {
        if(Input.GetMouseButtonDown(0))
        {
            SelectByMousePos();
        }
        if(Input.GetMouseButtonDown(1) && Selection != null)
        {
            var hit = CastClickRay(TargetableLayers);
            if(hit.collider != null)
            {
                Selection.Target(hit.collider.gameObject, hit.point);
            }
        }
	}

    private void SelectByMousePos()
    {
        var hit = CastClickRay(SelectableLayers);
        //Se o raio colidiu com alguma objeto
        if (hit.collider != null)
        {
            var unit = hit.collider.GetComponent<Unit>();
            //Se esse objeto possui um componente de unidade
            if (unit != Selection)
            {
                    if (Selection != null)
                        Selection.Deselect();
                    if (unit != null && unit.Selectable)
                        unit.Select();
                    Selection = unit;
            }
        }
        else
        {
            Selection.Deselect();
            Selection = null;
        }
    }

    //Funções Helper
    private static RaycastHit CastClickRay(LayerMask layers)
    {
        //Variável que vai guardar as informações de colisão do raio
        RaycastHit hit;
        //Posição atual do mouse
        Vector3 MousePosition = Input.mousePosition;
        //Raio a ser lançado
        Ray r = Camera.main.ScreenPointToRay(MousePosition);

        //Chamada do método
        Physics.Raycast(r, out hit, float.PositiveInfinity, layers.value);

        return hit;
    }
}