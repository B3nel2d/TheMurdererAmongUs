using System;

public class Knife : Ability{

    public override string name{
        get{
            switch(Localization.language){
                case Localization.Language.English:
                    return "Knife";

                case Localization.Language.Japanese:
                    return "ナイフ";
            }

            return null;
        }
    }

    /******************************/

    public Knife(){
        isTargetable = true;
        cooldown = 0;
    }

    /******************************/

    override public void Use(Player user, Character target){
        base.Use(user, target);

        target.state = Character.State.Attacked;
        target.killer = user;
        target.causeOfDeath = name;
        cooldown = 2;

        switch(Localization.language){
            case Localization.Language.English:
                user.actionMessage = "KILLED " + Localization.LocalizeCharacterName(target.name).ToUpper() + " BY KNIFE";
                resultMessage = "You killed <b>" + Localization.LocalizeCharacterName(target.name) + "</b>!";

                break;
            case Localization.Language.Japanese:
                user.actionMessage = "ナイフで" + Localization.LocalizeCharacterName(target.name) + "を殺害した";
                resultMessage = "<b>" + Localization.LocalizeCharacterName(target.name) + "</b>を\nナイフで殺害しました！";

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
        else if(0 < cooldown){
            switch(Localization.language){
                case Localization.Language.English:
                    resultMessage = "You cannot use\nthis ability\ntwice in a row.";

                    break;
                case Localization.Language.Japanese:
                    resultMessage = "連続でこの能力を\n使用することはできません。";

                    break;
            }

            return false;
        }

        return true;
    }

    public override bool FulfillsCondition(Character target){
        if(target.state != Player.State.Dead){
            return true;
            
        }

        return false;
    }

}
