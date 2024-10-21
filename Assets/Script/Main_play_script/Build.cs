using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEditor.PlayerSettings;

//ui->canvas->build_button에서 사용

public class Build : MonoBehaviour
{
    //클릭한 위치의 object파괴 후 새로운 건물 생성을 위한 코드
    GameObject objectToRemove = null;

    public GameObject base_building_B;
    public GameObject sword_building_B;
    public GameObject ax_building_B;
    public GameObject shield_building_B;

    public void build_Button_Click(int type_value)
    {
        //클릭이벤트 스크립트에서 click_Obj의 위치값을 가져온다.
        Vector3 clickObjPosition = GameObject.Find("ClickManager").GetComponent<ClickEventScript>().save_Obj_Foruibutton.transform.position;

        List<GameObject> blueUnitList = GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit;
        //Resourse 스크립트의 blue_Unit 리스트 가져오기

        //테두리타일 건물생성 불가능(유닛생성때 오류발생해서)
        if ((int)clickObjPosition.x != 0 && (int)clickObjPosition.y != 0 && (int)clickObjPosition.x != 101 && (int)clickObjPosition.y != 101)
        {
            foreach (GameObject obj in blueUnitList)
            {
                if (obj.transform.position == clickObjPosition)
                {
                    objectToRemove = obj;
                    break;
                }
                //클릭한 유닛 오브젝트의 위치와 리스트의 위치값이 같으면 objectToRemove의 obj의 값이 들어감
            }

            //건물번호(blue_Unit_Tile) 및 object결정 ,10(type_value = 1):블루 기본, 11(type_value = 2):블루 검, 12(type_value = 3):블루 도끼, 13(type_value = 4):블루 방패
            if (objectToRemove != null)
            {
                int blue_wood = GameObject.Find("Resourse").GetComponent<Resourse>().wood;  //자원소비
                if(blue_wood >= 50 && type_value == 1) {

                    GameObject.Find("Resourse").GetComponent<Resourse>().wood -= 50;
                    //현재 obj의 위치에 있는 타일의 값 건물값으로 변경
                    blueUnitList.Remove(objectToRemove); // List에서 해당 오브젝트 제거
                    Destroy(objectToRemove); // 해당 오브젝트 파괴

                    GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_Tower_B(clickObjPosition);

                }

                if (blue_wood >= 70 && type_value == 2)
                {

                    GameObject.Find("Resourse").GetComponent<Resourse>().wood -= 70;
                    //현재 obj의 위치에 있는 타일의 값 건물값으로 변경
                    blueUnitList.Remove(objectToRemove); // List에서 해당 오브젝트 제거
                    Destroy(objectToRemove); // 해당 오브젝트 파괴

                    GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_Sword_Tower_B(clickObjPosition);

                }

                if (blue_wood >= 70 && type_value == 3)
                {

                    GameObject.Find("Resourse").GetComponent<Resourse>().wood -= 70;
                    //현재 obj의 위치에 있는 타일의 값 건물값으로 변경
                    blueUnitList.Remove(objectToRemove); // List에서 해당 오브젝트 제거
                    Destroy(objectToRemove); // 해당 오브젝트 파괴

                    GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_Ax_Tower_B(clickObjPosition);

                }

                if (blue_wood >= 70 && type_value == 4)
                {

                    GameObject.Find("Resourse").GetComponent<Resourse>().wood -= 70;
                    //현재 obj의 위치에 있는 타일의 값 건물값으로 변경
                    blueUnitList.Remove(objectToRemove); // List에서 해당 오브젝트 제거
                    Destroy(objectToRemove); // 해당 오브젝트 파괴

                    GameObject.Find("SpawnObj").GetComponent<SpawnObj>().Spawn_Shield_Tower_B(clickObjPosition);

                }
            }




        }
    }
}
    
