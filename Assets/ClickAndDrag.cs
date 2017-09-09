using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

// attach on the dragging button "Button_drag" which is attached on the prefab "Image_newColumn" in the scene "ownerEdit"
// control the dragging and call the "RefAndListControl.cs" to adjust the indexes in the control list

public class ClickAndDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler{

    private Image obj;
    private Button dragBtn;

    private GameObject tempParent;
    private GameObject view;
    private GameObject shadow;

    private GameObject insertPrefab;
    private GameObject insert;

    public int listIndex;
    public int toPutIndex;

    private float columnHeight;

    private bool dragging;

    // Use this for initialization
    void Start () {
        dragging = false;

        tempParent = GameObject.FindGameObjectWithTag("ownerEdit_control").GetComponent<RefAndListControl>().tempPanel;
        view = GameObject.FindGameObjectWithTag("ownerEdit_control").GetComponent<RefAndListControl>().view;
        shadow = GameObject.FindGameObjectWithTag("ownerEdit_control").GetComponent<RefAndListControl>().shadow;
        
        insertPrefab = Resources.Load<GameObject>("Prefabs/Image_insert");

        dragBtn = GetComponent<Button>();
        obj = gameObject.transform.parent.gameObject.GetComponent<Image>();
        columnHeight = obj.GetComponent<LayoutElement>().preferredHeight;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        tempParent.SetActive(true);
        shadow.transform.position = new Vector3(obj.transform.position.x+10, obj.transform.position.y-10,0);
        obj.transform.SetParent(tempParent.transform);
        obj.GetComponent<CanvasGroup>().alpha = 0.5f;
        insert = Instantiate(insertPrefab);
        insert.transform.SetParent(view.transform);
        toPutIndex = RefAndListControl.objList[listIndex].index;
        insert.transform.SetSiblingIndex(toPutIndex);
        dragging = true;
        //throw new NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // object following mouse
        obj.transform.position = new Vector3(Input.mousePosition.x-170, Input.mousePosition.y, 0);
        shadow.transform.position = new Vector3(obj.transform.position.x + 10, obj.transform.position.y - 10, 0);
        
        // dragging down
        if (listIndex+1 < RefAndListControl.objList.Count && obj.transform.position.y < RefAndListControl.objList[listIndex + 1].obj.transform.position.y) {
            if (RefAndListControl.objList[listIndex].index < RefAndListControl.objList[listIndex + 1].index) {
                RefAndListControl.switchItems(listIndex, listIndex + 1, out toPutIndex);
                insert.transform.SetSiblingIndex(toPutIndex);
            }
            
        }

        // dragging up
        if (listIndex-1 > -1 && obj.transform.position.y > RefAndListControl.objList[listIndex - 1].obj.transform.position.y) {
            if (RefAndListControl.objList[listIndex].index > RefAndListControl.objList[listIndex - 1].index) {
                RefAndListControl.switchItems(listIndex, listIndex - 1, out toPutIndex);
                insert.transform.SetSiblingIndex(toPutIndex);
            }
                
        }
        
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        Destroy(insert);
        tempParent.SetActive(false);
        obj.transform.SetParent(view.transform);
        obj.GetComponent<CanvasGroup>().alpha = 1f;
        obj.transform.SetSiblingIndex(toPutIndex);
    }
    
    void Update()
    {
        // detect if scroll content should move
        if (dragging) {
            if (obj.transform.position.y > 540) // going up
            {
                if (view.transform.localPosition.y - 10 < 0)
                {
                    view.transform.localPosition = new Vector3(view.transform.localPosition.x, 0, view.transform.localPosition.z);
                }
                else
                {
                    view.transform.localPosition += new Vector3(0, -10, 0);
                }
            }
            else if (obj.transform.position.y < 250) // going down
            {
                if (view.transform.localPosition.y + 10 > 230 + (columnHeight + 15) * (RefAndListControl.objList.Count - 2))
                {
                    view.transform.localPosition = new Vector3(view.transform.localPosition.x, 230 + (columnHeight + 15) * (RefAndListControl.objList.Count - 2), view.transform.localPosition.z);
                }
                else
                {
                    view.transform.localPosition += new Vector3(0, 10, 0);
                }
            }
        }
    }



}
