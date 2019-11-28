using UnityEngine;

public class TextSettings{

    public string id{
        get;
        set;
    }

    public string text{
        get;
        set;
    }

    public Font font{
        get;
        set;
    }

    public FontStyle fontStyle{
        get;
        set;
    }

    public int fontSize{
        get;
        set;
    }

    public float lineSpacing{
        get;
        set;
    }

    /******************************/

    public TextSettings(string id){
        this.id = id;
    }
    public TextSettings(string id, string text, Font font, FontStyle fontStyle, int fontSize, float lineSpacing){
        this.id = id;
        this.text = text;
        this.font = font;
        this.fontStyle = fontStyle;
        this.fontSize = fontSize;
        this.lineSpacing = lineSpacing;
    }

}
