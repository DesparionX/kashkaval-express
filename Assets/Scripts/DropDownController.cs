using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DropDownController : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    void Start()
    {
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = GetComponent<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = GetComponent<EventSystem>();
    }

    void Update()
    {
        //Check if the left Mouse button is clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            var triggers = new List<RaycastResult>();
            foreach (RaycastResult result in results)
            {
                Debug.Log("Hit " + result.gameObject.name);
                switch (result.gameObject.name)
                {
                    case "NameSearch":
                    case "SearchTempl":
                    case "Cust":
                    case "DropMenu":
                        triggers.Add(result);
                        break;
                    default:
                        break;
                }
            }
            if (triggers.Count > 0)
            {
                FindObjectOfType<NameSearch>().OpenDD();
                triggers.Clear();
            }
            else
                FindObjectOfType<NameSearch>().CloseDD();
        }
    }

    private void Cl()
    {

        /*
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("CLICK");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if(hit.transform != null)
                {
                    Debug.Log("IMAA");
                }
                GameObject g = hit.transform.gameObject;
                Debug.Log(g.name);
                switch (g.name)
                {
                    case "NameSearch":
                    case "SearchTempl":
                    case "Cust":
                    case "DropMenu":
                        FindObjectOfType<NameSearch>().OpenDD();
                        break;
                    default:
                        FindObjectOfType<NameSearch>().CloseDD();
                        break;
                }
            }
        }
        */
    }
}
