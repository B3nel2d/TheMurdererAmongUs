//====================================================================================================
//
//  Character
//
//  パーティー参加者(キャラクター)の基底クラス
//
//====================================================================================================

using System.Collections.Generic;

public class Character{

    /// <summary>
    /// キャラクターの状態
    /// </summary>
    public enum State{
        Alive,
        Attacked,
        Dead
    }

    /// <summary>
    /// 他プレイヤーから見た評価
    /// </summary>
    public enum Impression{
        Unknown,
        Friendly,
        Unfriendly
    }

    /// <summary>
    /// 前のラウンドでの行動
    /// </summary>
    public enum Action{
        Nothing,
        Ability,
        Move,
    }

    /******************************/

    /// <summary>
    /// 現在の状態
    /// </summary>
    public State state{
        get;
        set;
    }

    /// <summary>
    /// キャラクター名
    /// </summary>
    public GameManager.CharacterName name{
        get;
        set;
    }

    /// <summary>
    /// 各プレイヤーからの評価
    /// </summary>
    public Dictionary<Player, Impression> impressions{
        get;
        set;
    }

    /// <summary>
    /// 現在いる部屋
    /// </summary>
    public GameManager.Location? location{
        get;
        set;
    }
    /// <summary>
    /// 次のラウンドでの移動先
    /// </summary>
    public GameManager.Location? destination{
        get;
        set;
    }

    /// <summary>
    /// そのラウンドでの行動
    /// </summary>
    public Action? action{
        get;
        set;
    }
    /// <summary>
    /// 前回のラウンドでの行動
    /// </summary>
    public Action? previousAction{
        get;
        set;
    }
    /// <summary>
    /// 行動のログ
    /// </summary>
    public string actionMessage{
        get;
        set;
    }
    /// <summary>
    /// 前回の行動のログ
    /// </summary>
    public string previousActionMessage{
        get;
        set;
    }

    /// <summary>
    /// 殺害者
    /// </summary>
    public Player killer{
        get;
        set;
    }
    /// <summary>
    /// 死因
    /// </summary>
    public string causeOfDeath{
        get;
        set;
    }

    /// <summary>
    /// 死体が隠されているか
    /// </summary>
    public bool isBodyHidden{
        get;
        set;
    }
    /// <summary>
    /// 死体が次のラウンドから隠されるか
    /// </summary>
    public bool isBodyToBeHidden{
        get;
        set;
    }

    /******************************/

    /// <summary>
    /// コンストラクター
    /// </summary>
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
