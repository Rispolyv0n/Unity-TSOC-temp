using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// attach on the fav_btn in the scene "shopInfo"

public class AddFavShop : MonoBehaviour
{

    private Button thisBtn;
    private Image btnImg;

    public bool isFav;

    // Use this for initialization
    void Start()
    {
        thisBtn = GetComponent<Button>();
        btnImg = thisBtn.GetComponent<Image>();
        thisBtn.onClick.AddListener(checkToAddOrRemove);

    }

    void checkToAddOrRemove()
    {
        Sprite fullHeart = Resources.Load<Sprite>("ImageSource/BackgroundImage/Street/btn_favoriteList");
        Sprite emptyHeart = Resources.Load<Sprite>("ImageSource/BackgroundImage/ShopInfo/btn_favorite");

        Debug.Log(btnImg.sprite.name);

        if (isFav)
        {
            // to remove fav
            for (int i = 0; i < PlayerInfo.fav_shopID_list.Count; ++i)
            {
                if (PlayerInfo.fav_shopID_list[i].shopID.Equals(PlayerInfo.currentCheckingShopID))
                {
                    PlayerInfo.fav_shopID_list.RemoveAt(i);
                    break;
                }
            }
            isFav = false;
            btnImg.overrideSprite = emptyHeart;
        }
        else
        {
            // to add fav
            bool found = false;
            for (int i = 0; i < PlayerInfo.fav_shopID_list.Count; ++i)
            {
                if (PlayerInfo.fav_shopID_list[i].shopID.Equals(PlayerInfo.currentCheckingShopID))
                {
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                PlayerInfo.favShop shop = new PlayerInfo.favShop();
                shop.shopID = PlayerInfo.currentCheckingShopID;
                PlayerInfo.fav_shopID_list.Add(shop);
                isFav = true;
                btnImg.overrideSprite = fullHeart;
            }

        }
        GetFavShopList.refreshContent();
        StartCoroutine(PlayerInfo.uploadFavShopList());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
