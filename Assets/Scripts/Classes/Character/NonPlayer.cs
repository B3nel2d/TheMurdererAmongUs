using System;
using System.Collections.Generic;

public class NonPlayer : Character{

    public enum Personality{
        Normal
    }

    /******************************/

    public Personality personality{
        get;
        set;
    }

    float chanceToMove;

    /******************************/

    public NonPlayer(){
        personality = Personality.Normal;

        switch(personality){
            default:
                chanceToMove = 0.3f;

                break;
        }
    }

    /******************************/

    public void Act(){
        float random = UnityEngine.Random.Range(0f, 1f);

        if(random <= chanceToMove){
            Move();
        }
        else{
            action = Action.Nothing;
        }
    }

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
