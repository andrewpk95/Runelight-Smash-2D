using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Token : MonoBehaviour
{
    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;

    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        m_Raycaster = canvas.GetComponent<GraphicRaycaster>();
        m_EventSystem = canvas.GetComponent<EventSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPosition(Vector3 position) {
        transform.position = position;
    }

    public CharacterType Select(Vector3 position) {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = position;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
        CharacterSelectButtonBehavior button = null;
        foreach (RaycastResult result in results) {
            Debug.Log("Hit " + result.gameObject.name);
            button = result.gameObject.GetComponent<CharacterSelectButtonBehavior>();
        }
        
        //Get information on what character it is
        if (button != null) {
            return button.character;
        }
        return CharacterType.None;
    }

    public void Deselect() {

    }
}
