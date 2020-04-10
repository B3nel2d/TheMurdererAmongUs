//====================================================================================================
//
//  TextSettings
//
//  テキストの持つ設定を管理するクラス
//
//====================================================================================================

using UnityEngine;

public class TextSettings{

    /// <summary>
    /// ローカライズID
    /// </summary>
    public string id{
        get;
        set;
    }

    /// <summary>
    /// テキスト文字列
    /// </summary>
    public string text{
        get;
        set;
    }

    /// <summary>
    /// フォント
    /// </summary>
    public Font font{
        get;
        set;
    }

    /// <summary>
    /// フォントのスタイル
    /// </summary>
    public FontStyle fontStyle{
        get;
        set;
    }

    /// <summary>
    /// フォントのサイズ
    /// </summary>
    public int fontSize{
        get;
        set;
    }

    /// <summary>
    /// 行間の間隔
    /// </summary>
    public float lineSpacing{
        get;
        set;
    }

    /******************************/

    /// <summary>
    /// コンストラクター
    /// </summary>
    public TextSettings(string id){
        this.id = id;
    }
    /// <summary>
    /// コンストラクター
    /// </summary>
    public TextSettings(string id, string text, Font font, FontStyle fontStyle, int fontSize, float lineSpacing){
        this.id = id;
        this.text = text;
        this.font = font;
        this.fontStyle = fontStyle;
        this.fontSize = fontSize;
        this.lineSpacing = lineSpacing;
    }

}
