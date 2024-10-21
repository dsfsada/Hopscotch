using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnEnd : MonoBehaviour
{
    //ui -> canvas -> turn_end에서 사용중

    public void turn_End() //턴 종료 버튼
    {
        GameObject.Find("Resourse").GetComponent<Resourse>().the_world = 1;//클릭 못하게 제함
        Befor_Fight();
        Invoke("after_Fight", 1);
    }

    private void Befor_Fight()
    {
        GameObject.Find("Resourse").GetComponent<Resourse>().turn_value += 1; //턴 증가
        GameObject.Find("Resourse").GetComponent<Resourse>().draw_Unit();
        GameObject.Find("Resourse").GetComponent<Resourse>().update_Visible();
        //시야 그리기 ( 영향력 또한 포함
        GameObject.Find("DrawField").GetComponent<Draw_Field>().draw_Dark(
            GameObject.Find("Resourse").GetComponent<Resourse>().visible_Tile,
            GameObject.Find("Resourse").GetComponent<Resourse>().blue_Influence_Tile,
            GameObject.Find("Resourse").GetComponent<Resourse>().red_Influence_Tile,
            GameObject.Find("Resourse").GetComponent<Resourse>().width, //가로
            GameObject.Find("Resourse").GetComponent<Resourse>().height//세로
            );
    }
    private void after_Fight()
    {
        //유닛 전투 시작
        GameObject.Find("Resourse").GetComponent<Resourse>().update_Fight_Unit();
        /*GameObject.Find("SpawnObj").GetComponent<SpawnObj>().update_Unit_Draw();*/ // 현재 updat_Fight_Unit () 내에서 작동중
        GameObject.Find("Resourse").GetComponent<Resourse>().draw_Unit();
        //시야 재 설정
        GameObject.Find("Resourse").GetComponent<Resourse>().update_Visible();
        //영향력 계산
        GameObject.Find("Resourse").GetComponent<Resourse>().update_Influence();
        //나무 식량 추가
        GameObject.Find("Resourse").GetComponent<Resourse>().update_Resourse();
        //시야 그리기 ( 영향력 또한 포함
        GameObject.Find("DrawField").GetComponent<Draw_Field>().draw_Dark(
            GameObject.Find("Resourse").GetComponent<Resourse>().visible_Tile,
            GameObject.Find("Resourse").GetComponent<Resourse>().blue_Influence_Tile,
            GameObject.Find("Resourse").GetComponent<Resourse>().red_Influence_Tile,
            GameObject.Find("Resourse").GetComponent<Resourse>().width, //가로
            GameObject.Find("Resourse").GetComponent<Resourse>().height//세로
            );
        // 이동

        GameObject.Find("Resourse").GetComponent<Resourse>().activeEnemy(); // 이동 생성 동시 실행

        // 생성

        GameObject.Find("Resourse").GetComponent<Resourse>().the_world = 0; // 다시 클릭할 수 있습니다.
    }

}
