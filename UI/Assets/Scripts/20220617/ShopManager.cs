using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO; // 파일로 저장하기 위한 라이브러리

// 클래스를 만들자 - 제이슨화 할수 있는 포장 : 리스트가 안되므로
public class JsonList
{
    // target에 myItemList를 담자.
    public List<Itemm> target;
    // 초기값 - 간소화
    public JsonList(List<Itemm> _target) => target = _target;
}

[System.Serializable]
public class Itemm
{
    public string type, name, explain, price, index;
    public bool isUsing;
    // 생성자(반환형이 없는 메서드+클래스와 같음) - 초기값
    public Itemm(string _type, string _name, string _explain, 
                 string _price, bool _isUsing, string _index) 
    {
        // 초기값 = 매게변수로 선언한 값
        type = _type; name = _name; explain = _explain;
        price = _price; isUsing = _isUsing; index = _index;
    }
}

public class ShopManager : MonoBehaviour
{
    // DB정보
    public TextAsset itemDB; 
    // 위 string 정보를 담을 리스트 공간 - 
    // 상점에서 팔 수 있는 모든 아이템들의 리스트
    // 전체 DB.
    [Header("---리스트---")]
    public List<Itemm> allItemList = new List<Itemm>();
    // EX) 1, 2, 3단계에서 각각 선택할 수도 있게 만들어줌.
    public List<Itemm> myItemList = new List<Itemm>();
    // 현재 선택한 아이템 리스트
    public List<Itemm> curItemList = new List<Itemm>();
    
    [Header("---UI---")]
    // 기본값 = 캐릭터 타입
    public string curType = "Character";
    public GameObject[] slots;
    // 모든 이미지 배열 - DB 순서와 일치하게 
    public Sprite[] itemImages;
    // 버튼 이미지 배열 - 변경해야함.
    public Image[] tabBtnImages;
    // 버튼 선택했을 때 이미지, 선택 안된 버튼 이미지
    public Sprite btnSelectedSprite, btnIdleSprite;

    [Header("---file---")]
    // 저장 - 파일 위치
    string filePath;


    void Start()
    {
        // 저장 경로 출력
        print(Application.persistentDataPath);
        // 파일 경로 및 이름.
        filePath = Application.persistentDataPath+"/myItemText.txt";

        // 행별 분리 - 데이터베이스 파일 가져다가 엔터를 기준.
        string[] lines = itemDB.text.Split('\n'); 
        // 확인
        print(lines[0]);
        // 하나씩 분리하여 allItemList에 저장
        foreach (var line in lines)
        {   
            // 열별 분리 - 탭 기준
            string[] rows = line.Split('\t');
            Itemm itemm = new Itemm(rows[0], rows[1], rows[2], rows[3], rows[4]=="TRUE", rows[5]);
            allItemList.Add(itemm);
        }
        TabClick(curType);
        // 게임 시작하자마자 로드
        Load();
    }

    // 구매 내역 파일로 저장 - 자신의 컴퓨터에 텍스트 타입으로.
    void Save()
    {
        // myItemList를 클래스로 포장
        JsonList tmp = new JsonList(myItemList);
        // 문자열로 저장 - 리스트를 제이슨화(문자열) 시켜 저장.
        // ToJson(클래스 타입) - 리스트 타입은 불가능(에러는 안뜸) : 맨위 클래스 선언
        string jData = JsonUtility.ToJson(tmp);
        // 파일에 넣음 - filePath경로에 jData를 텍스트로 저장.
        File.WriteAllText(filePath, jData);
    }

    // 구매 내역 유지
    void Load()
    {
        // 파일이 없을 때 - 이 파일이 존재하는가? : 아뇨 -> 돌아가
        if (!File.Exists(filePath)) return;

        string jData = File.ReadAllText(filePath);
        // FromJson : 제네릭 타입 - 자료형을 맞춰줘야함
        // myItemList이 가진 자료형 = FromJson<List<Itemm>>의 자료형
        // -> 리스트라서 정보 저장이 안됨 : 위에 만든 클래스 참고.
        // target = 내가 가지고 있는 myItemList
        myItemList = JsonUtility.FromJson<JsonList>(jData).target;
    }

    // 슬롯 클릭(몇번째 슬롯 - 번호)
    public void SlotClick(int slotNum)
    {
        // 보유하지 않은 Item=>buy
        // 보유하고 있는 Item 구분 
        // 내 아이템 리스트에서 찾음 - 현재 가지고 있는 아이템의 이름이 내 아이템 리스트 이름과 같은지
        Itemm curItem = myItemList.Find(x=>x.name == curItemList[slotNum].name);
        // 보유하지 않은 Item
        if (curItem == null)
        {
            // 구매 
            myItemList.Add(curItemList[slotNum]);
        }
        // 보유하고 있는 Item
        else
        {
            print("소유하고 있는 아이템입니다.");
        }
        
        // 물건을 살 때마다(슬롯클릭 때마다) 저장 or 게임을 끝낼 때마다 저장
        Save();
    }

    // 외부에서(클릭하면) 호출되는 메서드(누구를 눌렀는가) 
    public void TabClick(string tabName)
    {
        curType = tabName;
        // allItemList 중에서 찾아라 - 이 타입이 curType과 같다면 curItemList에서 모두 넣어주어라
        curItemList = allItemList.FindAll(x=>x.type==curType);

        // slots 길이 만큼 for문
        for (int i = 0; i < slots.Length; i++)
        {
            // curItemList 수만큼 하나하나 대상 확인
            bool isExist = i < curItemList.Count;
            slots[i].SetActive(isExist);
            // 존재한다면
            if (isExist)
            {
                // 슬롯 채우기
                // 텍스트 여러개라면 구분을 불가 
                // 슬롯의 첫번째 자식의 텍스트의 이름을 가지고 와라
                // 글씨 바꿈
                slots[i].transform.GetChild(0).GetComponent<Text>().text = curItemList[i].name;

                // 문자(curItemList[i].index) -> int 형변환
                int index = int.Parse(curItemList[i].index);
                // 스프라이트(이미지) 넣어줌. - 본인의 인덱스를 이용해 몇 번째 이미지인지.
                 slots[i].transform.GetChild(1).GetComponent<Image>().sprite = itemImages[index];
            }
        }

        // tab버튼 이미지 변경 파트 //
        // 이름을 번호로 변경 - 배열 접근 : 인덱스 번호 필요
        int tabNum = 0;

        // tabName을 알고 있을 때,
        switch (tabName)
        {
            // tabName이 Character이면 tabNum은 0
            case "Character" : 
                tabNum = 0;
                break;
            // tabName이 Balloon이면 tabNum은 1
            case "Balloon" : 
                tabNum = 1;
                break;
        }

        // tabBtnImages의 길이만큼 반복
        for (int i = 0; i < tabBtnImages.Length; i++)
        {
            // 버튼의 이미지를 바꿔줌
            // 클릭한 버튼
            if ( i == tabNum )
            {
                // 선택중인 버튼 이미지
                tabBtnImages[i].sprite = btnSelectedSprite;
            }
            // 그렇지 않은 버튼
            else
            {
                // 선택하지 않은 버튼 이미지
                tabBtnImages[i].sprite = btnIdleSprite;
            }
        }
    }
}
