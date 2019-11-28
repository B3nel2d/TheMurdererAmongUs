using UnityEngine;

public class HideDeadbody : Ability{

    public override string name{
        get{
            switch(Localization.language){
                case Localization.Language.English:
                    return "Hide Body";

                case Localization.Language.Japanese:
                    return "証拠隠滅";
            }

            return null;
        }
    }

    /******************************/

    public HideDeadbody(){
        isTargetable = true;
    }

    /******************************/

    override public void Use(Player user, Character target){
        base.Use(user, target);

        target.isBodyToBeHidden = true;

        switch(Localization.language){
            case Localization.Language.English:
                user.actionMessage = "HID " + Localization.LocalizeCharacterName(target.name).ToUpper() + "'S BODY";
                resultMessage = "You hid the body!";

                break;
            case Localization.Language.Japanese:
                user.actionMessage = Localization.LocalizeCharacterName(target.name) + "の死体を隠した";
                resultMessage = "死体を誰にも\n見つからない場所へ\n隠しました。";

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
