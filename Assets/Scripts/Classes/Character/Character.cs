using System.Collections.Generic;

public class Character{

    public enum State{
        Alive,
        Attacked,
        Dead
    }

    public enum Impression{
        Unknown,
        Friendly,
        Unfriendly
    }

    public enum Action{
        Nothing,
        Ability,
        Move,
    }

    /******************************/

    public State state{
        get;
        set;
    }

    public GameManager.CharacterName name{
        get;
        set;
    }

    public Dictionary<Player, Impression> impressions{
        get;
        set;
    }

    public GameManager.Location? location{
        get;
        set;
    }
    public GameManager.Location? destination{
        get;
        set;
    }

    public Action? action{
        get;
        set;
    }
    public Action? previousAction{
        get;
        set;
    }
    public string actionMessage{
        get;
        set;
    }
    public string previousActionMessage{
        get;
        set;
    }

    public Player killer{
        get;
        set;
    }
    public string causeOfDeath{
        get;
        set;
    }

    public bool isBodyHidden{
        get;
        set;
    }
    public bool isBodyToBeHidden{
        get;
        set;
    }

    /******************************/

    public Character(){
        state = State.Alive;

        impressions = new Dictionary<Player, Impression>();

        location = null;
        destination = null;

        action = Action.Nothing;
        previousAction = Action.Nothing;
        actionMessage = null;
        previousActionMessage = null;

        killer = null;
        causeOfDeath = null;

        isBodyHidden = false;
        isBodyToBeHidden = false;
    }

}
