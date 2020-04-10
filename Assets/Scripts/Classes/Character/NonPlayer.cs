//====================================================================================================
//
//  NonPlayer
//
//  NPCを規定するクラス
//
//====================================================================================================

using System;
using System.Collections.Generic;

public class NonPlayer : Character{

    /// <summary>
    /// 性格の種類
    /// </summary>
    public enum Personality{
        Normal
    }

    /******************************/

    /// <summary>
    /// 性格
    /// </summary>
    public Personality personality{
        get;
        set;
    }

    /// <summary>
    /// 移動を行う確率
    /// </summary>
    float chanceToMove;

    /******************************/

    /// <summary>
    /// コンストラクター
    /// </summary>
    public NonPlayer(){
        personality = Personality.Normal;

        switch(personality){
            default:
                chanceToMove = 0.3f;

                break;
        }
    }

    /******************************/

    /// <summary>
    /// 行動の決定
    /// </summary>
    public void Act(){
        float random = UnityEngine.Random.Range(0f, 1f);

        if(random <= chanceToMove){
            Move();
        }
        else{
            action = Action.Nothing;
        }
    }

    /// <summary>
    /// 移動先の決定
    /// </summary>
    void Move(){
        List<GameManager.Location> adjacentLocations = new List<GameManager.Location>();

        if(location == GameManager.Location.MainEntrance){
            foreach(GameManager.Location location in Enum.GetValues(typeof(GameManager.Location))){
                if(location != GameManager.Location.MainEntrance){
                    adjacentLocations.Add(location);
                }
            }
        }
        else{
            adjacentLocations.Add(GameManager.Location.MainEntrance);

            int adjacentLocation = (int)location + 1;
            if(Enum.GetValues(typeof(GameManager.Location)).Length <= adjacentLocation){
                adjacentLocation = 1;
            }
            adjacentLocations.Add((GameManager.Location)adjacentLocation);

            adjacentLocation = (int)location - 1;
            if(adjacentLocation < 1){
                adjacentLocation = Enum.GetValues(typeof(GameManager.Location)).Length - 1;
            }
            adjacentLocations.Add((GameManager.Location)adjacentLocation);
        }

        destination = adjacentLocations.GetAtRandom();
        action = Action.Move;
    }

}
