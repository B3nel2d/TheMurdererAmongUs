//====================================================================================================
//
//  Player
//
//  プレイヤーを規定するクラス
//
//====================================================================================================

using System;
using System.Collections.Generic;

public class Player : Character{

    public enum Side{
        Murderer,
        Innocents
    }

    public enum Role{
        Murderer,
        Betrayer,
        Policeman,
        Detective,
        Bystander
    }

    public enum Result{
        Win,
        Lose
    }

    /******************************/

    public string id{
        get;
        private set;
    }

    public Side side{
        get;
        set;
    }

    public Role role{
        get;
        set;
    }

    public Result result{
        get;
        set;
    }

    public List<Ability> abilities{
        get;
        set;
    }

    public bool foundKeyPeace{
        get;
        set;
    }

    public bool roleConfirmed{
        get;
        set;
    }

    public GameManager.Location keyPeaceLocation{
        get;
        set;
    }
    public Dictionary<GameManager.Location?, bool> searchedLocations{
        get;
        set;
    }

    public List<Character> metCharacters{
        get;
        set;
    }

    /******************************/

    public Player(string id){
        this.id = id;

        side = Side.Innocents;
        role = Role.Bystander;
        result = Result.Lose;

        abilities = new List<Ability>();

        foundKeyPeace = false;

        roleConfirmed = false;
        
        keyPeaceLocation = GameManager.Location.MainEntrance;
        searchedLocations = new Dictionary<GameManager.Location?, bool>();

        foreach(GameManager.Location location in Enum.GetValues(typeof(GameManager.Location))){
            searchedLocations.Add(location, false);
        }

        metCharacters = new List<Character>();
    }

}
