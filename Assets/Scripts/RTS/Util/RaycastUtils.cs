using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RTS.Util
{
    static class RaycastUtils
    {
        public static RaycastHit CastClickRay(LayerMask layers)
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

        public static RaycastHit[] BoxSelect(LayerMask layers, Vector3 startPosition)
        {
            float dist = Camera.main.nearClipPlane;
            startPosition.z = dist;
            var mousePos = Input.mousePosition;
            mousePos.z = dist;
            Vector3 startWorldPosition = Camera.main.ScreenToWorldPoint(startPosition);
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePos);



            Quaternion cameraOrientation = Camera.main.transform.rotation;

            Vector3 halfExtents = (mouseWorldPosition - startWorldPosition) / 2;
            Vector3 center = startWorldPosition + halfExtents;
            halfExtents = Quaternion.Inverse(cameraOrientation) *  halfExtents;
            halfExtents.z = 1;
            if (halfExtents.x < 0)
                halfExtents.x *= -1;
            if (halfExtents.y < 0)
                halfExtents.y *= -1;
            
            //halfExtents = new Vector3(1, 1, 1);
            //Debug.Log(halfExtents);

            //ExtDebug.DrawBoxCastBox(center,
            //    halfExtents, cameraOrientation, cameraOrientation * Vector3.forward, 100, Color.red);
            return Physics.BoxCastAll(center, 
                halfExtents, cameraOrientation * Vector3.forward, cameraOrientation, 100, layers.value);
        }
    }
}
