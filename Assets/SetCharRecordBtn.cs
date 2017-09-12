using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class SetCharRecordBtn : MonoBehaviour {

    public Image charBtnPrefab;
    public GameObject btnParent;

    struct charRecord {
        public int id;
        public int num_goodEnding;
        public int num_badEnding;
        public int num_strEnding;
        public charRecord(int id, int good, int bad, int str) {
            this.id = id;
            this.num_goodEnding = good;
            this.num_badEnding = bad;
            this.num_strEnding = str;
        }
    }
    private List<charRecord> charlist;


	void Start () {

        // set charlist to see how many btns should be instantiated
        charlist = new List<charRecord>();
        foreach (PlayerInfo.characterItem item in PlayerInfo.characterCollection) {

            bool found = false;
            int indexInCharlist = -1;

            // check if the char is already exist in the charlist
            for (int i=0;i<charlist.Count;++i) {
                if (charlist[i].id == item.id)
                {
                    indexInCharlist = i;
                    found = true;
                    break;
                }
            }
            if (found)
            {
                switch (item.ending)
                {
                    case PlayerInfo.ENDING_GOOD:
                        charlist[indexInCharlist] = new charRecord(charlist[indexInCharlist].id,charlist[indexInCharlist].num_goodEnding+1,charlist[indexInCharlist].num_badEnding,charlist[indexInCharlist].num_strEnding);
                        break;
                    case PlayerInfo.ENDING_BAD:
                        charlist[indexInCharlist] = new charRecord(charlist[indexInCharlist].id, charlist[indexInCharlist].num_goodEnding, charlist[indexInCharlist].num_badEnding+1, charlist[indexInCharlist].num_strEnding);
                        break;
                    case PlayerInfo.ENDING_STR:
                        charlist[indexInCharlist] = new charRecord(charlist[indexInCharlist].id, charlist[indexInCharlist].num_goodEnding, charlist[indexInCharlist].num_badEnding, charlist[indexInCharlist].num_strEnding+1);
                        break;
                }
            }
            else {
                charRecord recordFound = new charRecord();
                recordFound.id = item.id;
                recordFound.num_goodEnding = 0;
                recordFound.num_badEnding = 0;
                recordFound.num_strEnding = 0;
                switch (item.ending)
                {
                    case PlayerInfo.ENDING_GOOD:
                        ++recordFound.num_goodEnding;
                        break;
                    case PlayerInfo.ENDING_BAD:
                        ++recordFound.num_badEnding;
                        break;
                    case PlayerInfo.ENDING_STR:
                        ++recordFound.num_strEnding;
                        break;
                }
                charlist.Add(recordFound);
                // add list
            }
        }

        for (int i = 0; i < charlist.Count; ++i) {
            Image obj = Instantiate(charBtnPrefab);
            obj.GetComponent<CharRecord>().charID = charlist[i].id;
            obj.GetComponent<CharRecord>().num_goodEnding = charlist[i].num_goodEnding;
            obj.GetComponent<CharRecord>().num_badEnding = charlist[i].num_badEnding;
            obj.GetComponent<CharRecord>().num_strEnding = charlist[i].num_strEnding;
            obj.transform.GetChild(0).GetChild(0).GetComponent<Image>().overrideSprite = GamingInfo.characters[charlist[i].id].imgForChoosing;
            obj.transform.GetChild(0).Find("Text").GetComponent<Text>().text = GamingInfo.characters[charlist[i].id].name;
            obj.transform.SetParent(btnParent.transform);
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
