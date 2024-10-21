using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


//리소스폴더 -> uidocument 열어서 비교하면서 작성

public class UiScript : MonoBehaviour
{
    //유닛 타입 결정 변수
    public int unitType_DecisionVariable = 0; //0기본, 1검, 2방패, 3도끼

    //ui관련 불값(ui가 켜져있는지 꺼져있는지 확인)
    bool building_ShowOn = false;
    bool base_person_ShowOn = false, sword_person_ShowOn = false, ax_person_ShowOn = false, shield_person_ShowOn = false;

    //유닛(건물소환) 언더바 ui
    private VisualElement unitUnderbar;
    //건물(유닛소환) 언더바 ui
    private VisualElement base_buildingUnderbar_B;
    private VisualElement sword_buildingUnderbar_B;
    private VisualElement ax_buildingUnderbar_B;
    private VisualElement shield_buildingUnderbar_B;
    //승리, 패배 ui
    private VisualElement game_win;
    private VisualElement game_lose;

    //건물
    private Button spawn_base_building_B_button; //블루팀 건물
    private Button spawn_sword_building_B_button;
    private Button spawn_ax_building_B_button;
    private Button spawn_shield_building_B_button;

    //unit
    private Button spawn_base_blueUnit_button;  //기본유닛
    private Button spawn_knife_blueUnit_button; //검 아군
    private Button spawn_shield_blueUnit_button; //방패 아군
    private Button spawn_axe_blueUnit_button; //엑스 아군

    //return button
    private Button return_button1, return_button2; 

    //turnend button
    private Button turn_end;

    //자원 텍스트
    private Label food_text_lable;
    private Label wood_text_lable;
    //몇 턴인지
    private Label turn_value_lable;

    //턴엔드 클릭시 나오는 깃발모양 표시지 및 라벨
    private VisualElement turn_visual;
    private Label turn_visual_lable;

    //loding표시
    //public ParticleSystem loding;
    //Camera Camera;


    void Start()
    {
        //ui에 존재하는 uidocument를 가져온다
        var root = GetComponent<UIDocument>().rootVisualElement;

        unitUnderbar = root.Q<VisualElement>("unit_Underbar"); //unit_Underbar = 해당 visualwelement의 이름

        base_buildingUnderbar_B = root.Q<VisualElement>("basic_building_Underbar_B"); //빌딩언더바 ui 설정
        sword_buildingUnderbar_B = root.Q<VisualElement>("sword_building_Underbar_B");
        ax_buildingUnderbar_B = root.Q<VisualElement>("ax_building_Underbar_B");
        shield_buildingUnderbar_B = root.Q<VisualElement>("shield_building_Underbar_B");

        game_win = root.Q<VisualElement>("gameEndVisualElement_win");//승리 ui
        game_lose = root.Q<VisualElement>("gameEndVisualElement_lose");//패배 ui

        turn_end = root.Q<Button>("turnend_button"); //턴 종료 버튼
        
        //블루팀 건물
        spawn_base_building_B_button = root.Q<Button>("Summon_spawn_base_building_B");
        spawn_sword_building_B_button = root.Q<Button>("Summon_spawn_sword_building_B");
        spawn_ax_building_B_button = root.Q<Button>("Summon_spawn_ax_building_B");
        spawn_shield_building_B_button = root.Q<Button>("Summon_spawn_shield_building_B");

       

        //블루 유닛
        spawn_base_blueUnit_button = root.Q<Button>("Summon_spawn_base_unit_blue"); //기본유닛 버튼 설정
        spawn_knife_blueUnit_button = root.Q<Button>("Summon_spawn_knife_unit_blue"); 
        spawn_shield_blueUnit_button = root.Q<Button>("Summon_spawn_shield_unit_blue"); 
        spawn_axe_blueUnit_button = root.Q<Button>("Summon_spawn_axe_unit_blue");

        //메인으로 복귀 버튼
        return_button1 = root.Q<Button>("return_Button_win");
        return_button2 = root.Q<Button>("return_Button_lose");

        //자원 텍스트
        food_text_lable = root.Q<Label>("food_text");
        wood_text_lable = root.Q<Label>("wood_text");

        //턴 값 텍스트
        turn_value_lable = root.Q<Label>("turn_value");

        //턴엔드 클릭시 나오는 깃발모양 표시지
        turn_visual = root.Q<VisualElement>("turn_visual");
        turn_visual_lable = root.Q<Label>("turn_visual_label");

    }

    void Update() //ui에 있는 버튼 클릭이랑 click이벤트의 마우스클릭이랑 동시에 실행되서 따로 만듬
    {
        if (GameObject.Find("Resourse").GetComponent<Resourse>().the_world == 0)
        {
            //turn_end버튼 클릭
            turn_end.RegisterCallback<ClickEvent>(OnTurnEndButtonClicked);
        }


        //빌딩버튼 클릭
        spawn_base_building_B_button.RegisterCallback<ClickEvent>((e) => OnSpawnBuildingButtonClicked(e, 1));
        spawn_sword_building_B_button.RegisterCallback<ClickEvent>((e) => OnSpawnBuildingButtonClicked(e, 2));
        spawn_ax_building_B_button.RegisterCallback<ClickEvent>((e) => OnSpawnBuildingButtonClicked(e, 3));
        spawn_shield_building_B_button.RegisterCallback<ClickEvent>((e) => OnSpawnBuildingButtonClicked(e, 4));

        //유닛 버튼 클릭
        spawn_base_blueUnit_button.RegisterCallback<ClickEvent>((e) => OnSpawn_UnitButtonClicked(e, 0)); //기본유닛
        spawn_knife_blueUnit_button.RegisterCallback<ClickEvent>((e) => OnSpawn_UnitButtonClicked(e, 1)); //검 
        spawn_axe_blueUnit_button.RegisterCallback<ClickEvent>((e) => OnSpawn_UnitButtonClicked(e, 2));//도끼
        spawn_shield_blueUnit_button.RegisterCallback<ClickEvent>((e) => OnSpawn_UnitButtonClicked(e, 3)); //방패

        //리턴 버튼 클릭시
        return_button1.RegisterCallback<ClickEvent>(OnReturnButtonClicked);
        return_button2.RegisterCallback<ClickEvent>(OnReturnButtonClicked);

        food_text_lable.text = GameObject.Find("Resourse").GetComponent<Resourse>().food.ToString(); //음식 갯수 표기
        wood_text_lable.text = GameObject.Find("Resourse").GetComponent<Resourse>().wood.ToString(); //나무 갯수 표기
        turn_value_lable.text = GameObject.Find("Resourse").GetComponent<Resourse>().turn_value.ToString(); //턴 표시

        //턴엔드버튼 클릭시 이미지 표시
        if (GameObject.Find("Resourse").GetComponent<Resourse>().the_world == 1)
        {
            turn_visual.RemoveFromClassList("turn_visual_out");
            //turn_visual_label값 변경
            turn_visual_lable.text = GameObject.Find("Resourse").GetComponent<Resourse>().turn_value.ToString() + "턴";
        }
        else
        {
            turn_visual.AddToClassList("turn_visual_out");
            //turn_visual_label값 변경
            turn_visual_lable.text = " ";
        }
        
        
    }


    //턴종료 이벤트 실행
    public void OnTurnEndButtonClicked(ClickEvent evt)
    {
        GameObject.Find("Ui").GetComponent<TurnEnd>().turn_End();
    }

    //spawn건물 버튼 클릭
    public void OnSpawnBuildingButtonClicked(ClickEvent evt,int type_value)
    {
        //건물의 타일을 결정할 type_value의 값을 보낸다
        GameObject.Find("Ui").GetComponent<Build>().build_Button_Click(type_value); 
        unitUnderbar.RemoveFromClassList("ui_button_up");
        if (building_ShowOn == true)
        {
            building_ShowOn = false;
        }
       
    }

    //유닛 소환
    //유닛 타입을 번호별로 소환되게 하는건 spawnperson스크립트에서 결정할거임
    public void OnSpawn_UnitButtonClicked(ClickEvent evt, int unit_number)
    {

        unitType_DecisionVariable = unit_number; //기본 유닛 타입 번호 0
        spawnUnitFunction();    //소환

        base_buildingUnderbar_B.RemoveFromClassList("ui_button_up"); //ui창 닫기
        sword_buildingUnderbar_B.RemoveFromClassList("ui_button_up"); //ui창 닫기
        ax_buildingUnderbar_B.RemoveFromClassList("ui_button_up"); //ui창 닫기
        shield_buildingUnderbar_B.RemoveFromClassList("ui_button_up"); //ui창 닫기

    }


    //유닛소환
    public void spawnUnitFunction()
    {
        GameObject.Find("Ui").GetComponent<SpawnPerson>().person_button_click();
        if (base_person_ShowOn == true || sword_person_ShowOn == true || ax_person_ShowOn == true || shield_person_ShowOn == true )
        {
            base_person_ShowOn = false; //혹시 몰라서 showon을 false로 변경
            sword_person_ShowOn = false;
            ax_person_ShowOn = false;
            shield_person_ShowOn = false;
        }
    }

    //유닛 ui 관련
    public void ui_Open_Down(GameObject click_obj)
    {
        if (click_obj != null)
        {
            //unit클릭시 나오는 빌딩 ui 애니메이션
            if (click_obj.tag == "Unit" && 
                GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)click_obj.transform.position.x, (int)click_obj.transform.position.y] == 1)  //기본유닛일때만
            {
                //스타일시트 가져오기(애니메이션 효과 적용)
                unitUnderbar.AddToClassList("ui_button_up");
                building_ShowOn = true;
            }
            else
            {
                if (building_ShowOn == true)
                {
                    //스타일시트 제거하기(애니메이션 효과 적용)
                    unitUnderbar.RemoveFromClassList("ui_button_up");
                    building_ShowOn = false;
                }
            }

            //person ui 애니메이션
            //base_buildingUnderbar_B ui창
            if (click_obj.tag == "Building" &&
                GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)click_obj.transform.position.x, (int)click_obj.transform.position.y] == 10)
            {
                base_person_ShowOn = true;
                //스타일시트 가져오기(애니메이션 효과 적용)
                base_buildingUnderbar_B.AddToClassList("ui_button_up");
            }
            else
            {
                if (base_person_ShowOn == true)
                {
                    //스타일시트 제거하기(애니메이션 효과 적용)
                    base_buildingUnderbar_B.RemoveFromClassList("ui_button_up");
                    base_person_ShowOn = false;
                }
            }
            //sword_buildingUnderbar_B ui창
            if (click_obj.tag == "Building" &&
                GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)click_obj.transform.position.x, (int)click_obj.transform.position.y] == 11)
            {
                sword_person_ShowOn = true;
                //스타일시트 가져오기(애니메이션 효과 적용)
                sword_buildingUnderbar_B.AddToClassList("ui_button_up");
            }
            else
            {
                if (sword_person_ShowOn == true)
                {
                    //스타일시트 제거하기(애니메이션 효과 적용)
                    sword_buildingUnderbar_B.RemoveFromClassList("ui_button_up");
                    sword_person_ShowOn = false;
                }
            }
            //ax_buildingUnderbar_B ui창
            if (click_obj.tag == "Building" &&
                GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)click_obj.transform.position.x, (int)click_obj.transform.position.y] == 12)
            {
                ax_person_ShowOn = true;
                //스타일시트 가져오기(애니메이션 효과 적용)
                ax_buildingUnderbar_B.AddToClassList("ui_button_up");
            }
            else
            {
                if (ax_person_ShowOn == true)
                {
                    //스타일시트 제거하기(애니메이션 효과 적용)
                    ax_buildingUnderbar_B.RemoveFromClassList("ui_button_up");
                    ax_person_ShowOn = false;
                }
            }
            //shield_buildingUnderbar_B ui창
            if (click_obj.tag == "Building" &&
                GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)click_obj.transform.position.x, (int)click_obj.transform.position.y] == 13)
            {
                shield_person_ShowOn = true;
                //스타일시트 가져오기(애니메이션 효과 적용)
                shield_buildingUnderbar_B.AddToClassList("ui_button_up");
            }
            else
            {
                if (shield_person_ShowOn == true)
                {
                    //스타일시트 제거하기(애니메이션 효과 적용)
                    shield_buildingUnderbar_B.RemoveFromClassList("ui_button_up");
                    shield_person_ShowOn = false;
                }
            }


        }
    }

    //리턴버튼 클릭
    public void OnReturnButtonClicked(ClickEvent evt)
    {
        SceneManager.LoadScene("Start_screen_scene"); //Start_play_scene 신을 불러오겠다.
    }

    //승리 or 패배시 뜨는 ui
    //GamePlay 스크립트에서 사용
    public void game_Win_Ui()
    {
        game_win.style.display = DisplayStyle.Flex;
    }
    public void game_Lose_Ui()
    {
        game_lose.style.display = DisplayStyle.Flex;
    }
}
