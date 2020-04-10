//====================================================================================================
//
//  GameManager
//
//  ゲーム進行や設定の管理を行うマネージャースクリプト
//
//====================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour{

    /// <summary>
    /// ゲームのフェーズ(段階)
    /// </summary>
    public enum Phase{
        GameStart,
        RoundStart,
        ExcludedCheck,
        Discussion,
        RoundEnd,
        GameEnd
    }

    /// <summary>
    /// キャラクターの名前
    /// </summary>
    public enum CharacterName{
        Aligator,
        Bear,
        Cow,
        Deer,
        Eagle,
        Frog,
        Goat,
        Horse,
        Iguana,
        Jackal,
        Koala,
        Lion,
        Monkey,
        Newt,
        Owl,
        Pig,
        Quetzal,
        Rabbit,
        Squirrel,
        Tiger,
        Uguisu,
        Vulture,
        Wolf,
        Yak,
        Zebra
    }

    /// <summary>
    /// マップ内の各部屋
    /// </summary>
    public enum Location{
        MainEntrance,
        Parlor,
        Study,
        DiningRoom,
        Kitchen,
        StockRoom,
        Restroom
    }

    /******************************/

    /// <summary>
    /// クラスのインスタンス
    /// </summary>
    private static GameManager singleton;
    public static GameManager instance{
        get{
            return singleton;
        }
        private set{
            singleton = value;
        }
    }

    /// <summary>
    /// ゲーム(試合)中であるか
    /// </summary>
    public bool isInGame{
        get;
        set;
    }

    /// <summary>
    /// 音楽のボリューム
    /// </summary>
    public float musicVolume{
        get;
        set;
    }
    /// <summary>
    /// 効果音のボリューム
    /// </summary>
    public float soundEffectVolume{
        get;
        set;
    }
    
    /// <summary>
    /// プレイヤー数の上限
    /// </summary>
    public const int playerUpperLimit = 10;
    /// <summary>
    /// プレイヤー数の下限
    /// </summary>
    public const int playerLowerLimit = 3;
    /// <summary>
    /// プレイヤー数
    /// </summary>
    private int playerCount{
        get;
        set;
    }

    /// <summary>
    /// プレイヤーとNPCを合わせたキャラクターのリスト
    /// </summary>
    public List<Character> characters{
        get;
        private set;
    }
    /// <summary>
    /// プレイヤーのリスト
    /// </summary>
    public List<Player> players{
        get;
        private set;
    }
    /// <summary>
    /// NPCのリスト
    /// </summary>
    public List<NonPlayer> nonPlayers{
        get;
        private set;
    }
    
    /// <summary>
    /// 現在のラウンド
    /// </summary>
    public int round{
        get;
        private set;
    }
    /// <summary>
    /// 現在のターン
    /// </summary>
    public int turn{
        get;
        private set;
    }
    /// <summary>
    /// 現在のフェーズ
    /// </summary>
    public Phase phase{
        get;
        set;
    }

    /// <summary>
    /// 集めた手掛かりの数
    /// </summary>
    public int clueCount{
        get;
        private set;
    }
    /// <summary>
    /// 必要な手掛かりの数
    /// </summary>
    public int requiredClueCount{
        get;
        private set;
    }

    /// <summary>
    /// そのラウンド内で除外されたプレイヤーのリスト
    /// </summary>
    public List<Player> excludedPlayersInTheRound{
        get;
        set;
    }

    /******************************/

    private void Awake(){
        Setup();
    }

    private void Start(){
        Initialize();
	}

    /******************************/

    /// <summary>
    /// ゲーム開始前の準備処理
    /// </summary>
    private void Setup(){
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(gameObject);
        }

        Localization.Setup();

        LoadSettingData();
        isInGame = false;
    }

    /// <summary>
    /// ゲーム設定の読み込み
    /// </summary>
    private void LoadSettingData(){
        Localization.language = ((Localization.Language)PlayerPrefs.GetInt("Language", 0));

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 100);
        soundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume", 100);

        Debug.Log("Language: " + Localization.language);
        Debug.Log("Music: " + musicVolume + " / SE: " + soundEffectVolume);
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    public void Initialize(){
        playerCount = 0;

        players = new List<Player>();
        nonPlayers = new List<NonPlayer>();
        characters = new List<Character>();

        round = 0;
        turn = 0;
        phase = Phase.GameStart;

        clueCount = 0;

        excludedPlayersInTheRound = new List<Player>();
    }

    /// <summary>
    /// 各キャラクターの生成
    /// </summary>
    private void CreateCharacters(){
        playerCount = UIManager.instance.playerNames.Count;

        for(int count = 0; count < playerCount; count++){
            players.Add(new Player(UIManager.instance.playerNames[count]));
        }
        while(nonPlayers.Count < players.Count * 1.5f){
            nonPlayers.Add(new NonPlayer());
        }

        foreach(Player player in players){
            characters.Add(player);
        }
        foreach(NonPlayer nonPLayer in nonPlayers){
            characters.Add(nonPLayer);
        }

        foreach(Character character in characters){
            foreach(Player player in players){
                character.impressions.Add(player, Character.Impression.Unknown);
            }
        }

        AllocateNames();
        AllocateRoles();
        AllocateLocations();
    }

    /// <summary>
    /// ユニークな名前の割り当て
    /// </summary>
    private void AllocateNames(){
        List<CharacterName> names = new List<CharacterName>();
        foreach(CharacterName name in Enum.GetValues(typeof(CharacterName))){
            names.Add(name);
        }

        names.Shuffle();

        int index = 0;
        for(int count = 0; count < players.Count; count++){
            players[count].name = names[index];
            index++;
        }
        for(int count = 0; count < nonPlayers.Count; count++){
            nonPlayers[count].name = names[index];
            index++;
        }
    }

    /// <summary>
    /// 役職の割り当て
    /// </summary>
    private void AllocateRoles(){
        List<Player.Role> roles = new List<Player.Role>();

        switch(playerCount){
            case 3:
                roles.Add(Player.Role.Murderer);
                roles.Add(Player.Role.Policeman);
                roles.Add(Player.Role.Detective);

                break;
            case 4:
                roles.Add(Player.Role.Murderer);
                roles.Add(Player.Role.Policeman);
                roles.Add(Player.Role.Detective);
                roles.Add(Player.Role.Bystander);

                break;
            case 5:
                roles.Add(Player.Role.Murderer);
                roles.Add(Player.Role.Betrayer);
                roles.Add(Player.Role.Policeman);
                roles.Add(Player.Role.Detective);
                roles.Add(Player.Role.Bystander);

                break;
            case 6:
                roles.Add(Player.Role.Murderer);
                roles.Add(Player.Role.Betrayer);
                roles.Add(Player.Role.Policeman);
                roles.Add(Player.Role.Detective);
                roles.Add(Player.Role.Bystander);
                roles.Add(Player.Role.Bystander);

                break;
            case 7:
                roles.Add(Player.Role.Murderer);
                roles.Add(Player.Role.Murderer);
                roles.Add(Player.Role.Policeman);
                roles.Add(Player.Role.Detective);
                roles.Add(Player.Role.Bystander);
                roles.Add(Player.Role.Bystander);
                roles.Add(Player.Role.Bystander);

                break;
            case 8:
                roles.Add(Player.Role.Murderer);
                roles.Add(Player.Role.Murderer);
                roles.Add(Player.Role.Betrayer);
                roles.Add(Player.Role.Policeman);
                roles.Add(Player.Role.Detective);
                roles.Add(Player.Role.Bystander);
                roles.Add(Player.Role.Bystander);
                roles.Add(Player.Role.Bystander);

                break;
            case 9:
                roles.Add(Player.Role.Murderer);
                roles.Add(Player.Role.Murderer);
                roles.Add(Player.Role.Murderer);
                roles.Add(Player.Role.Policeman);
                roles.Add(Player.Role.Detective);
                roles.Add(Player.Role.Detective);
                roles.Add(Player.Role.Bystander);
                roles.Add(Player.Role.Bystander);
                roles.Add(Player.Role.Bystander);

                break;
            case 10:
                roles.Add(Player.Role.Murderer);
                roles.Add(Player.Role.Murderer);
                roles.Add(Player.Role.Murderer);
                roles.Add(Player.Role.Betrayer);
                roles.Add(Player.Role.Policeman);
                roles.Add(Player.Role.Detective);
                roles.Add(Player.Role.Detective);
                roles.Add(Player.Role.Bystander);
                roles.Add(Player.Role.Bystander);
                roles.Add(Player.Role.Bystander);
                roles.Add(Player.Role.Bystander);

                break;
        }

        roles.Shuffle();

        for(int index = 0; index < players.Count; index++){
            players[index].role = roles[index];

            if(roles[index] == Player.Role.Murderer || roles[index] == Player.Role.Betrayer){
                players[index].side = Player.Side.Murderer;
            }
            else{
                players[index].side = Player.Side.Innocents;
            }

            switch(roles[index]){
                case Player.Role.Murderer:
                    players[index].abilities.Add(new Knife());
                    players[index].abilities.Add(new HideDeadbody());
                    players[index].abilities.Add(new Feint());

                    break;
                case Player.Role.Betrayer:
                    players[index].abilities.Add(new HideDeadbody());
                    players[index].abilities.Add(new Feint());

                    break;
                case Player.Role.Policeman:
                    players[index].abilities.Add(new Pistol());
                    players[index].abilities.Add(new Search());

                    break;
                case Player.Role.Detective:
                    players[index].abilities.Add(new Autopsy());
                    players[index].abilities.Add(new Search());

                    break;
                case Player.Role.Bystander:
                    players[index].abilities.Add(new Search());

                    break;
            }
        }
    }

    /// <summary>
    /// スポーンする部屋の割り当て
    /// </summary>
    private void AllocateLocations(){
        foreach(Character character in characters){
            character.location = (Location)Enum.ToObject(typeof(Location), UnityEngine.Random.Range(0, Enum.GetValues(typeof(Location)).Length));
        }

        foreach(Player player in players){
            player.keyPeaceLocation = (Location)Enum.ToObject(typeof(Location), UnityEngine.Random.Range(1, Enum.GetValues(typeof(Location)).Length));
        }
    }

    /// <summary>
    /// ラウンド終了処理
    /// </summary>
    private void FinishRound(){
        round++;
        turn = 0;
        
        int innocentCount = 0;
        int murdererCount = 0;

        foreach(Character character in characters){
            if(character.state == Character.State.Attacked){
                character.state = Character.State.Dead;

                if(character.GetType() == typeof(Player)){
                    excludedPlayersInTheRound.Add((Player)character);
                }
            }
            if(character.GetType() == typeof(Player) && ((Player)character).foundKeyPeace){
                Player player = (Player)character;

                player.foundKeyPeace = false;
                player.keyPeaceLocation = (Location)Enum.ToObject(typeof(Location), UnityEngine.Random.Range(1, Enum.GetValues(typeof(Location)).Length));

                foreach(Location location in Enum.GetValues(typeof(Location))){
                    player.searchedLocations[location] = false;
                }

                clueCount++;

            }
            if(character.isBodyToBeHidden){
                character.isBodyHidden = true;
                character.isBodyToBeHidden = false;
            }

            if(character.GetType() == typeof(Player)){
                Player player = (Player)character;

                if(player.role == Player.Role.Murderer){
                    murdererCount++;

                    if(player.state == Character.State.Dead){
                        murdererCount--;
                    }
                }
                else if(player.side == Player.Side.Innocents){
                    innocentCount++;

                    if(player.state == Character.State.Dead){
                        innocentCount--;
                    }
                }

                foreach(Ability ability in player.abilities){
                    if(ability.cooldown != null){
                        ability.cooldown--;
                    }
                }
            }
            else if(character.GetType() == typeof(NonPlayer)){
                ((NonPlayer)character).Act();
            }

            if(character.state != Character.State.Dead){
                if(character.destination != null){
                    character.location = character.destination;
                    character.destination = null;
                }
                
                character.previousAction = character.action;
                character.action = Character.Action.Nothing;

                character.previousActionMessage = character.actionMessage;
                character.actionMessage = null;
            }
        }

        if(requiredClueCount <= clueCount || innocentCount == 0 || murdererCount == 0){
            foreach(Player player in players){
                if(player.side == Player.Side.Murderer){
                    if(0 < murdererCount && innocentCount == 0){
                        player.result = Player.Result.Win;
                    }
                }
                else if(player.side == Player.Side.Innocents){
                    if((0 < innocentCount) && (requiredClueCount <= clueCount || murdererCount == 0)){
                        player.result = Player.Result.Win;
                    }
                }
            }

            isInGame = false;
            phase = Phase.GameEnd;
        }

        UIManager.instance.ChangeScreen(UIManager.instance.messageScreen);
    }

    /// <summary>
    /// ゲーム設定終了時の処理
    /// </summary>
    public void OnSettingFinished(){
        CreateCharacters();

        requiredClueCount = (int)Math.Ceiling(players.Where(x => x.side == Player.Side.Innocents).Count() / 2.0f);

        foreach(Player player in players.Where(x => x.role == Player.Role.Policeman)){
            foreach(Ability ability in player.abilities){
                if(ability.GetType() == typeof(Pistol)){
                    ((Pistol)ability).residualUseCount = players.Where(x => x.role == Player.Role.Murderer).Count() + 1;
                }
            }
        }
    }

    /// <summary>
    /// 各プレイヤーの行動終了時の処理
    /// </summary>
    public void OnActionFinished(){
        turn++;

        if(turn < playerCount){
            UIManager.instance.ChangeScreen(UIManager.instance.confirmScreen);
        }
        else{
            FinishRound();
        }

        UIManager.instance.informationPanel.SetActive(false);
    }

    /// <summary>
    /// シャットダウン時の処理
    /// </summary>
    public void OnGameQuit(){
        Application.Quit();
    }

}
