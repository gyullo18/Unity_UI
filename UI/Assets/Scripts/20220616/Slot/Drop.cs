using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // UI에 관련된 이벤트들을 쓰기 위함.

// 아이템 오브젝트를 슬롯에서 놓을 때 쓰는 스크립트
// 가운데에 아이템이 잘 들어가게 하기 위해 슬롯에 Grid Layout Group을 줌
public class Drop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData e)
    {
        // update에 쓰게 된다면 bool값 필요.
        // 슬롯이 비었다면 = 자식이 없다면
        // 자식의 카운드를 셈(몇 개 있는지, 있는지 없는지 등)
        if(transform.childCount == 0)
        {
            // 클래스 Drag에서 draggingItem 접근
            // 드래그 중인 아이템을 드랍한 슬롯으로 부모를 바꿔줌
            // this.transform = 이 스크립트를 가진 슬롯의 transform
            Drag.draggingItem.transform.SetParent(this.transform);
            
            // 아이템 = 드래그 하고 있는 본인의 정보가 들어간 아이템
            Item item = Drag.draggingItem.GetComponent<ItemInfo>().itemData;
            // 게임 매니저의 AddItem 호출
            GameManager.instance.AddItem(item);
        }
        // 호출 확인
        print("Drop");
    }
}
