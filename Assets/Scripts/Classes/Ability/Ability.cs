using UnityEngine;

public class Ability{

    virtual public string name{
        get;
    }

    virtual public string description{
        get;
    }

    public bool isTargetable{
        get;
        protected set;
    }

    public int? cooldown{
        get;
        set;
    }

    public int? residualUseCount{
        get;
        set;
    }

    public string resultMessage{
        get;
        protected set;
    }

    /******************************/

    public Ability(){
        isTargetable = false;
        cooldown = null;
        residualUseCount = null;
        resultMessage = null;
    }

    virtual public void Use(Player user){
        user.action = Character.Action.Ability;
    }
    virtual public void Use(Player user, Character target){
        user.action = Character.Action.Ability;
    }

    virtual public bool FulfillsCondition(Player user){
        return true;
    }

    virtual public bool FulfillsCondition(Character target){
        return true;
    }

}
