using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [System.Serializable]
public class Itemm
{
    public string type, name, explain, price, index;
    public bool isUsing;
}

public class ShopManager : MonoBehaviour
{
    // 위 string 정보를 담을 리스트 공간 - 
    // 상점에서 팔 수 있는 모든 아이템들의 리스트
    // 전체 DB.
    public List<Itemm> allItemList = new List<Itemm>();
    // EX) 1, 2, 3단계에서 각각 선택할 수도 있게 만들어줌.
    public List<Itemm> myItemList = new List<Itemm>();
    // 현재 선택한 아이템 리스트
    public List<Itemm> curItemList = new List<Itemm>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
