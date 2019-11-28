using UnityEngine;

public class Feint : Ability{

    public override string name{
        get{
            switch(Localization.language){
                case Localization.Language.English:
                    return "Feint";

                case Localization.Language.Japanese:
                    return "探索の振り";
            }

            return null;
        }
    }

    /******************************/

    public Feint(){
        isTargetable = false;
    }

    /******************************/

    override public void Use(Player user){
        base.Use(user);

        switch(Localization.language){
            case Localization.Language.English:
                user.actionMessage = "SEARCHED";
                resultMessage = "You pretended to\nsearch for the key.";

                break;
            case Localization.Language.Japanese:
                user.actionMessage = "探索";
                resultMessage = "鍵を探す振りをしました。";

                break;
        }
    }

}
