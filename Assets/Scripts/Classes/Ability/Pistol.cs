//====================================================================================================
//
//  Pistol
//
//  能力「拳銃」を規定するクラス
//
//====================================================================================================

using System;

public class Pistol : Ability{

    public override string name{
        get{
            switch(Localization.language){
                case Localization.Language.English:
                    return "Pistol";

                case Localization.Language.Japanese:
                    return "拳銃";
            }

            return null;
        }
    }

    /******************************/

    public Pistol(){
        isTargetable = true;
    }

    /******************************/

    override public void Use(Player user, Character target){
        base.Use(user, target);

        target.state = Character.State.Attacked;
        target.killer = user;
        target.causeOfDeath = name;
        residualUseCount--;

        switch(Localization.language){
            case Localization.Language.English:
                user.actionMessage = "KILLED " + Localization.LocalizeCharacterName(target.name).ToUpper() + " BY PISTOL";
                resultMessage = "You killed <b>" + Localization.LocalizeCharacterName(target.name) + "</b>!";

                break;
            case Localization.Language.Japanese:
                user.actionMessage = Localization.LocalizeCharacterName(target.name) + "を拳銃で殺害した";
                resultMessage = "<b>" + Localization.LocalizeCharacterName(target.name) + "</b>を\n拳銃で殺害しました！";

                break;
        }
    }

    public override bool FulfillsCondition(Player user){
        if(GameManager.instance.round == 0){
            switch(Localization.language){
                case Localization.Language.English:
                    resultMessage = "You cannot use\nthis ability\nin the first round.";

                    break;
                case Localization.Language.Japanese:
                    resultMessage = "最初のラウンド中は\nこの能力を使用できません。";

                    break;
            }

            return false;
        }
        else if(residualUseCount == 0){
            switch(Localization.language){
                case Localization.Language.English:
                    resultMessage = "You already\nran out of bullets.";

                    break;
                case Localization.Language.Japanese:
                    resultMessage = "能力の使用回数上限に\n達しています。";

                    break;
            }

            return false;
        }

        return true;
    }

    public override bool FulfillsCondition(Character target){
        if(target.state == Player.State.Dead){
            return false;
        }

        return true;
    }

}
