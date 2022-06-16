using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // UI에 관련된 이벤트들을 쓰기 위함.

// 아이템을 움직이는 것에 관한 스크립트
public class Drag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // 다이렉트 접근 가능. 누구를 드래그 하는가
    public static GameObject draggingItem = null;

    Transform inventoryTf;     // 인벤토리의 위치정보 
    Transform itemTf;     // 아이템 자신의 위치 정보
    Transform itemListTf; // 아이템리스트의 위치 정보
    
    CanvasGroup itemCg; // 아이템 자신의 캔버스 그룹
    


    void Start()
    {
        itemTf = GetComponent<Transform>();
        itemCg = GetComponent<CanvasGroup>();

        // 인벤토리 오브젝트 찾아옴
        var inventory = GameObject.Find("Inventory");
        // 찾아온 인벤토리 오브젝트에서 위치 정보를 가져옴.
        inventoryTf = inventory.transform;
        // inventoryTf = inventory.GetComponent<Transform>();

        // 아이템리스트 오브젝트 찾아옴
        var itemList = GameObject.Find("ItemList");
        // 찾아온 아이템리스트 오브젝트에서 위치 정보를 가져옴.
        itemListTf = itemList.transform;
    }

    // 드래그를 할 때마다 호출
    // 클릭(버튼 컴포넌트) & 드래그(이벤트 트리거 컴포넌트)에 따라 
    // 아이템의 위치가 마우스 방향으로 움직이는 위치로 이동하는 것.
    public void OnDrag(PointerEventData e)
    {
        Debug.Log("on");
        // 내 위치 = 마우스의 위치
        itemTf.position = Input.mousePosition;
    }

    // 드래그를 시작했을 때 호출
    // itemlist의 자식에서 빠져나옴
    // 아이템 리스트 안 > 아이템 정렬을 위함.
    public void OnBeginDrag(PointerEventData e)
    {   
        // 내 자신의 부모 = 인벤토리
        // itemTf.parent = inventoryTf;
        itemTf.SetParent(inventoryTf);
        // 지금 드래그 중인 아이템 = 자신.
        draggingItem = gameObject;
        print(draggingItem.name);
        // 드래그를 시작하자마자 아이템의 캔버스 그룹의 레이를 끔
        itemCg.blocksRaycasts = false;
    }

    // 드래그가 끝났을 때 호출
    // 슬롯 근처에 갔을 때 안착
    public void OnEndDrag(PointerEventData e)
    {
        // 드래그 아이템 = x
        draggingItem=null;
        // 레이를 다시 킴 - 드래그를 다시 가능.
        itemCg.blocksRaycasts = true;

        // 허공에 있을 때 (inventory의 자식일 때)
        // 손을 놓을 때 부모가 인벤토리?
        if (itemTf.parent==inventoryTf)
        {
            // itemList를 부모로 지정 = 회수된 아이템.
            itemTf.SetParent(itemListTf);

            // 아이템 = 나의 아이템 정보를 가져다가  (아래로)
            Item item = GetComponent<ItemInfo>().itemData;
            // 게임 매니저의 RemoveItem 호출 - 전달.
            GameManager.instance.RemoveItem(item);
        }
    }
}
