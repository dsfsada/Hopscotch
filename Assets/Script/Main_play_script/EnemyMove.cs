using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    int[,] check_Unit = new int[,] { // 적이 감지할 타일
        { -1, -1 }, { -1, 0 }, { -1, 1 }, { 0, 1 }, { 1, 1 }, { 1, 0 }, { 1, -1 }, { 1, 1 },
        { -2, -2 }, { -2, -1 }, { -2, 0 }, { -2, 1 }, { -2, 2 }, { -1, 2 }, { 0, 2 }, { 1, 2 }, { 2, 2 }, {2, 1}, {2,0 }, {2,-1 },{2,-2 }, {1,-2 }, {0,-2 }, {-1,-2 },
        {-3,-3 }, {-3,-2},{-3,-1},{-3,0},{-3,1},{-3,2},{-3,3},{-2,3},{-1,3},{0,3},{1,3},{2,3},{3,3},{3,2},{3,1},{3,0},{3,-1},{3,-2},{3,-3},{2,-3},{1,-3},{0,-3},{-1,-3},{-2,-3}
    };

    public void MoveEnemy() //적이 유닛의 위치를 탐색하여 이동을 할 예정
    {
        Vector3 unitPos;
        unitPos = this.gameObject.transform.position;

        int unitType = 0;
        int cPosX = 0;
        int cPosY = 0;

        unitType = GameObject.Find("Resourse").GetComponent<Resourse>().red_Unit_Tile[(int)unitPos.x, (int)unitPos.y]; // 이동전 자신의 유닛 종류 저장

        for (int i = 0; i < check_Unit.GetLength(0); i++)
        {

            int searchTileX = (int)unitPos.x + check_Unit[i, 0];    
            int searchTileY = (int)unitPos.x + check_Unit[i, 1];

            if(searchTileX < 0)
            {
                searchTileX = 0;
            }
            else if(searchTileX > 30)
            {
                searchTileX = 30;
            }

            if(searchTileY < 0)
            {
                searchTileY = 0;
            }
            else if (searchTileY > 30)
            {
                searchTileY = 30;
            }

            if (GameObject.Find("Resourse").GetComponent<Resourse>().blue_Unit_Tile[searchTileX, searchTileY] > 0) // 검사한 blue유닛타일이 0보다 크다는것은 blue유닛의 오브젝트가 있음을 의미
            {
                if (check_Unit[i, 0] > 0)
                {
                    cPosX = 1;
                }                                                        
                else if (check_Unit[i, 0] < 0)                           
                {
                    cPosX = -1;
                }                                                    
                if (check_Unit[i, 1] > 0)                            
                {                                                    
                    cPosY = 1;
                }                                                    
                else if (check_Unit[i, 1] < 0)                       
                {                                                    
                    cPosY = -1;
                }
                GameObject.Find("Resourse").GetComponent<Resourse>().red_Unit_Tile[(int)unitPos.x, (int)unitPos.y] = 0;
                GameObject.Find("Resourse").GetComponent<Resourse>().red_Unit_Tile[(int)unitPos.x + cPosX, (int)unitPos.y + cPosY] = unitType;
                break;
            }
        }
    }
}
