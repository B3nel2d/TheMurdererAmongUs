using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public static class Localization{

    public enum Language{
        English,
        Japanese
    }

    /******************************/

    public static Language language{
        get;
        set;
    }

    private static Dictionary<Language, List<TextSettings>> localizations{
        get;
        set;
    }

    private static List<TextSettings> localizationTarget{
        get;
        set;
    }

    /******************************/

    public static void Setup(){
        localizations = new Dictionary<Language, List<TextSettings>>();
        localizationTarget = null;

        foreach(Language language in Enum.GetValues(typeof(Language))){
            ReadCsv("Data/Localization_" + language.ToString(), language);
        }
    }

    public static string LocalizeLanguageName(Language languageName){
        switch(languageName){
            case Language.English:
                switch(language){
                    case Language.English:
                        return "English";

                    case Language.Japanese:
                        return "英語";

                }

                break;
            case Language.Japanese:
                switch(language){
                    case Language.English:
                        return "Japanese";

                    case Language.Japanese:
                        return "日本語";

                }

                break;
        }

        Debug.Log("Failed to localize.");
        return null;
    }

    public static string LocalizeCharacterName(GameManager.CharacterName name){
        switch(name){
            case GameManager.CharacterName.Aligator:
                switch(language){
                    case Language.English:
                        return "Aligator";

                    case Language.Japanese:
                        return "ワニ";
                }

                break;
            case GameManager.CharacterName.Bear:
                switch(language){
                    case Language.English:
                        return "Bear";

                    case Language.Japanese:
                        return "クマ";
                }

                break;
            case GameManager.CharacterName.Cow:
                switch(language){
                    case Language.English:
                        return "Cow";

                    case Language.Japanese:
                        return "ウシ";
                }

                break;
            case GameManager.CharacterName.Deer:
                switch(language){
                    case Language.English:
                        return "Deer";

                    case Language.Japanese:
                        return "シカ";
                }

                break;
            case GameManager.CharacterName.Eagle:
                switch(language){
                    case Language.English:
                        return "Eagle";

                    case Language.Japanese:
                        return "ワシ";
                }

                break;
            case GameManager.CharacterName.Frog:
                switch(language){
                    case Language.English:
                        return "Frog";

                    case Language.Japanese:
                        return "カエル";
                }

                break;
            case GameManager.CharacterName.Goat:
                switch(language){
                    case Language.English:
                        return "Goat";

                    case Language.Japanese:
                        return "ヤギ";
                }

                break;
            case GameManager.CharacterName.Horse:
                switch(language){
                    case Language.English:
                        return "Horse";

                    case Language.Japanese:
                        return "ウマ";
                }

                break;
            case GameManager.CharacterName.Iguana:
                switch(language){
                    case Language.English:
                        return "Iguana";

                    case Language.Japanese:
                        return "イグアナ";
                }

                break;
            case GameManager.CharacterName.Jackal:
                switch(language){
                    case Language.English:
                        return "Jackal";

                    case Language.Japanese:
                        return "ジャッカル";
                }

                break;
            case GameManager.CharacterName.Koala:
                switch(language){
                    case Language.English:
                        return "Koala";

                    case Language.Japanese:
                        return "コアラ";
                }


                break;
            case GameManager.CharacterName.Lion:
                switch(language){
                    case Language.English:
                        return "Lion";

                    case Language.Japanese:
                        return "ライオン";
                }

                break;
            case GameManager.CharacterName.Monkey:
                switch(language){
                    case Language.English:
                        return "Monkey";

                    case Language.Japanese:
                        return "サル";
                }

                break;
            case GameManager.CharacterName.Newt:
                switch(language){
                    case Language.English:
                        return "Newt";

                    case Language.Japanese:
                        return "イモリ";
                }

                break;
            case GameManager.CharacterName.Owl:
                switch(language){
                    case Language.English:
                        return "Owl";

                    case Language.Japanese:
                        return "フクロウ";
                }

                break;
            case GameManager.CharacterName.Pig:
                switch(language){
                    case Language.English:
                        return "Pig";

                    case Language.Japanese:
                        return "ブタ";
                }

                break;
            case GameManager.CharacterName.Quetzal:
                switch(language){
                    case Language.English:
                        return "Quetzal";

                    case Language.Japanese:
                        return "ケツァール";
                }

                break;
            case GameManager.CharacterName.Rabbit:
                switch(language){
                    case Language.English:
                        return "Rabbit";

                    case Language.Japanese:
                        return "ウサギ";
                }

                break;
            case GameManager.CharacterName.Squirrel:
                switch(language){
                    case Language.English:
                        return "Squirrel";

                    case Language.Japanese:
                        return "リス";
                }

                break;
            case GameManager.CharacterName.Tiger:
                switch(language){
                    case Language.English:
                        return "Tiger";

                    case Language.Japanese:
                        return "トラ";
                }

                break;
            case GameManager.CharacterName.Uguisu:
                switch(language){
                    case Language.English:
                        return "Uguisu";

                    case Language.Japanese:
                        return "ウグイス";
                }

                break;
            case GameManager.CharacterName.Vulture:
                switch(language){
                    case Language.English:
                        return "Vulture";

                    case Language.Japanese:
                        return "ハゲタカ";
                }

                break;
            case GameManager.CharacterName.Wolf:
                switch(language){
                    case Language.English:
                        return "Wolf";

                    case Language.Japanese:
                        return "オオカミ";
                }

                break;
            case GameManager.CharacterName.Yak:
                switch(language){
                    case Language.English:
                        return "Yak";

                    case Language.Japanese:
                        return "ヤク";
                }

                break;
            case GameManager.CharacterName.Zebra:
                switch(language){
                    case Language.English:
                        return "Zebra";

                    case Language.Japanese:
                        return "シマウマ";
                }

                break;
        }

        Debug.Log("Failed to localize.");
        return null;
    }

    public static string LocalizeLocation(GameManager.Location? location){
        switch(location){
            case GameManager.Location.MainEntrance:
                switch(language){
                    case Language.English:
                        return "Main Entrance";

                    case Language.Japanese:
                        return "メインエントランス";
                }

                break;
            case GameManager.Location.Parlor:
                switch(language){
                    case Language.English:
                        return "Parlor";

                    case Language.Japanese:
                        return "居間";
                }

                break;
            case GameManager.Location.Study:
                switch(language){
                    case Language.English:
                        return "Study";

                    case Language.Japanese:
                        return "書斎";
                }

                break;
            case GameManager.Location.DiningRoom:
                switch(language){
                    case Language.English:
                        return "Dining Room";

                    case Language.Japanese:
                        return "食堂";
                }

                break;
            case GameManager.Location.Kitchen:
                switch(language){
                    case Language.English:
                        return "Kitchen";

                    case Language.Japanese:
                        return "キッチン";
                }

                break;
            case GameManager.Location.StockRoom:
                switch(language){
                    case Language.English:
                        return "Stock Room";

                    case Language.Japanese:
                        return "倉庫";
                }

                break;
            case GameManager.Location.Restroom:
                switch(language){
                    case Language.English:
                        return "Restroom";

                    case Language.Japanese:
                        return "トイレ";
                }

                break;
        }

        Debug.Log("Failed to localize.");
        return null;
    }

    public static string LocalizeRole(Player.Role role){
        switch(role){
            case Player.Role.Murderer:
                switch(language){
                    case Language.English:
                        return "Murderer";

                    case Language.Japanese:
                        return "殺人鬼";
                }

                break;
            case Player.Role.Betrayer:
                switch(language){
                    case Language.English:
                        return "Betrayer";

                    case Language.Japanese:
                        return "裏切者";
                }

                break;
            case Player.Role.Detective:
                switch(language){
                    case Language.English:
                        return "Detective";

                    case Language.Japanese:
                        return "探偵";
                }

                break;
            case Player.Role.Policeman:
                switch(language){
                    case Language.English:
                        return "Policeman";

                    case Language.Japanese:
                        return "警察官";
                }

                break;
            case Player.Role.Bystander:
                switch(language){
                    case Language.English:
                        return "Bystander";

                    case Language.Japanese:
                        return "一般人";
                }

                break;
        }

        Debug.Log("Failed to localize.");
        return null;
    }

    public static void SetLocalizationTarget(Language language){
        if(!localizations.ContainsKey(language)){
            Debug.Log("Failed to set localization target.");
            return;
        }

        Localization.language = language;
        localizationTarget = localizations[language];
    }

    private static void ReadCsv(string pass, Language language){
        List<TextSettings> valueList = new List<TextSettings>();
        StringReader stringReader = new StringReader(((TextAsset)Resources.Load(pass)).text);

        stringReader.ReadLine();

        while(stringReader.Peek() != -1){
            string[] values = stringReader.ReadLine().Split('\t');
            for(int index = 0; index < 6; index++){
                values[index] = TrimBothEnds(values[index]);
            }

            Font font = null;
            switch(values[2]){
                case "Afton James":
                    font = Resources.Load("Fonts/AFTON JAMES") as Font;
                    break;
                case "EB Garamond":
                    font = Resources.Load("Fonts/EBGARAMOND-REGULAR") as Font;
                    break;
                case "Top Secret":
                    font = Resources.Load("Fonts/TOP SECRET") as Font;
                    break;
                case "Soukou Mincho":
                    font = Resources.Load("Fonts/SoukouMincho") as Font;
                    break;
                case "Koku Mincho":
                    font = Resources.Load("Fonts/font_1_kokumr_1.00_rls") as Font;
                    break;
                case "Genkai Mincho":
                    font = Resources.Load("Fonts/genkai-mincho") as Font;
                    break;
            }

            FontStyle fontStyle = FontStyle.Normal;
            switch(values[3]){
                case "Normal":
                    fontStyle = FontStyle.Normal;
                    break;
                case "Bold":
                    fontStyle = FontStyle.Bold;
                    break;
                case "Italic":
                    fontStyle = FontStyle.Italic;
                    break;
                case "BoldAndItalic":
                    fontStyle = FontStyle.BoldAndItalic;
                    break;
            }

            TextSettings textSettings = new TextSettings(values[0], values[1], font, fontStyle, int.Parse(values[4]), float.Parse(values[5]));
            valueList.Add(textSettings);
        }

        localizations.Add(language, valueList);
    }

    private static string TrimBothEnds(string line){
        line = Regex.Replace(line, @"^.", "");
        line = Regex.Replace(line, @".$", "");

        return line;
    }

    public static void LocalizeText(string id, Text text){
        if(!localizationTarget.Exists(item => item.id == id)){
            Debug.Log("Text ID [" + id + "] not found.");
            return;
        }

        try{
            TextSettings textSettings = localizationTarget.Find(item => item.id == id);

            text.text = textSettings.text;
            text.font = textSettings.font;
            text.fontStyle = textSettings.fontStyle;
            text.fontSize = textSettings.fontSize;
            text.lineSpacing = textSettings.lineSpacing;

            text.text = text.text.Replace("|", "\n");
        }
        catch(NullReferenceException){
            Debug.Log("An exception occured when localizing the text: [" + id + "]");
        }
    }

    public static void LocalizePictureBook(GameObject pictureBook){
        int count = 1;
        string pass = pass = "Images/PictureBook/" + pictureBook.name + "/" + language + "/";

        foreach(Transform page in pictureBook.transform){
            page.GetComponent<Image>().sprite = Resources.Load<Sprite>(pass + "Page" + count);
            count++;
        }
    }

}
