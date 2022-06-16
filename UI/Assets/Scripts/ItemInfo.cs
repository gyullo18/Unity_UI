using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 사용자 정의 자료형 클래스를 만드는 것.
//(public만으로는 인스펙터에 노출되지 않음)

[System.Serializable]
public class Item
{
    // 아이템 타입(종류)
    // 소모품 (사용하면 사라짐)
    // 스태미너(ST) 상승, 배터리(BT) 증가 등 - 역할
    public enum ItemType{ST, BT, HP}
    
    public ItemType type;
    public string name;
    public int value;
}

// 아이템의 정보
// 수동 처리
// MonoBehaviour 상속 : new x, 컴포넌트 역할을 할 수 있음.
public class ItemInfo : MonoBehaviour
{
    public Item itemData; // 아이템 자료형을 이용해 만든 본인의 아이템 정보
}
