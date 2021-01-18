using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;

    public Collider collision;

    private Vector3 screenPoint;
    private Vector3 offset;

    private float halfWidth;
    private float halfHeight;

    private float halfCanvasW;
    private float halfCanvasH;

    //private bool draggable;

    private void Start() {
        halfWidth = (float) (GameObject.Find("Player").GetComponent<SpriteRenderer>().bounds.size.x * 0.5);
        halfHeight = (float) (GameObject.Find("Player").GetComponent<SpriteRenderer>().bounds.size.y * 0.5);

        Vector2 topRightCorner = new Vector2(1, 1);
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
        halfCanvasH = (float) (edgeVector.y * 2 * 0.5);
        halfCanvasW = (float) (edgeVector.x * 2 * 0.5);

        //draggable = true;

    }
    void OnMouseDown() {
        if (Input.GetMouseButton(0) && !GameObject.Find("Main Camera").GetComponent<GameManager>().GameOver()) {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }

    }

    void OnMouseDrag() {
       // if (draggable) {
            if (Input.GetMouseButton(0) && !GameObject.Find("Main Camera").GetComponent<GameManager>().GameOver()) {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
                if (curPosition.x < halfCanvasW - halfWidth && curPosition.x > -halfCanvasW + halfWidth &&
                        curPosition.y < halfCanvasH - halfHeight && curPosition.y > -halfCanvasH + halfHeight) {
                    transform.position = curPosition;                 
                }
               /* else {
                    draggable = false;
                }*/
            }
       // }
    }

    private void OnMouseUp() {
        //draggable = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "obstacle") {
            OnPlayerDied();
        }
    }

}
