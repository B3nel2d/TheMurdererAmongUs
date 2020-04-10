//====================================================================================================
//
//  Search
//
//  能力「探索」を規定するクラス
//
//====================================================================================================

using UnityEngine;

public class Search : Ability{

    public override string name{
        get{
            switch(Localization.language){
                case Localization.Language.English:
                    return "Search";

                case Localization.Language.Japanese:
                    return "探索";
            }

            return null;
        }
    }

    /******************************/

    public Search(){
        isTargetable = false;
    }

    /******************************/

    override public void Use(Player user){
        base.Use(user);

        user.searchedLocations[user.location] = true;

        switch(Localization.language){
            case Localization.Language.English:
                user.actionMessage = "SEARCHED";

                if(user.keyPeaceLocation == user.location){
                    user.foundKeyPeace = true;

                    resultMessage = "You found a clue!";
                }
                else{
                    resultMessage = "You could not find anything...";
                }

                break;
            case Localization.Language.Japanese:
                user.actionMessage = "探索";

                if(user.keyPeaceLocation == user.location){
                    user.foundKeyPeace = true;

                    resultMessage = "手掛かりを見つけました！";
                }
                else{
                    resultMessage = "何も見つけられませんでした...";
                }

                break;
        }
    }

    public override bool FulfillsCondition(Player player){
        if(GameManager.instance.round < 3){
            switch(Localization.language){
                case Localization.Language.English:
                    resultMessage = "You cannot search\nuntil <b>3</b> rounds pass.\n(<b>" + (3 - GameManager.instance.round) + "</b> rounds left)";

                    break;
                case Localization.Language.Japanese:
                    resultMessage = "<b>3</b>ラウンドが経過するまで\n探索を行う事は出来ません。\n(残り<b>" + (3 - GameManager.instance.round) + "</b>ラウンド)";

                    break;
            }

            return false;
        }
        else if(player.location == GameManager.Location.MainEntrance){
            switch(Localization.language){
                case Localization.Language.English:
                    resultMessage = "You cannot find\nanything here.";

                    break;
                case Localization.Language.Japanese:
                    resultMessage = "ここに手掛かりはないようです。";

                    break;
            }

            return false;
        }

        return true;
    }

}
