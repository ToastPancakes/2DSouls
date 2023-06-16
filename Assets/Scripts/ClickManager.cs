using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    public static GameObject rayHit = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouse2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D ray = Physics2D.Raycast(mouse2D, Vector2.zero);
            if(ray.collider != null)
            {
                Debug.Log("foundTarget");
                rayHit = ray.collider.gameObject;
                   
            }
        }
    }
}
