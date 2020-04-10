//====================================================================================================
//
//  ExtendedButton
//
//  UnityのButtonの拡張クラス
//
//====================================================================================================

using UnityEngine;
using UnityEngine.UI;

public class ExtendedButton : Button{

    /// <summary>
    /// 標準のテキストの色
    /// </summary>
    private Color defaultColor;

    protected override void Awake(){
        base.Awake();

        foreach(Text text in gameObject.GetComponentsInChildren<Text>()){
            if(text.tag == "ButtonText"){
                defaultColor = text.color;
            }
        }
    }

    /// <summary>
    /// ボタンの状態遷移に合わせたテキスト色の変更
    /// </summary>
    /// <param name="state"></param>
    /// <param name="instant"></param>
    protected override void DoStateTransition(SelectionState state, bool instant){
        base.DoStateTransition(state, instant);

        int count = 0;
        foreach(Text text in gameObject.GetComponentsInChildren<Text>()){
            if(text.tag == "ButtonText"){
                switch(state){
                    case SelectionState.Normal:
                        text.color = defaultColor;
                        break;
                    case SelectionState.Highlighted:
                        text.color = Color.gray;
                        break;
                    case SelectionState.Pressed:
                        text.color = Color.red;
                        break;
                    case SelectionState.Selected:
                        text.color = defaultColor;
                        break;
                    case SelectionState.Disabled:
                        text.color = Color.gray;
                        break;
                }

                count++;
            }
        }
    }

}
