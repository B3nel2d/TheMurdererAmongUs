//====================================================================================================
//
//  Autopsy
//
//  能力「検死」を規定するクラス
//
//====================================================================================================

using System;

public class Autopsy : Ability{

    public override string name{
        get{
            switch(Localization.language){
                case Localization.Language.English:
                    return "Autopsy";

                case Localization.Language.Japanese:
                    return "検死";
            }

            return null;
        }
    }

    /******************************/

    public Autopsy(){
        isTargetable = true;
    }

    /******************************/

    override public void Use(Player user, Character target){
        base.Use(user, target);

        switch(Localization.language){
            case Localization.Language.English:
                user.actionMessage = "EXAMINED " + Localization.LocalizeCharacterName(target.name).ToUpper() + "'S BODY";
                resultMessage = "It seems this dead body was\n killed by <b>" + Localization.LocalizeCharacterName(target.killer.name) + "</b>\nusing <b>" + target.causeOfDeath + "</b>...";

                break;
            case Localization.Language.Japanese:
                user.actionMessage = Localization.LocalizeCharacterName(target.name) + "の死体を調べた";
                resultMessage = "この死体は<b>" + Localization.LocalizeCharacterName(target.killer.name) + "</b>によって\n<b>" + target.causeOfDeath + "</b>を用いて\n殺害されたようです。";

                break;
        }
    }

    public override bool FulfillsCondition(Character target){
        if(target.state == Player.State.Dead && !target.isBodyHidden){
            return true;
        }

        return false;
    }

}
