using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Experimental.Networking;
using UnityEngine.SceneManagement;
using System.Web.Script.Serialization;

// attach on the empty object "empty_refAndListControl" in the scene "ownerEdit"
// do the dragging list control, tag the index of every reorderable column
// store the content which entered by the user, triggered by the button "preview"

public class RefAndListControl : MonoBehaviour
{
    static RefAndListControl refAndListControl;

    static public GameObject viewStatic;
    public GameObject view;
    public GameObject tempPanel;
    public GameObject shadow;
    static public GameObject timeContentStatic;
    public GameObject timeContent;
    public GameObject savingPanel;
    public GameObject sendingPanel;

    public struct ObjWithIndex
    {
        public int index; // sibling index
        public GameObject obj;
        public ObjWithIndex(int index, GameObject obj)
        {
            this.index = index;
            this.obj = obj;
        }
    }
    static public List<ObjWithIndex> objList;
    static public int maxIndex; // max sibling index
    static public int const_index_diff;

    // edit content
    static public string name;
    static public string address;
    static public List<OwnerInfo.ColumnItem> columnList;

    // edit time
    static public OwnerInfo.day[] tempOpenTime = new OwnerInfo.day[7];




    // Use this for initialization
    void Start()
    {
        viewStatic = view;
        timeContentStatic = timeContent;

        for (int i = 0; i < 7; ++i)
        {
            tempOpenTime[i] = new OwnerInfo.day();
            tempOpenTime[i].open = new bool();
            tempOpenTime[i].timePeriod = new List<OwnerInfo.period>();
        }

        objList = new List<ObjWithIndex>();
        maxIndex = 2;
        const_index_diff = 3;

        // column list initialization
        GameObject[] tempList = GameObject.FindGameObjectsWithTag("reorderable");
        for (int i = 0; i < tempList.Length; ++i)
        {
            ObjWithIndex item = new ObjWithIndex();
            item.obj = tempList[i];
            item.index = maxIndex + 1;
            item.obj.transform.Find("Button_drag").gameObject.GetComponent<ClickAndDrag>().listIndex = i;
            objList.Add(item);
            maxIndex++;
        }

        displaySavedContentAndGetSavedOpenTime();

    }

    // triggered when the user enters in the scene (void Start())
    void displaySavedContentAndGetSavedOpenTime()
    {
        if (OwnerInfo.storeInfo.shopName != null)
        {
            view.transform.GetChild(0).GetComponent<InputField>().text = OwnerInfo.storeInfo.shopName;
        }
        if (OwnerInfo.storeInfo.shopAddress != null)
        {
            view.transform.GetChild(2).GetComponent<InputField>().text = OwnerInfo.storeInfo.shopAddress;
        }
        tempOpenTime = OwnerInfo.storeInfo.openTime;

        if (OwnerInfo.storeInfo.infoList != null)
        {
            // create column based on ownerInfo list
            GameObject columnPrefab = Resources.Load("Prefabs/Image_newColumn") as GameObject;
            for (int i = 0; i < OwnerInfo.storeInfo.infoList.Count; ++i)
            {

                if (OwnerInfo.storeInfo.infoList[i].title == "店家介紹")
                {
                    view.transform.Find("Image_info").Find("InputField").GetComponent<InputField>().text = OwnerInfo.storeInfo.infoList[i].content;
                    view.transform.Find("Image_info").SetSiblingIndex(i + const_index_diff);
                }
                else if (OwnerInfo.storeInfo.infoList[i].title == "店家聯絡方式")
                {
                    view.transform.Find("Image_contact").Find("InputField").GetComponent<InputField>().text = OwnerInfo.storeInfo.infoList[i].content;
                    view.transform.Find("Image_contact").SetSiblingIndex(i + const_index_diff);
                }
                else
                {
                    GameObject obj = Instantiate(columnPrefab);
                    obj.transform.SetParent(view.transform);
                    obj.transform.SetSiblingIndex(i + const_index_diff);

                    obj.transform.GetChild(0).GetChild(0).GetComponent<InputField>().text = OwnerInfo.storeInfo.infoList[i].title;
                    obj.transform.Find("InputField").GetComponent<InputField>().text = OwnerInfo.storeInfo.infoList[i].content;

                    addNewItem(obj);
                }

            }
        }

    }

    // triggered by the editTime button
    static public void displayTempOpenTime()
    {
        if (tempOpenTime != null)
        {
            for (int i = 0; i < 7; ++i)
            {
                if (tempOpenTime[i].open == true)
                {
                    timeContentStatic.transform.GetChild(i).Find("Dropdown_open").GetComponent<Dropdown>().value = 0;

                    GameObject periodParent = timeContentStatic.transform.GetChild(i).Find("Panel_expandSection").gameObject;
                    int tempMin = -1;

                    if (tempOpenTime[i].timePeriod.Count > 0)
                    {
                        periodParent.transform.GetChild(0).Find("InputField").GetComponent<InputField>().text = tempOpenTime[i].timePeriod[0].begin_hr;
                        int.TryParse(tempOpenTime[i].timePeriod[0].begin_min, out tempMin);
                        periodParent.transform.GetChild(0).Find("Dropdown").GetComponent<Dropdown>().value = tempMin / 10;
                        periodParent.transform.GetChild(0).Find("InputField (1)").GetComponent<InputField>().text = tempOpenTime[i].timePeriod[0].end_hr;
                        int.TryParse(tempOpenTime[i].timePeriod[0].end_min, out tempMin);
                        periodParent.transform.GetChild(0).Find("Dropdown (1)").GetComponent<Dropdown>().value = tempMin / 10;

                        GameObject timePeriodPrefab = Resources.Load<GameObject>("Prefabs/Image_timePeriod");
                        for (int j = 1; j < tempOpenTime[i].timePeriod.Count; ++j)
                        {
                            //instantiate
                            timeContentStatic.transform.GetChild(i).GetComponent<LayoutElement>().preferredHeight += 115;
                            periodParent.transform.Find("Button_add").SetSiblingIndex(periodParent.transform.childCount - 1);

                            GameObject timeObj = Instantiate(timePeriodPrefab);
                            timeObj.transform.SetParent(periodParent.transform);
                            timeObj.transform.SetSiblingIndex(periodParent.transform.childCount - 2);
                            //set value
                            periodParent.transform.GetChild(j).Find("InputField").GetComponent<InputField>().text = tempOpenTime[i].timePeriod[j].begin_hr;
                            int.TryParse(tempOpenTime[i].timePeriod[j].begin_min, out tempMin);
                            periodParent.transform.GetChild(j).Find("Dropdown").GetComponent<Dropdown>().value = tempMin / 10;
                            periodParent.transform.GetChild(j).Find("InputField (1)").GetComponent<InputField>().text = tempOpenTime[i].timePeriod[j].end_hr;
                            int.TryParse(tempOpenTime[i].timePeriod[j].end_min, out tempMin);
                            periodParent.transform.GetChild(j).Find("Dropdown (1)").GetComponent<Dropdown>().value = tempMin / 10;
                        }
                    }

                }
                else
                {
                    timeContentStatic.transform.GetChild(i).Find("Dropdown_open").GetComponent<Dropdown>().value = 1;
                    timeContentStatic.transform.GetChild(i).Find("Panel_block").gameObject.SetActive(true);
                }
            }
        }
    }

    static public void addNewItem(GameObject obj)
    {
        ObjWithIndex item = new ObjWithIndex();
        item.obj = obj;
        item.index = maxIndex + 1;
        obj.transform.Find("Button_drag").gameObject.GetComponent<ClickAndDrag>().listIndex = item.index - const_index_diff;
        maxIndex++;
        objList.Add(item);
    }

    // index is for list index
    static public void removeItem(int index)
    {
        objList.RemoveAt(index);
        for (int i = index; i < objList.Count; ++i)
        {
            objList[i] = new ObjWithIndex(objList[i].index - 1, objList[i].obj);
            objList[i].obj.transform.Find("Button_drag").gameObject.GetComponent<ClickAndDrag>().listIndex = i;
        }
        maxIndex--;
    }

    // arguments are list indexes, except for the out argument which is sibling index
    static public void switchItems(int draggingIndex, int objToSwitchIndex, out int newSiblingIndex)
    {

        // switch the two objects in the list
        GameObject tempObj = objList[draggingIndex].obj;
        objList[draggingIndex] = new ObjWithIndex(objList[draggingIndex].index, objList[objToSwitchIndex].obj);
        objList[objToSwitchIndex] = new ObjWithIndex(objList[objToSwitchIndex].index, tempObj);

        // setting the listIndex of two objects
        objList[draggingIndex].obj.transform.Find("Button_drag").gameObject.GetComponent<ClickAndDrag>().listIndex = draggingIndex;
        objList[objToSwitchIndex].obj.transform.Find("Button_drag").gameObject.GetComponent<ClickAndDrag>().listIndex = objToSwitchIndex;

        // set the sibling index of the original objToSwitch (now listIndexed draggingIndex)
        objList[draggingIndex].obj.transform.SetSiblingIndex(objList[draggingIndex].index);
        newSiblingIndex = objList[objToSwitchIndex].index;
    }

    // triggered by confirm button in the editTime panel
    static public void getEditTime()
    {
        //tempOpenTime = new OwnerInfo.day[7];
        GameObject[] dayTime = GameObject.FindGameObjectsWithTag("dayTime");

        for (int i = 0; i < 7; ++i)
        {
            if (dayTime[i].transform.Find("Dropdown_open").GetComponent<Dropdown>().value == 0)
            {
                // open
                tempOpenTime[i].open = true;
                tempOpenTime[i].timePeriod.RemoveRange(0, tempOpenTime[i].timePeriod.Count);

                //Debug.Log(dayTime[i].transform.Find("Panel_expandSection").childCount); // i thought it won't count inactive children???
                for (int j = 0; j < dayTime[i].transform.Find("Panel_expandSection").childCount - 1; ++j)
                {

                    OwnerInfo.period periodItem = new OwnerInfo.period();
                    GameObject periodImg = dayTime[i].transform.Find("Panel_expandSection").GetChild(j).gameObject;

                    periodItem.begin_hr = periodImg.transform.Find("InputField").GetComponent<InputField>().text;
                    int begin_minute = periodImg.transform.Find("Dropdown").GetComponent<Dropdown>().value * 10;
                    if (begin_minute == 0)
                    {
                        periodItem.begin_min = begin_minute.ToString() + begin_minute.ToString();
                    }
                    else
                    {
                        periodItem.begin_min = begin_minute.ToString();
                    }


                    periodItem.end_hr = periodImg.transform.Find("InputField (1)").GetComponent<InputField>().text;
                    int end_minute = periodImg.transform.Find("Dropdown (1)").GetComponent<Dropdown>().value * 10;
                    if (end_minute == 0)
                    {
                        periodItem.end_min = end_minute.ToString() + end_minute.ToString();
                    }
                    else
                    {
                        periodItem.end_min = end_minute.ToString();
                    }


                    tempOpenTime[i].timePeriod.Add(periodItem);
                }

            }
            else
            {
                // close
                tempOpenTime[i].open = false;
            }
        }

    }

    // triggered by preview btn
    static public void getEditContent()
    {
        name = viewStatic.transform.GetChild(0).gameObject.GetComponent<InputField>().text;
        address = viewStatic.transform.GetChild(2).gameObject.GetComponent<InputField>().text;

        // open time get in the text object itself

        columnList = new List<OwnerInfo.ColumnItem>();
        for (int i = 3; i < viewStatic.transform.childCount; ++i)
        {
            GameObject obj = viewStatic.transform.GetChild(i).gameObject;
            if (obj.tag == "reorderable")
            {
                OwnerInfo.ColumnItem item = new OwnerInfo.ColumnItem();
                item.title = obj.transform.Find("Image_title").Find("Text").gameObject.GetComponent<Text>().text;
                item.content = obj.transform.Find("InputField").gameObject.GetComponent<InputField>().text;
                columnList.Add(item);
            }
            else if (obj.tag == "newColumn")
            {
                OwnerInfo.ColumnItem item = new OwnerInfo.ColumnItem();
                item.title = obj.transform.Find("Image_title").Find("InputField").gameObject.GetComponent<InputField>().text;
                item.content = obj.transform.Find("InputField").gameObject.GetComponent<InputField>().text;
                columnList.Add(item);
            }
        }

    }

    public void saveEditContent()
    {
        savingPanel.SetActive(true);
        getEditContent();
        OwnerInfo.storeInfo.shopName = name;
        OwnerInfo.storeInfo.shopAddress = address;
        OwnerInfo.storeInfo.openTime = tempOpenTime;
        OwnerInfo.storeInfo.infoList = columnList;

        // upload edit content
        StartCoroutine(sendData());
        //StartCoroutine(printOwnerData());
    }

    IEnumerator printOwnerData()
    {
        string toUrl = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/get_shopOwnerInfo?shopID=" + OwnerInfo.ownerID;
        UnityWebRequest sending = UnityWebRequest.Get(toUrl);
        yield return sending.Send();
        Debug.Log("get the owner data---");

        if (sending.error != null)
        {
            Debug.Log("error below:");
            Debug.Log(sending.error);
        }
        else
        {
            Debug.Log("correct below:");
            Debug.Log(sending.downloadHandler.text);
        }
    }



    IEnumerator sendData()
    {
        string toUrl = PlayerInfo.whichHttp + "://kevin.imslab.org" + PlayerInfo.port + "/set_shopInfo";

        WWWForm formdata = new WWWForm();
        formdata.AddField("shopID", OwnerInfo.ownerID);
        formdata.AddField("password", OwnerInfo.ownerPW);
        Debug.Log("send id:" + OwnerInfo.ownerID);
        Debug.Log("send pw:" + OwnerInfo.ownerPW);
        if (OwnerInfo.storeInfo.shopName != null)
        {
            formdata.AddField("shopName", OwnerInfo.storeInfo.shopName);
            Debug.Log("send name:" + OwnerInfo.storeInfo.shopName);

        }
        if (OwnerInfo.storeInfo.shopAddress != null)
        {
            formdata.AddField("shopAddress", OwnerInfo.storeInfo.shopAddress);
            Debug.Log("send address:" + OwnerInfo.storeInfo.shopAddress);
        }

        var jsonObj = new JavaScriptSerializer();
        string json_openTime = jsonObj.Serialize(OwnerInfo.storeInfo.openTime);
        string json_infoList = jsonObj.Serialize(OwnerInfo.storeInfo.infoList);
        Debug.Log(json_openTime);
        Debug.Log(json_infoList);
        formdata.AddField("openTime", json_openTime);
        formdata.AddField("infoList", json_infoList);


        UnityWebRequest sending = UnityWebRequest.Post(toUrl, formdata);
        yield return sending.Send();
        if (sending.error != null)
        {
            sendingPanel.transform.Find("Text").GetComponent<Text>().text = "存取資料失敗，請返回編輯重新儲存或離開捨棄檔案。";
            Debug.Log("send/error below:");
            Debug.Log("send mes:" + sending.error);
        }
        else
        {
            Debug.Log("send/correct below:");
            if (sending.downloadHandler.text == "success")
            {
                Debug.Log("send mes:" + sending.downloadHandler.text);
            }
            else
            {
                sendingPanel.transform.Find("Text").GetComponent<Text>().text = "存取資料失敗，請返回編輯重新儲存或離開捨棄檔案。";
                Debug.Log("send mes:" + sending.downloadHandler.text);
            }
        }

        savingPanel.SetActive(false);
        sendingPanel.SetActive(true);
    }

    // triggered by the send button in the editTimePanel
    static public int checkIfOpenTimeComplete()
    {
        GameObject[] inputs = GameObject.FindGameObjectsWithTag("timeCheck");
        int parseResult;
        bool success = false;
        foreach (GameObject item in inputs)
        {
            if (item.transform.parent.parent.parent.Find("Panel_block").gameObject.activeInHierarchy == false)
            {
                success = int.TryParse(item.GetComponent<InputField>().text, out parseResult);
                if (item.GetComponent<InputField>().text == "")
                {
                    return 0;
                }
                else if (parseResult > 23 || parseResult < 0)
                {
                    return -1;
                }
                else if (!success)
                {
                    return -1;
                }
            }
        }
        return 1;
    }

    static public string parseOpenTimeStruct()
    {
        string result = "";
        for (int i = 0; i < 7; ++i)
        {

            switch (i)
            {
                case 0:
                    result += "周一 ";
                    break;
                case 1:
                    result += "周二 ";
                    break;
                case 2:
                    result += "周三 ";
                    break;
                case 3:
                    result += "周四 ";
                    break;
                case 4:
                    result += "周五 ";
                    break;
                case 5:
                    result += "周六 ";
                    break;
                case 6:
                    result += "周日 ";
                    break;
            }

            if (tempOpenTime[i].open == true)
            {
                for (int j = 0; j < tempOpenTime[i].timePeriod.Count; ++j)
                {
                    result += tempOpenTime[i].timePeriod[j].begin_hr + ":" + tempOpenTime[i].timePeriod[j].begin_min + " - " + tempOpenTime[i].timePeriod[j].end_hr + ":" + tempOpenTime[i].timePeriod[j].end_min + "  ";
                }
                result += "\n";
            }
            else
            {
                result += "公休\n";
            }

        }
        return result;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
