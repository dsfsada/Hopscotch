using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

//ui 확인


public class SpawnPerson : MonoBehaviour
{
    public bool spawn_possible = false;
    //unit소환용 코드
    public void person_button_click()
    {
        spawn_possible = true;

        //클릭이벤트 스크립트에서 click_Obj의 위치값을 가져온다.
        Vector3 clickObjPosition = GameObject.Find("ClickManager").GetComponent<ClickEventScript>().save_Obj_Foruibutton.transform.position;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i != 1 || j != 1)
                {

                     // spawn 에 있던 확인용이 여기로 이동함.
                     if (GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[(int)clickObjPosition.x + i - 1, (int)clickObjPosition.y + j - 1] == 0)
                     {
                         //리소스 스크립트에 존재하는 spawn_value값에 클릭한 오브젝트 주위의 위치들을 저장
                         Vector2 unit_spawn_possible = new Vector2(clickObjPosition.x + i - 1, clickObjPosition.y + j - 1);
                         GameObject.Find("Resourse").GetComponent<Resourse>().spawn_value[i, j] = unit_spawn_possible;
                     }

                }
            }
        }
    }

    //person(unit)버튼 클릭후 바닥에 클릭시 unit생성 
    public void unitSummon_ClickOnFloor(Vector2 click_Obj_position)
    {
        int unitTypeNumber = GameObject.Find("Ui").GetComponent<UiScript>().unitType_DecisionVariable; //유닛 타입번호에 따른 소환 (0이면 기본,1이면 아군 검, 2방패, 3도끼)
        int blue_food = GameObject.Find("Resourse").GetComponent<Resourse>().food;
        if (spawn_possible == true) //spawnPerson 스크립트에서 person_button을 클릭하고 나면 실행 가능한 if 문
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    //건물 주위의 타일 위치값들과 선택한 타일의 위치값을 비교해서 맞는 위치에 유닛 소환
                    if (click_Obj_position == GameObject.Find("Resourse").GetComponent<Resourse>().spawn_value[i, j]
                       && GameObject.Find("Resourse").GetComponent<Resourse>().visible_Tile[(int)click_Obj_position.x, (int)click_Obj_position.y] != 0)
                    {
                        if (unitTypeNumber == 0) //0번 타입의 유닛 소환(기본유닛)
                        {
                            if (blue_food >= 40)
                            {
                                GameObject.Find("SpawnObj").GetComponent<SpawnObj>().SpawnBasicUnit(click_Obj_position); //기본 유닛 소환
                                GameObject.Find("Resourse").GetComponent<Resourse>().food -= 40;
                            }                                                             //추가로 식량 자원 감소 코드같은거 넣으면 될 듯!!!
                        }
                        else if (unitTypeNumber == 1) //1번 타입의 유닛 소환(아군 검)
                        {
                            if (blue_food >= 70)
                            {
                                GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_knife_Blue_Unit(click_Obj_position);
                                GameObject.Find("Resourse").GetComponent<Resourse>().food -= 70;
                            }
                        }
                        else if (unitTypeNumber == 2) //2번 타입의 유닛 소환(아군 도끼)
                        {
                            if (blue_food >= 70)
                            {
                              
                                GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_axe_Blue_Unit(click_Obj_position);
                                GameObject.Find("Resourse").GetComponent<Resourse>().food -= 70;
                            }
                        }
                        else if (unitTypeNumber == 3) //3번 타입의 유닛 소환(아군 방패)
                        {
                            if (blue_food >= 70)
                            {
                                GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_shield_Blue_Unit(click_Obj_position);
                                GameObject.Find("Resourse").GetComponent<Resourse>().food -= 70;
                            }
                        }

                    }
                }
            }
        }
    }
}
