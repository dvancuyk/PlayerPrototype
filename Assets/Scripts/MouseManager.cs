using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MouseManager : MonoBehaviour
{
    // Know what objects are clickable
    public LayerMask ClickableLayer;
    // Normal pointer
    public Texture2D Pointer; // normal pointer
    public Texture2D Target; // cursor for clickable objects, i.e the world
    public Texture2D Doorway; // Cursor for doorways
    public Texture2D Combat; // Cursor for combat actions
    public EventVector3 OnClickEnvironment;
    private Dictionary<string, Func<MouseManager, Texture2D>> interactiveElements = new Dictionary<string, Func<MouseManager, Texture2D>>
    {
        {  "Doorway", m => m.Doorway },
        { "Item", m => m.Combat }
    };

    // Swap cursors per object
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        var cursorTexture = Pointer;
        var cursorPosition = Vector2.zero;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50, ClickableLayer.value))
        {
            cursorTexture = Target;
            cursorPosition = new Vector2(16, 16);

            if (interactiveElements.ContainsKey(hit.collider.gameObject.tag))
            {
                cursorTexture = interactiveElements[hit.collider.gameObject.tag](this);

            }

            if (Input.GetMouseButtonDown(0))
            {
                var position = hit.point;

                if (interactiveElements.ContainsKey(hit.collider.gameObject.tag))
                { 
                    position = hit.collider.gameObject.transform.position;
                    Debug.Log(hit.collider.gameObject.tag);
                }

                OnClickEnvironment.Invoke(position);
            }
        }

        Cursor.SetCursor(cursorTexture, cursorPosition, CursorMode.Auto);

    }
}

[System.Serializable]
public class EventVector3 : UnityEvent<Vector3> { }