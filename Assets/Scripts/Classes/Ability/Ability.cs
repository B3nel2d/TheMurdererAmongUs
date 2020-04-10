//====================================================================================================
//
//  Ability
//
//  役職固有能力の基底クラス
//
//====================================================================================================

using UnityEngine;

public class Ability{

    /// <summary>
    /// 能力名
    /// </summary>
    virtual public string name{
        get;
    }

    /// <summary>
    /// 説明
    /// </summary>
    virtual public string description{
        get;
    }

    /// <summary>
    /// 対象を選択するか
    /// </summary>
    public bool isTargetable{
        get;
        protected set;
    }

    /// <summary>
    /// クールダウン(ラウンド単位)
    /// </summary>
    public int? cooldown{
        get;
        set;
    }

    /// <summary>
    /// 使用回数制限
    /// </summary>
    public int? residualUseCount{
        get;
        set;
    }

    /// <summary>
    /// 使用後のメッセージ
    /// </summary>
    public string resultMessage{
        get;
        protected set;
    }

    /******************************/

    /// <summary>
    /// コンストラクター
    /// </summary>
    public Ability(){
        isTargetable = false;
        cooldown = null;
        residualUseCount = null;
        resultMessage = null;
    }

    /// <summary>
    /// 能力の使用
    /// </summary>
    virtual public void Use(Player user){
        user.action = Character.Action.Ability;
    }
    /// <summary>
    /// 対象への能力の使用
    /// </summary>
    virtual public void Use(Player user, Character target){
        user.action = Character.Action.Ability;
    }

    /// <summary>
    /// 使用条件が満たされているか
    /// </summary>
    virtual public bool FulfillsCondition(Player user){
        return true;
    }
    /// <summary>
    /// 使用条件が満たされているか
    /// </summary>
    virtual public bool FulfillsCondition(Character target){
        return true;
    }

}
