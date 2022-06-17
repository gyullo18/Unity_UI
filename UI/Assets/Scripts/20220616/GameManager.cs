using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 연동
public class GameManager : MonoBehaviour
{
    // 싱클턴
    public static GameManager instance;
    void Awake() 
    {
        if (instance == null) instance = this;    
    }

    // 아이템 정보를 받는 것 - 소모품 x
    // 아이템 추가(누구의 아이템인지 - iteminfo의 정보)
    public void AddItem(Item item)
    {   
        // 아이템 타입에 따라 기능이 달라짐
        switch(item.type)
        {
            case Item.ItemType.ST :
                print("스태미너 증가");
                // ex) stamina + value
                break;
            case Item.ItemType.BT:
                print("배터리+1");
                break; 
        }
    }

    public void RemoveItem(Item item)
    {   
        // 아이템 타입에 따라 기능이 달라짐
        switch(item.type)
        {
            case Item.ItemType.ST :
                print("스태미너 감소");
                // ex) stamina + value
                break;
            case Item.ItemType.BT:
                print("배터리-1");
                break; 
        }
    }
    // 소모품은 아이템 제거
}
