using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour{

    public enum PictureBook{
        Story,
        Tutorial
    }

    /******************************/

    private static UIManager singleton;
    public static UIManager instance{
        get{
            return singleton;
        }
        private set{
            singleton = value;
        }
    }

    private GameObject activeScreen{
        get;
        set;
    }

    private int bookPageNumber{
        get;
        set;
    }

    public List<string> playerNames{
        get;
        set;
    }

    public float discussionTime{
        get;
        set;
    }
    public bool isTimerActivated{
        get;
        set;
    }
    public TimeSpan discussionTimeSpan{
        get;
        set;
    }

    /******************************/
    
    [SerializeField] public GameObject titleScreen;
    [SerializeField] public GameObject howToPlayScreen;
    [SerializeField] public GameObject pictureBookScreen;
    [SerializeField] public GameObject ruleScreen;
    [SerializeField] public GameObject settingScreen;
    [SerializeField] public GameObject roleListScreen;
    [SerializeField] public GameObject messageScreen;
    [SerializeField] public GameObject confirmScreen;
    [SerializeField] public GameObject informationPanel;
    [SerializeField] public GameObject actionScreen;
    [SerializeField] public GameObject abilityScreen;
    [SerializeField] public GameObject moveScreen;
    [SerializeField] public GameObject helpScreen;
    [SerializeField] public GameObject informationHelpPanel;
    [SerializeField] public GameObject actionHelpScreen;
    [SerializeField] public GameObject abilityHelpScreen;
    [SerializeField] public GameObject moveHelpScreen;
    [SerializeField] public GameObject excludedPlayerListScreen;
    [SerializeField] public GameObject discussionScreen;
    [SerializeField] public GameObject resultScreen;
    [SerializeField] public GameObject overlay;

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject versionText;
    [SerializeField] private GameObject pictureBook;
    [SerializeField] private GameObject ruleScreenView;
    [SerializeField] private GameObject roleIntroductionScreenView;
    [SerializeField] private GameObject playerList;
    [SerializeField] private GameObject warningText;
    [SerializeField] private GameObject addPlayerButton;
    [SerializeField] private GameObject settingFinishButton;
    [SerializeField] private GameObject roleList;
    [SerializeField] private GameObject playerNameText;
    [SerializeField] private GameObject confirmDescriptionText;
    [SerializeField] private GameObject signText;
    [SerializeField] private GameObject confirmButton;
    [SerializeField] private GameObject playerIdText;
    [SerializeField] private GameObject portraitImage;
    [SerializeField] private GameObject nameText;
    [SerializeField] private GameObject roleText;
    [SerializeField] private GameObject playerCountText;
    [SerializeField] private GameObject keyCountText;
    [SerializeField] private GameObject locationText;
    [SerializeField] private GameObject neighborList;
    [SerializeField] private GameObject neighborNotificationText;
    [SerializeField] private GameObject abilityList;
    [SerializeField] private GameObject[] locationButtons;
    [SerializeField] private GameObject[] arrowImages;
    [SerializeField] private GameObject excludedPlayerList;
    [SerializeField] private GameObject excludedNotificationText;
    [SerializeField] private GameObject timerText;
    [SerializeField] private GameObject skipDiscussionButton;
    [SerializeField] private GameObject resultPlayerList;
    
    [SerializeField] private GameObject messageWindowPrefab;
    [SerializeField] private GameObject audioSettingWindowPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject rolePrefab;
    [SerializeField] private GameObject confirmWindowPrefab;
    [SerializeField] private GameObject roleWindowPrefab;
    [SerializeField] private GameObject participantPrefab;
    [SerializeField] private GameObject rosterWindowPrefab;
    [SerializeField] private GameObject rosterParticipantPrefab;
    [SerializeField] private GameObject abilityButtonPrefab;
    [SerializeField] private GameObject targetSelectWindowPrefab;
    [SerializeField] private GameObject targetButtonPrefab;
    [SerializeField] private GameObject excludedPlayerPrefab;
    [SerializeField] private GameObject mapWindowPrefab;
    [SerializeField] private GameObject resultPlayerPrefab;

    [SerializeField] private Sprite murdererIconSprite;
    [SerializeField] private Sprite betrayerIconSprite;
    [SerializeField] private Sprite policemanIconSprite;
    [SerializeField] private Sprite detectiveIconSprite;
    [SerializeField] private Sprite bystanderIconSprite;
    [SerializeField] private Sprite questionMarkSprite;
    [SerializeField] private Sprite thumbsUpSprite;
    [SerializeField] private Sprite thumbsDownSprite;
    [SerializeField] private Sprite[] portraitSprites;

    [SerializeField] public Font aftonJamesFont;
    [SerializeField] public Font ebGaramondFont;
    [SerializeField] public Font kokuMinchoFont;
    [SerializeField] public Font soukouMinchoFont;
    
    [SerializeField] private AudioSource[] audioSources;

    [SerializeField] private AudioClip shortPageTurnAudio;
    [SerializeField] private AudioClip midiumPageTurnAudio;
    [SerializeField] private AudioClip longPageTurnAudio;
    [SerializeField] private AudioClip clockChimeAudio;
    [SerializeField] private AudioClip heartbeatAudio;
    [SerializeField] private AudioClip clappingHandsAudio;

    /******************************/

    private void Awake(){
        Setup();
    }

    private void Update(){
        TickTimer();
    }

    /******************************/

    private void Setup(){
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(gameObject);
        }

        Localize(Localization.language);

        audioSources[0].volume = GameManager.instance.musicVolume / 100.0f;
        audioSources[1].volume = GameManager.instance.soundEffectVolume / 100.0f;

        ChangeScreen(titleScreen);
        versionText.GetComponent<Text>().text = "Version: " + Application.version;

        playerNames = new List<string>();

        isTimerActivated = false;
    }

    public void ChangeScreen(GameObject screen){
        activeScreen?.SetActive(false);
        screen?.SetActive(true);

        activeScreen = screen;

        UpdateScreen(screen);
    }

    private void UpdateScreen(GameObject screen){
        if(screen == ruleScreen){
            UpdateRuleScreen();
        }
        else if(screen == settingScreen){
            UpdateSettingScreen();
        }
        else if(screen == roleListScreen){
            UpdateRoleListScreen();
        }
        else if(screen == messageScreen){
            UpdateMessageScreen();
        }
        else if(screen == confirmScreen){
            UpdateConfirmScreen(GameManager.instance.players[GameManager.instance.turn]);
        }
        else if(screen == actionScreen){
            UpdateActionScreen(GameManager.instance.players[GameManager.instance.turn]);
        }
        else if(screen == abilityScreen){
            UpdateAbilityScreen(GameManager.instance.players[GameManager.instance.turn]);
        }
        else if(screen == moveScreen){
            UpdateMoveScreen(GameManager.instance.players[GameManager.instance.turn]);
        }
        else if(screen == excludedPlayerListScreen){
            UpdateExcludedPlayerListScreen();
        }
        else if(screen == discussionScreen){
            UpdateDiscussionScreen();
        }
        else if(screen == resultScreen){
            UpdateResultScreen();
        }
    }

    public void UpdatePictureBookScreen(int targetBook){
        foreach(Transform title in pictureBookScreen.transform.GetChild(0)){
            title.gameObject.SetActive(false);
        }
        pictureBookScreen.transform.GetChild(0).GetChild(targetBook).gameObject.SetActive(true);

        Transform books = pictureBook.transform.GetChild(0);
        Transform pages = books.GetChild((int)targetBook);
        Text bookPageNumberText = pictureBook.transform.GetChild(1).GetComponent<Text>();
        Button nextPageButton = pictureBook.transform.GetChild(2).GetComponent<Button>();
        Button previousPageButton = pictureBook.transform.GetChild(3).GetComponent<Button>();
        int upperPageLimit = pages.childCount;

        foreach(Transform book in books){
            foreach(Transform page in book){
                page.gameObject.SetActive(false);
            }
        }

        bookPageNumber = 1;
        bookPageNumberText.GetComponent<Text>().text = bookPageNumber + "/" + upperPageLimit;
        pages.GetChild(bookPageNumber - 1).gameObject.SetActive(true);

        nextPageButton.onClick.RemoveAllListeners();
        nextPageButton.onClick.AddListener(() => {
            pages.GetChild(bookPageNumber - 1).gameObject.SetActive(false);

            if(upperPageLimit <= ++bookPageNumber){
                bookPageNumber = upperPageLimit;
                nextPageButton.interactable = false;
            }

            bookPageNumberText.GetComponent<Text>().text = bookPageNumber + "/" + upperPageLimit;
            previousPageButton.interactable = true;

            pages.GetChild(bookPageNumber - 1).gameObject.SetActive(true);
        });

        previousPageButton.onClick.RemoveAllListeners();
        previousPageButton.onClick.AddListener(() => {
            pages.GetChild(bookPageNumber - 1).gameObject.SetActive(false);

            if(--bookPageNumber <= 1){
                bookPageNumber = 1;
                previousPageButton.interactable = false;
            }

            bookPageNumberText.GetComponent<Text>().text = bookPageNumber + "/" + upperPageLimit;
            nextPageButton.interactable = true;

            pages.GetChild(bookPageNumber - 1).gameObject.SetActive(true);
        });

        nextPageButton.interactable = true;
        previousPageButton.interactable = false;
    }

    private void UpdateRuleScreen(){
        Button backButton = ruleScreenView.transform.GetChild(4).GetComponent<Button>();
        backButton.onClick.RemoveAllListeners();

        if(GameManager.instance.isInGame){
            informationPanel.SetActive(false);
            actionScreen.SetActive(false);
            
            backButton.onClick.AddListener(() => {
                informationPanel.SetActive(true);
                actionScreen.SetActive(true);

                ChangeScreen(actionScreen);
            });
        }
        else{
            backButton.onClick.AddListener(() => {
                ChangeScreen(howToPlayScreen);
            });
        }
    }

    public void UpdateRoleIntroductionScreen(int targetRole){
        ResetScroll(roleIntroductionScreenView);

        Text title = roleIntroductionScreenView.transform.GetChild(0).GetComponent<Text>();
        Text description = roleIntroductionScreenView.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        Text introduction = roleIntroductionScreenView.transform.GetChild(1).GetChild(1).GetComponent<Text>();
        Transform abilityButtonList = roleIntroductionScreenView.transform.GetChild(2).GetChild(1);

        foreach(Transform button in abilityButtonList){
            Destroy(button.gameObject);
        }

        title.text = "- " + Localization.LocalizeRole((Player.Role)targetRole).ToUpper() + " -";
        Localization.LocalizeText("RoleIntroduction_" + ((Player.Role)targetRole).ToString() + "Introduction", introduction);

        List<Ability> abilities = new List<Ability>();

        switch(targetRole){
            case (int)Player.Role.Murderer:
                abilities.Add(new Knife());
                abilities.Add(new HideDeadbody());
                abilities.Add(new Feint());

                break;
            case (int)Player.Role.Betrayer:
                abilities.Add(new HideDeadbody());
                abilities.Add(new Feint());

                break;
            case (int)Player.Role.Policeman:
                abilities.Add(new Pistol());
                abilities.Add(new Search());

                break;
            case (int)Player.Role.Detective:
                abilities.Add(new Autopsy());
                abilities.Add(new Search());

                break;
            case (int)Player.Role.Bystander:
                abilities.Add(new Search());

                break;
        }

        foreach(Ability ability in abilities){
            GameObject abilityButton = Instantiate(abilityButtonPrefab, abilityButtonList);
            Button useButton = abilityButton.transform.GetChild(0).GetComponent<Button>();

            Text useButtonText = abilityButton.GetComponentInChildren<Text>();
            useButtonText.text = ability.name;

            abilityButton.transform.GetChild(1).gameObject.SetActive(false);
             abilityButton.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);

            Text abilityButtonText = abilityButton.GetComponentInChildren<Text>();
            Localization.LocalizeText("RoleIntroduction_AbilityButton", abilityButtonText);
            abilityButtonText.text = ability.name;

            useButton.GetComponent<Button>().onClick.AddListener(() => {
                ShowMessageWindow("Rule_" + ability.GetType().Name);
            });
        }
    }

    private void UpdateSettingScreen(){
        foreach(Transform content in playerList.transform){
            Destroy(content.gameObject);
        }

        GameManager.instance.Initialize();

        if(playerNames.Count < GameManager.playerLowerLimit){
            warningText.SetActive(true);

            switch (Localization.language) {
                case Localization.Language.English:
                    warningText.GetComponent<Text>().text = (GameManager.playerLowerLimit - playerNames.Count) + " more players to play";

                    break;
                case Localization.Language.Japanese:
                    warningText.GetComponent<Text>().text = "プレイにはあと" + (GameManager.playerLowerLimit - playerNames.Count) + "人のプレイヤーが必要です";

                    break;
            }

            settingFinishButton.GetComponent<Button>().interactable = false;
        }
        else{
            warningText.SetActive(false);
            settingFinishButton.GetComponent<Button>().interactable = true;
        }

        if(playerNames.Count == GameManager.playerUpperLimit - 1){
            addPlayerButton.GetComponent<Button>().interactable = false;
        }
        else{
            addPlayerButton.GetComponent<Button>().interactable = true;
        }
    }

    private void UpdateRoleListScreen(){
        foreach(Transform content in roleList.transform){
            Destroy(content.gameObject);
        }

        foreach(Player.Role role in Enum.GetValues(typeof(Player.Role))){
            if(0 < GameManager.instance.players.Count(player => player.role == role)){
                GameObject roleText = Instantiate(rolePrefab, roleList.transform);
                roleText.GetComponent<Text>().text = Localization.LocalizeRole(role) + " - " + GameManager.instance.players.Count(player => player.role == role);

                switch(Localization.language){
                    case Localization.Language.English:
                        roleText.GetComponent<Text>().font = ebGaramondFont;
                        roleText.GetComponent<Text>().fontStyle = FontStyle.Normal;

                        break;
                    case Localization.Language.Japanese:
                        roleText.GetComponent<Text>().font = kokuMinchoFont;
                        roleText.GetComponent<Text>().fontStyle = FontStyle.Bold;

                        break;
                }

                if(role == Player.Role.Murderer || role == Player.Role.Betrayer){
                    roleText.GetComponent<Text>().color = Color.red;
                }
                else{
                    roleText.GetComponent<Text>().color = Color.blue;
                }
            }
        }

        GameObject npc = Instantiate(rolePrefab, roleList.transform);
        npc.GetComponent<Text>().text = "NPC - " + GameManager.instance.nonPlayers.Count;
        npc.GetComponent<Text>().color = Color.gray;

        switch(Localization.language){
            case Localization.Language.English:
                npc.GetComponent<Text>().font = ebGaramondFont;
                npc.GetComponent<Text>().fontStyle = FontStyle.Normal;

                break;
            case Localization.Language.Japanese:
                npc.GetComponent<Text>().font = kokuMinchoFont;
                npc.GetComponent<Text>().fontStyle = FontStyle.Bold;

                break;
        }
    }

    private void UpdateMessageScreen(){
        Text title = messageScreen.transform.GetChild(0).GetComponent<Text>();
        Text description = messageScreen.transform.GetChild(0).GetComponentInChildren<Text>();
        Text message = messageScreen.transform.GetChild(1).GetComponent<Text>();
        Button button = messageScreen.transform.GetChild(2).GetComponent<Button>();

        Localization.LocalizeText("Message_" + GameManager.instance.phase + "_Title", title);
        Localization.LocalizeText("Message_" + GameManager.instance.phase + "_Text", message);

        switch(GameManager.instance.phase){
            case GameManager.Phase.GameStart:
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    GameManager.instance.phase = GameManager.Phase.ExcludedCheck;
                    ChangeScreen(confirmScreen);
                });

                break;
            case GameManager.Phase.RoundStart:
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    GameManager.instance.phase = GameManager.Phase.ExcludedCheck;
                    ChangeScreen(confirmScreen);
                });

                break;
            case GameManager.Phase.ExcludedCheck:
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    GameManager.instance.phase = GameManager.Phase.Discussion;
                    ChangeScreen(excludedPlayerListScreen);
                });

                break;
            case GameManager.Phase.Discussion:
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    GameManager.instance.phase = GameManager.Phase.RoundEnd;
                    ChangeScreen(discussionScreen);
                });

                break;
            case GameManager.Phase.RoundEnd:
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    GameManager.instance.phase = GameManager.Phase.RoundStart;
                    ChangeScreen(messageScreen);
                });

                break;
            case GameManager.Phase.GameEnd:
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    ChangeScreen(resultScreen);
                });

                audioSources[0].Stop();
                PlayAudio(heartbeatAudio);

                break;
        }
    }

    private void UpdateConfirmScreen(Player player){
        string skipButtonTextString = null;
        string confirmButtonTextString = null;
        string confirmMessage = null;

        switch(Localization.language){
            case Localization.Language.English:
                playerNameText.GetComponent<Text>().text = "- " + player.id.ToUpper() + "'s ACTION -";
                confirmDescriptionText.GetComponent<Text>().text = "CONTINUE IF YOU ARE " + player.id.ToUpper() + ".";

                skipButtonTextString = "SKIP";
                confirmButtonTextString = "CONTINUE";

                confirmMessage = "Next is\n<b>" + player.id + "</b>'s turn.\nAre you sure?";

                break;
            case Localization.Language.Japanese:
                playerNameText.GetComponent<Text>().text = "- " + player.id + "の手番 -";
                confirmDescriptionText.GetComponent<Text>().text = "あなたが" + player.id + "ならば手番を開始して下さい";

                skipButtonTextString = "スキップ";
                confirmButtonTextString = "手番を始める";

                confirmMessage = "<b>" + player.id + "</b>の\nターンを開始します。\nよろしいですか？";

                break;
        }

        confirmButton.GetComponent<Button>().onClick.RemoveAllListeners();

        if(player.state == Character.State.Dead){
            confirmButton.GetComponentInChildren<Text>().text = skipButtonTextString;
            confirmButton.GetComponent<Button>().onClick.AddListener(GameManager.instance.OnActionFinished);

            signText.gameObject.SetActive(true);
        }
        else{
            confirmButton.GetComponentInChildren<Text>().text = confirmButtonTextString;
            confirmButton.GetComponent<Button>().onClick.AddListener(() => {
                ShowConfirmWindow(confirmMessage, () => {
                    ChangeScreen(actionScreen);
                });
            });

            signText.gameObject.SetActive(false);
        }
    }

    private void UpdateActionScreen(Player player){
        if(player == null || !GameManager.instance.players.Contains(player)){
            return;
        }

        informationPanel.SetActive(true);
        UpdateInformationPanel(player);

        locationText.GetComponent<Text>().text = Localization.LocalizeLocation(player.location);

        foreach(Transform content in neighborList.transform){
            Destroy(content.gameObject);
        }

        Vector2 neighborListSize = neighborList.GetComponent<RectTransform>().sizeDelta;

        GameManager.instance.characters.Sort((x, y) => string.Compare(Localization.LocalizeCharacterName(x.name), Localization.LocalizeCharacterName(y.name)));

        int interval = 10;
        int count = 0;

        foreach(Character character in GameManager.instance.characters){
            if(player.location == character.location && player != character && !character.isBodyHidden){
                GameObject participant = Instantiate(participantPrefab, neighborList.transform);
                Text nameText = participant.transform.GetChild(1).GetComponent<Text>();
                Text murdererSignText = participant.transform.GetChild(1).GetChild(0).GetComponent<Text>();
                Text actionText = participant.transform.GetChild(3).GetComponent<Text>();
                Button impressionButton = participant.transform.GetChild(4).GetComponent<Button>();

                nameText.text = Localization.LocalizeCharacterName(character.name).ToUpper();
                SetPortrait(participant.transform.GetChild(0).GetComponent<Image>(), character.name);

                switch(Localization.language){
                    case Localization.Language.English:
                        nameText.font = ebGaramondFont;

                        actionText.font = ebGaramondFont;
                        actionText.fontStyle = FontStyle.Bold;
                        actionText.fontSize = 40;

                        break;
                    case Localization.Language.Japanese:
                        nameText.font = kokuMinchoFont;

                        actionText.font = kokuMinchoFont;
                        actionText.fontStyle = FontStyle.Normal;
                        actionText.fontSize = 60;

                        break;
                }

                if(character.state == Character.State.Dead){
                    switch(Localization.language){
                        case Localization.Language.English:
                            actionText.text = "DEAD";

                            break;
                        case Localization.Language.Japanese:
                            actionText.text = "死亡";

                            break;
                    }
                }
                else if(player.previousAction == Character.Action.Move){
                    actionText.text = "  ?";
                }
                else if(character.previousAction == Character.Action.Nothing){
                    switch(Localization.language){
                        case Localization.Language.English:
                            actionText.text = "NOTHING";

                            break;
                        case Localization.Language.Japanese:
                            actionText.text = "目立った行動なし";

                            break;
                    }
                }
                else if(character.previousAction == Character.Action.Move){
                    switch(Localization.language){
                        case Localization.Language.English:
                            actionText.text = "MOVED";

                            break;
                        case Localization.Language.Japanese:
                            actionText.text = "移動";

                            break;
                    }
                }
                else{
                    actionText.text = character.previousActionMessage;
                }

                if(player.role == Player.Role.Murderer && character.GetType() == typeof(Player) && ((Player)character).role == Player.Role.Murderer){
                    murdererSignText.gameObject.SetActive(true);

                    switch(Localization.language){
                        case Localization.Language.English:
                            murdererSignText.text = "(Murderer)";
                            murdererSignText.font = ebGaramondFont;

                            break;
                        case Localization.Language.Japanese:
                            murdererSignText.text = "(殺人鬼)";
                            murdererSignText.font = kokuMinchoFont;

                            break;
                    }
                }
                else {
                    murdererSignText.gameObject.SetActive(false);
                }

                if(!player.metCharacters.Contains(character) && !character.isBodyHidden){
                    player.metCharacters.Add(character);
                }

                switch(character.impressions[player]){
                    case Character.Impression.Unknown:
                        impressionButton.image.sprite = questionMarkSprite;

                        break;
                    case Character.Impression.Friendly:
                        impressionButton.image.sprite = thumbsUpSprite;

                        break;
                    case Character.Impression.Unfriendly:
                        impressionButton.image.sprite = thumbsDownSprite;

                        break;
                }

                impressionButton.onClick.AddListener(() => {
                    switch(character.impressions[player]){
                        case Character.Impression.Unknown:
                            character.impressions[player] = Character.Impression.Friendly;
                            impressionButton.image.sprite = thumbsUpSprite;

                            break;
                        case Character.Impression.Friendly:
                            character.impressions[player] = Character.Impression.Unfriendly;
                            impressionButton.image.sprite = thumbsDownSprite;

                            break;
                        case Character.Impression.Unfriendly:
                            character.impressions[player] = Character.Impression.Unknown;
                            impressionButton.image.sprite = questionMarkSprite;

                            break;
                    }
                });

                count++;
            }
        }

        if(count == 0){
            neighborNotificationText.SetActive(true);
        }
        else{
            neighborNotificationText.SetActive(false);

            neighborList.GetComponent<RectTransform>().sizeDelta = new Vector2(neighborListSize.x, participantPrefab.GetComponent<RectTransform>().rect.height * count + interval * count);
        }

        if(GameManager.instance.round == 0 && !player.roleConfirmed){
            ShowRoleWindow(player.role);
            player.roleConfirmed = true;
        }
    }

    private void UpdateInformationPanel(Player player){
        playerIdText.GetComponent<Text>().text = player.id.ToString();
        playerCountText.GetComponent<Text>().text = GameManager.instance.players.Where(x => x.state != Character.State.Dead).Count().ToString();
        keyCountText.GetComponent<Text>().text = GameManager.instance.clueCount + "/" + GameManager.instance.requiredClueCount;

        SetPortrait(portraitImage.GetComponent<Image>(), player.name);

        switch(Localization.language){
            case Localization.Language.English:
                nameText.GetComponent<Text>().text = "NAME: " + Localization.LocalizeCharacterName(player.name);
                roleText.GetComponent<Text>().text = "ROLE: " + Localization.LocalizeRole(player.role);

                break;
            case Localization.Language.Japanese:
                nameText.GetComponent<Text>().text = "名前: " + Localization.LocalizeCharacterName(player.name);
                roleText.GetComponent<Text>().text = "役職: " + Localization.LocalizeRole(player.role);

                break;
        }
    }

    private void UpdateAbilityScreen(Player player){
        foreach(Transform abilityButton in abilityList.transform){
            Destroy(abilityButton.gameObject);
        }

        foreach(Ability ability in player.abilities){
            GameObject abilityButton = Instantiate(abilityButtonPrefab, abilityList.transform);

            Button useButton = abilityButton.transform.GetChild(0).GetComponent<Button>();
            Button helpButton = abilityButton.transform.GetChild(1).GetComponent<Button>();
            Text residualUseCountText = abilityButton.transform.GetChild(0).GetChild(1).GetComponent<Text>();

            Text useButtonText = abilityButton.GetComponentInChildren<Text>();
            Localization.LocalizeText("RoleIntroduction_AbilityButton", useButtonText);
            useButtonText.text = ability.name;

            if(ability.residualUseCount != null){
                residualUseCountText.gameObject.SetActive(true);
                residualUseCountText.text = "× " + ability.residualUseCount;
            }
            else{
                residualUseCountText.gameObject.SetActive(false);
            }

            useButton.onClick.AddListener(() => {
                GameObject abilityWindow = Instantiate(targetSelectWindowPrefab, overlay.transform);
                overlay.SetActive(true);

                Text titleText = abilityWindow.transform.GetChild(0).GetComponent<Text>(); ;
                Text messageText = abilityWindow.transform.GetChild(1).GetComponent<Text>();
                GameObject scrollView = abilityWindow.transform.GetChild(2).gameObject;
                GameObject scrollViewContent = abilityWindow.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
                GameObject frame = abilityWindow.transform.GetChild(2).GetChild(2).gameObject;
                GameObject button = abilityWindow.transform.GetChild(3).gameObject;
                Text buttonText = abilityWindow.transform.GetChild(3).GetComponentInChildren<Text>();

                string confirmMessage = null;

                if(ability.FulfillsCondition(player)){
                    if(ability.isTargetable){
                        Vector2 scrollViewContentSize = scrollViewContent.GetComponent<RectTransform>().sizeDelta;
                        scrollViewContentSize = new Vector2(scrollViewContentSize.x, 0);

                        messageText.gameObject.SetActive(false);

                        Localization.LocalizeText("AbilityWindow_TargetSelect_Title", titleText);
                        Localization.LocalizeText("Window_CancelButton", buttonText);

                        button.GetComponent<Button>().onClick.AddListener(() => {
                            Destroy(abilityWindow);
                            overlay.SetActive(false);
                        });

                        GameManager.instance.characters.Sort((x, y) => string.Compare(Localization.LocalizeCharacterName(x.name), Localization.LocalizeCharacterName(y.name)));

                        int count = 0;
                        foreach(Character target in GameManager.instance.characters){
                            if(target.location == player.location && target != player && ability.FulfillsCondition(target)){
                                count++;

                                scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollViewContentSize.x, count * targetButtonPrefab.GetComponent<RectTransform>().rect.height);
                                GameObject targetButton = Instantiate(targetButtonPrefab, scrollViewContent.transform);
                                Text targetButtonText = targetButton.GetComponentInChildren<Text>();

                                targetButtonText.text = Localization.LocalizeCharacterName(target.name);

                                switch(Localization.language){
                                    case Localization.Language.English:
                                        targetButtonText.font = aftonJamesFont;
                                        targetButtonText.fontStyle = FontStyle.Normal;
                                        targetButtonText.fontSize = 65;

                                        break;
                                    case Localization.Language.Japanese:
                                        targetButtonText.font = soukouMinchoFont;
                                        targetButtonText.fontStyle = FontStyle.Bold;
                                        targetButtonText.fontSize = 60;

                                        break;
                                }

                                targetButton.GetComponent<Button>().onClick.AddListener(() => {
                                    abilityWindow.SetActive(false);

                                    switch(Localization.language){
                                        case Localization.Language.English:
                                            confirmMessage = "You are going\nto use <b>\"" + ability.name + "\"</b> to <b>" + Localization.LocalizeCharacterName(target.name) + "</b>.\nAre you sure?";

                                            break;
                                        case Localization.Language.Japanese:
                                            confirmMessage = "<b>" + Localization.LocalizeCharacterName(target.name) + "</b>に対して\n<b>「" + ability.name + "」</b>を使用します。\nよろしいですか？";

                                            break;
                                    }

                                    ShowConfirmWindow(confirmMessage, () => {
                                        ability.Use(player, target);

                                        abilityWindow.SetActive(true);
                                        overlay.SetActive(true);

                                        messageText.gameObject.SetActive(true);
                                        scrollView.SetActive(false);

                                        Localization.LocalizeText("AbilityWindow_Result_Title", titleText);
                                        Localization.LocalizeText("Window_Text", messageText);
                                        Localization.LocalizeText("Window_OKButton", buttonText);

                                        messageText.text = ability.resultMessage;

                                        button.GetComponent<Button>().onClick.AddListener(() => {
                                            GameManager.instance.OnActionFinished();
                                            Destroy(abilityWindow);
                                            overlay.SetActive(false);
                                        });
                                    });
                                });
                            }
                        }
                    }
                    else{
                        abilityWindow.SetActive(false);

                        switch(Localization.language){
                            case Localization.Language.English:
                                confirmMessage = "You are going\nto use <b>\"" + ability.name + "\"</b>.\nAre you sure?";

                                break;
                            case Localization.Language.Japanese:
                                confirmMessage = "<b>「" + ability.name + "」</b>を使用します。\nよろしいですか？";

                                break;
                        }

                        ShowConfirmWindow(confirmMessage, () => {
                            ability.Use(player);

                            abilityWindow.SetActive(true);
                            overlay.SetActive(true);

                            messageText.gameObject.SetActive(true);
                            scrollView.SetActive(false);

                            Localization.LocalizeText("AbilityWindow_Result_Title", titleText);
                            Localization.LocalizeText("Window_Text", messageText);
                            Localization.LocalizeText("Window_OKButton", buttonText);

                            messageText.text = ability.resultMessage;

                            button.GetComponent<Button>().onClick.AddListener(() => {
                                GameManager.instance.OnActionFinished();
                                Destroy(abilityWindow);
                                overlay.SetActive(false);
                            });
                        });
                    }
                }
                else{
                    messageText.gameObject.SetActive(true);
                    scrollView.SetActive(false);

                    Localization.LocalizeText("AbilityWindow_Notice_Title", titleText);
                    Localization.LocalizeText("Window_Text", messageText);
                    Localization.LocalizeText("Window_OKButton", buttonText);

                    messageText.text = ability.resultMessage;

                    button.GetComponent<Button>().onClick.AddListener(() => {
                        Destroy(abilityWindow);
                        overlay.SetActive(false);
                    });
                }
            });

            helpButton.onClick.AddListener(() => {
                ShowMessageWindow("Rule_" + ability.GetType().ToString());
            });
        }
    }

    private void UpdateMoveScreen(Player player){
        List<GameManager.Location> adjacentLocations = new List<GameManager.Location>();

        if(player.location == GameManager.Location.MainEntrance){
            foreach(GameManager.Location location in Enum.GetValues(typeof(GameManager.Location))){
                if(location != GameManager.Location.MainEntrance){
                    adjacentLocations.Add(location);
                }
            }
        }
        else{
            adjacentLocations.Add(GameManager.Location.MainEntrance);

            int adjacentLocation = (int)player.location + 1;
            if(Enum.GetValues(typeof(GameManager.Location)).Length <= adjacentLocation){
                adjacentLocation = 1;
            }
            adjacentLocations.Add((GameManager.Location)adjacentLocation);

            adjacentLocation = (int)player.location - 1;
            if(adjacentLocation < 1){
                adjacentLocation = Enum.GetValues(typeof(GameManager.Location)).Length - 1;
            }
            adjacentLocations.Add((GameManager.Location)adjacentLocation);
        }

        foreach(GameManager.Location location in Enum.GetValues(typeof(GameManager.Location))){
            GameObject button = locationButtons[(int)location];

            button.GetComponent<Button>().interactable = false;
            button.GetComponent<Button>().onClick.RemoveAllListeners();

            button.GetComponent<Image>().color = Color.black;
            button.transform.GetChild(0).GetComponent<Text>().color = Color.gray;

            if(location == player.location){
                button.transform.GetChild(0).GetComponent<Text>().color = Color.red;

                button.transform.GetChild(1).GetComponentInChildren<Text>().text = GameManager.instance.characters.Where(x => x.location == location && x.state != Character.State.Dead).Count().ToString();
            }
            else if(adjacentLocations.Contains(location)){
                button.GetComponent<Button>().interactable = true;
                button.GetComponent<Button>().onClick.AddListener(() => {
                    string confirmMessage = null;
                    switch(Localization.language){
                        case Localization.Language.English:
                            confirmMessage = "You are going\nto go to <b>" + Localization.LocalizeLocation(location) + "</b>.\nAre you sure?";

                            break;
                        case Localization.Language.Japanese:
                            confirmMessage = "<b>" + Localization.LocalizeLocation(location) + "</b>へ移動します。\nよろしいですか？";;

                            break;
                    }

                    ShowConfirmWindow(confirmMessage, () => {
                        GameManager.instance.players[GameManager.instance.turn].destination = location;
                        player.action = Character.Action.Move;

                        GameManager.instance.OnActionFinished();
                    });
                });
                
                button.transform.GetChild(0).GetComponent<Text>().color = Color.black;
                button.transform.GetChild(1).GetComponentInChildren<Text>().text = GameManager.instance.characters.Where(x => x.location == location && x.state != Character.State.Dead).Count().ToString();
            }
            else{
                button.transform.GetChild(1).GetComponentInChildren<Text>().text = "?";
            }

            if(location != GameManager.Location.MainEntrance){
                if(player.searchedLocations[location]){
                    button.transform.GetChild(2).gameObject.SetActive(true);
                }
                else{
                    button.transform.GetChild(2).gameObject.SetActive(false);
                }
            }
        }

        foreach(GameObject image in arrowImages){
            image.SetActive(false);
        }

        switch(player.location){
            case GameManager.Location.MainEntrance:
                arrowImages[0].SetActive(true);
                arrowImages[1].SetActive(true);
                arrowImages[2].SetActive(true);
                arrowImages[3].SetActive(true);
                arrowImages[4].SetActive(true);
                arrowImages[5].SetActive(true);

                break;
            case GameManager.Location.Parlor:
                arrowImages[0].SetActive(true);
                arrowImages[6].SetActive(true);
                arrowImages[11].SetActive(true);

                break;
            case GameManager.Location.Study:
                arrowImages[1].SetActive(true);
                arrowImages[6].SetActive(true);
                arrowImages[7].SetActive(true);

                break;
            case GameManager.Location.DiningRoom:
                arrowImages[2].SetActive(true);
                arrowImages[7].SetActive(true);
                arrowImages[8].SetActive(true);

                break;
            case GameManager.Location.Kitchen:
                arrowImages[3].SetActive(true);
                arrowImages[8].SetActive(true);
                arrowImages[9].SetActive(true);

                break;
            case GameManager.Location.StockRoom:
                arrowImages[4].SetActive(true);
                arrowImages[9].SetActive(true);
                arrowImages[10].SetActive(true);

                break;
            case GameManager.Location.Restroom:
                arrowImages[5].SetActive(true);
                arrowImages[10].SetActive(true);
                arrowImages[11].SetActive(true);

                break;
        }
    }

    private void UpdateExcludedPlayerListScreen(){
        foreach(Transform content in excludedPlayerList.transform){
            Destroy(content.gameObject);
        }

        Vector2 playerListSize = excludedPlayerList.GetComponent<RectTransform>().sizeDelta;
        playerListSize = new Vector2(playerListSize.x, 0);

        int count = 0;
        foreach(Player player in GameManager.instance.players){
            if(player.state == Character.State.Dead){
                count++;

                excludedPlayerList.GetComponent<RectTransform>().sizeDelta = new Vector2(playerListSize.x, count * excludedPlayerPrefab.GetComponent<RectTransform>().rect.height);

                GameObject playerText = Instantiate(excludedPlayerPrefab, excludedPlayerList.transform);
                playerText.GetComponent<Text>().text = player.id;

                switch(Localization.language){
                    case Localization.Language.English:
                        playerText.GetComponent<Text>().font = ebGaramondFont;
                        playerText.GetComponent<Text>().fontSize = 70;

                        break;
                    case Localization.Language.Japanese:
                        playerText.GetComponent<Text>().font = kokuMinchoFont;
                        playerText.GetComponent<Text>().fontSize = 65;

                        break;
                }

                if(GameManager.instance.excludedPlayersInTheRound.Contains(player)){
                    playerText.transform.GetChild(0).gameObject.SetActive(true);
                }
                else{
                    playerText.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }

        if(0 < count){
            excludedNotificationText.SetActive(false);
        }
        else{
            excludedNotificationText.SetActive(true);
        }

        GameManager.instance.excludedPlayersInTheRound.Clear();
    }

    private void UpdateDiscussionScreen(){
        activeScreen.SetActive(false);
        discussionScreen.SetActive(true);

        discussionTime = 3 * 60;
        isTimerActivated = true;
        audioSources[1].Play();

        switch(Localization.language){
            case Localization.Language.English:
                skipDiscussionButton.transform.GetChild(0).GetComponent<Text>().text = "SKIP";

                break;
            case Localization.Language.Japanese:
                skipDiscussionButton.transform.GetChild(0).GetComponent<Text>().text = "終了";

                break;
        }
    }

    private void UpdateResultScreen(){
        foreach(Transform content in resultPlayerList.transform){
            Destroy(content.gameObject);
        }

        Vector2 playerListSize = resultPlayerList.GetComponent<RectTransform>().sizeDelta;
        playerListSize = new Vector2(playerListSize.x, 0);

        int count = 0;
        foreach(Player player in GameManager.instance.players){
            count++;

            resultPlayerList.GetComponent<RectTransform>().sizeDelta = new Vector2(playerListSize.x, count * resultPlayerPrefab.GetComponent<RectTransform>().rect.height);
            GameObject resultPlayer = Instantiate(resultPlayerPrefab, resultPlayerList.transform);
            Text resultText = resultPlayer.transform.GetChild(0).GetComponent<Text>();
            Text nameText = resultPlayer.transform.GetChild(2).GetComponent<Text>();
            Text roleText = resultPlayer.transform.GetChild(3).GetComponent<Text>();

            resultText.text = player.id + " - " + player.result.ToString().ToUpper();

            switch(Localization.language){
                case Localization.Language.English:
                    resultText.font = ebGaramondFont;

                    nameText.text = "NAME: " + Localization.LocalizeCharacterName(player.name).ToUpper();
                    nameText.font = ebGaramondFont;

                    roleText.text = "ROLE: " + Localization.LocalizeRole(player.role).ToUpper();
                    roleText.font = ebGaramondFont;

                    break;
                case Localization.Language.Japanese:
                    resultText.font = kokuMinchoFont;

                    nameText.text = "名前: " + Localization.LocalizeCharacterName(player.name);
                    nameText.font = kokuMinchoFont;

                    roleText.text = "役職: " + Localization.LocalizeRole(player.role);
                    roleText.font = kokuMinchoFont;

                    break;
            }

            audioSources[0].PlayDelayed(0.0f);
            PlayAudio(clappingHandsAudio);
        }
    }

    public void ShowMessageWindow(string localizationId){
        GameObject window = Instantiate(messageWindowPrefab, overlay.transform);
        overlay.SetActive(true);

        Text title = window.transform.GetChild(0).GetComponent<Text>();
        Text message = window.transform.GetChild(1).GetComponent<Text>();
        GameObject button = window.transform.GetChild(2).gameObject;

        Localization.LocalizeText(localizationId + "_Title", title);
        Localization.LocalizeText(localizationId + "_Text", message);
        Localization.LocalizeText("Window_CloseButton", button.gameObject.GetComponentInChildren<Text>());

        button.GetComponent<Button>().onClick.AddListener(() => {
            Destroy(window);
            overlay.SetActive(false);
        });
    }

    public void ShowAudioSettingWindow(){
        GameObject window = Instantiate(audioSettingWindowPrefab, overlay.transform);
        overlay.SetActive(true);

        Text title = window.transform.GetChild(0).GetComponent<Text>();
        Text musicText = window.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        Slider musicVolumeSlider = window.transform.GetChild(1).GetChild(1).GetComponent<Slider>();
        Text musicVolumeText = window.transform.GetChild(1).GetChild(2).GetComponent<Text>();
        Text soundEffectText = window.transform.GetChild(2).GetChild(0).GetComponent<Text>();
        Slider soundEffectVolumeSlider = window.transform.GetChild(2).GetChild(1).GetComponent<Slider>();
        Text soundEffectVolumeText = window.transform.GetChild(2).GetChild(2).GetComponent<Text>();
        GameObject button = window.transform.GetChild(3).gameObject;

        Localization.LocalizeText("AudioSetting_Title", title);
        Localization.LocalizeText("AudioSetting_Music_Text", musicText);
        Localization.LocalizeText("AudioSetting_SoundEffect_Text", soundEffectText);
        Localization.LocalizeText("Window_OKButton", button.GetComponentInChildren<Text>());

        musicVolumeSlider.value = GameManager.instance.musicVolume;
        musicVolumeText.text = ((int)GameManager.instance.musicVolume).ToString();

        soundEffectVolumeSlider.value = GameManager.instance.soundEffectVolume;
        soundEffectVolumeText.text = ((int)GameManager.instance.soundEffectVolume).ToString();

        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        musicVolumeSlider.onValueChanged.AddListener((value) => {
            GameManager.instance.musicVolume = value;
            audioSources[0].volume = value / 100.0f;
            musicVolumeText.text = ((int)value).ToString();

            PlayerPrefs.SetFloat("MusicVolume", value);
        });

        soundEffectVolumeSlider.onValueChanged.RemoveAllListeners();
        soundEffectVolumeSlider.onValueChanged.AddListener((value) => {
            GameManager.instance.soundEffectVolume = value;
            audioSources[1].volume = value / 100.0f;
            soundEffectVolumeText.text = ((int)value).ToString();

            PlayerPrefs.SetFloat("SoundEffectVolume", value);
        });

        button.GetComponent<Button>().onClick.AddListener(() => {
            Destroy(window);
            overlay.SetActive(false);
        });
    }

    public void ShowLanguageSettingWindow(){
        GameObject window = Instantiate(targetSelectWindowPrefab, overlay.transform);
        overlay.SetActive(true);

        Text title = window.transform.GetChild(0).GetComponent<Text>();
        Text description = window.transform.GetChild(1).GetComponent<Text>();
        GameObject scrollViewContent = window.transform.GetChild(2).GetChild(0).GetChild(0).gameObject;
        GameObject button = window.transform.GetChild(3).gameObject;

        Localization.LocalizeText("LanguageSetting_Title", title);
        Localization.LocalizeText("Window_CancelButton", button.GetComponentInChildren<Text>());

        Vector2 scrollViewContentSize = scrollViewContent.GetComponent<RectTransform>().sizeDelta;
        scrollViewContentSize = new Vector2(scrollViewContentSize.x, 0);

        description.gameObject.SetActive(false);

        button.GetComponent<Button>().onClick.AddListener(() => {
            Destroy(window);
            overlay.SetActive(false);
        });

        int count = 0;
        foreach(Localization.Language language in Enum.GetValues(typeof(Localization.Language))){
            if(language != Localization.language){
                count++;

                scrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollViewContentSize.x, count * targetButtonPrefab.GetComponent<RectTransform>().rect.height);
                GameObject languageButton = Instantiate(targetButtonPrefab, scrollViewContent.transform);
                Text languageButtonText = languageButton.GetComponentInChildren<Text>();

                switch(Localization.language){
                    case Localization.Language.English:
                        languageButtonText.font = aftonJamesFont;
                        languageButtonText.fontStyle = FontStyle.Normal;
                        languageButtonText.fontSize = 70;

                        break;
                    case Localization.Language.Japanese:
                        languageButtonText.font = soukouMinchoFont;
                        languageButtonText.fontStyle = FontStyle.Bold;
                        languageButtonText.fontSize = 70;

                        break;
                }

                languageButtonText.text = Localization.LocalizeLanguageName(language);
                languageButton.GetComponent<Button>().onClick.AddListener(() => {
                    Localize(language);

                    Destroy(window);
                    overlay.SetActive(false);
                });
            }
        }
    }

    public void ShowFeedbackWindow(){/*
        switch(Application.platform){
            case RuntimePlatform.WindowsPlayer:


                break;
            case RuntimePlatform.IPhonePlayer:
                if(!UnityEngine.iOS.Device.RequestStoreReview()){
		            //Application.OpenURL("itms-apps://itunes.apple.com/jp/app/idXXXXXXXX?mt=8&action=write-review");
                }

                break;
            case RuntimePlatform.Android:
                //Application.OpenURL("market://details?id=XXXXXXXX");

                break;
        }*/
    }

    private void ShowConfirmWindow(string message, UnityAction action){
        GameObject window = Instantiate(confirmWindowPrefab, overlay.transform);
        overlay.SetActive(true);

        Text titleText = window.transform.GetChild(0).GetComponent<Text>();
        Text messageText = window.transform.GetChild(1).GetComponent<Text>();
        GameObject okButton = window.transform.GetChild(2).gameObject;
        GameObject backButton = window.transform.GetChild(3).gameObject;

        Localization.LocalizeText("ConfirmWindow_Title", titleText);
        Localization.LocalizeText("ConfirmWindow_Text", messageText);
        Localization.LocalizeText("ConfirmWindow_OKButton", okButton.gameObject.GetComponentInChildren<Text>());
        Localization.LocalizeText("ConfirmWindow_CancelButton", backButton.gameObject.GetComponentInChildren<Text>());

        messageText.text = message;

        okButton.GetComponent<Button>().onClick.AddListener(() => {
            Destroy(window);
            overlay.SetActive(false);
        });
        okButton.GetComponent<Button>().onClick.AddListener(action);

        backButton.GetComponent<Button>().onClick.AddListener(() => {
            Destroy(window);
            overlay.SetActive(false);
        });
    }

    public void ShowRoleWindow(Player.Role role){
        GameObject window = Instantiate(roleWindowPrefab, overlay.transform);
        overlay.SetActive(true);

        Text title = window.transform.GetChild(0).GetComponent<Text>();
        Image image = window.transform.GetChild(1).GetComponent<Image>();
        Text message = window.transform.GetChild(2).GetComponent<Text>();
        GameObject button = window.transform.GetChild(3).gameObject;

        Localization.LocalizeText("RoleWindow_" + role.ToString() + "_Title", title);
        Localization.LocalizeText("RoleWindow_" + role.ToString() + "_Text", message);
        Localization.LocalizeText("Window_ContinueButton", button.GetComponentInChildren<Text>());

        switch(role){
            case Player.Role.Murderer:
                image.sprite = murdererIconSprite;
                break;
            case Player.Role.Betrayer:
                image.sprite = betrayerIconSprite;
                break;
            case Player.Role.Policeman:
                image.sprite = policemanIconSprite;
                break;
            case Player.Role.Detective:
                image.sprite = detectiveIconSprite;
                break;
            case Player.Role.Bystander:
                image.sprite = bystanderIconSprite;
                break;
        }

        button.GetComponent<Button>().onClick.AddListener(() => {
            Destroy(window);
            overlay.SetActive(false);
        });
    }

    public void ShowRosterWindow(){
        GameObject window = Instantiate(rosterWindowPrefab, overlay.transform);
        overlay.SetActive(true);

        Text title = window.transform.GetChild(0).GetComponent<Text>();
        GameObject characterList = window.transform.GetChild(1).GetChild(0).GetChild(0).gameObject;
        Vector2 characterListSize = characterList.GetComponent<RectTransform>().sizeDelta;
        GameObject button = window.transform.GetChild(2).gameObject;

        Localization.LocalizeText("RosterWindow_Title", title);
        Localization.LocalizeText("Window_CloseButton", button.GetComponentInChildren<Text>());

        Player player = GameManager.instance.players[GameManager.instance.turn];
        player.metCharacters.Sort((x, y) => string.Compare(Localization.LocalizeCharacterName(x.name), Localization.LocalizeCharacterName(y.name)));

        int count = 0;

        foreach(Character metCharacter in player.metCharacters){
            GameObject character = Instantiate(rosterParticipantPrefab, characterList.transform);
            GameObject portrait = character.transform.GetChild(0).gameObject;
            Text nameText = character.transform.GetChild(1).GetComponent<Text>();
            Button impressionButton = character.transform.GetChild(2).GetComponent<Button>();

            nameText.text = Localization.LocalizeCharacterName(metCharacter.name).ToUpper();
            SetPortrait(character.transform.GetChild(0).GetComponent<Image>(), metCharacter.name);

            if(player.role == Player.Role.Murderer && metCharacter.GetType() == typeof(Player) && ((Player)metCharacter).role == Player.Role.Murderer){
                nameText.color = Color.red;
            }

            switch(Localization.language){
                case Localization.Language.English:
                    nameText.font = ebGaramondFont;

                    break;
                case Localization.Language.Japanese:
                    nameText.font = kokuMinchoFont;

                    break;
            }

            switch(metCharacter.impressions[player]){
                case Character.Impression.Unknown:
                    impressionButton.image.sprite = questionMarkSprite;

                    break;
                case Character.Impression.Friendly:
                    impressionButton.image.sprite = thumbsUpSprite;

                    break;
                case Character.Impression.Unfriendly:
                    impressionButton.image.sprite = thumbsDownSprite;

                    break;
            }
            
            impressionButton.onClick.AddListener(() => {
                switch(metCharacter.impressions[player]){
                    case Character.Impression.Unknown:
                        metCharacter.impressions[player] = Character.Impression.Friendly;
                        impressionButton.image.sprite = thumbsUpSprite;

                        break;
                    case Character.Impression.Friendly:
                        metCharacter.impressions[player] = Character.Impression.Unfriendly;
                        impressionButton.image.sprite = thumbsDownSprite;

                        break;
                    case Character.Impression.Unfriendly:
                        metCharacter.impressions[player] = Character.Impression.Unknown;
                        impressionButton.image.sprite = questionMarkSprite;

                        break;
                }
            });
            
            count++;
        }

        characterList.GetComponent<RectTransform>().sizeDelta = new Vector2(characterListSize.x, rosterParticipantPrefab.GetComponent<RectTransform>().rect.height * count);
        
        button.GetComponent<Button>().onClick.AddListener(() => {
            UpdateActionScreen(GameManager.instance.players[GameManager.instance.turn]);

            Destroy(window);
            overlay.SetActive(false);
        });
    }

    public void ShowMapWindow(){
        overlay.SetActive(true);
        GameObject window = Instantiate(mapWindowPrefab, overlay.transform);

        Localization.LocalizeText("MapWindow_Title", window.transform.GetChild(0).GetComponent<Text>());
        Localization.LocalizeText("Window_CloseButton", window.transform.GetChild(2).GetComponentInChildren<Text>());

        int count = 1;
        foreach(Transform location in window.transform.GetChild(1).GetChild(1)){
            Localization.LocalizeText("Move_LocationButton" + count, location.GetComponentInChildren<Text>());
            count++;
        }

        window.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => {
            Destroy(window);
            overlay.SetActive(false);
        });
    }

    public void AddPlayer(){
        GameObject player = Instantiate(playerPrefab, playerList.transform);
        Text orderText = player.transform.GetChild(0).GetComponent<Text>();
        InputField nameInputField = player.transform.GetChild(1).GetComponent<InputField>();
        Button removeButton = player.transform.GetChild(2).GetComponent<Button>();

        string name = null;
        int count;

        switch(Localization.language){
            case Localization.Language.English:
                count = 0;
                do{
                    count++;
                    name = "Player" + count;
                }while(playerNames.Contains(name));
                
                nameInputField.textComponent.font = ebGaramondFont;
                nameInputField.textComponent.fontSize = 65;

                break;
            case Localization.Language.Japanese:
                count = 0;
                do{
                    count++;
                    name = "プレイヤー" + count;
                }while(playerNames.Contains(name));
                
                nameInputField.textComponent.font = soukouMinchoFont;
                nameInputField.textComponent.fontSize = 60;

                break;
        }

        nameInputField.text = name;
        playerNames.Add(name);

        foreach(Transform content in playerList.transform){
            content.GetChild(0).GetComponent<Text>().text = (content.GetSiblingIndex() + 1).ToString();
        }

        nameInputField.onEndEdit.AddListener((value) => {
            if(playerNames.Contains(value) || value == ""){
                nameInputField.text = playerNames[player.transform.GetSiblingIndex()];

                if(playerNames.Contains(value)){
                    warningText.SetActive(true);

                    switch(Localization.language){
                        case Localization.Language.English:
                            warningText.GetComponent<Text>().text = "That name already exists";

                            break;
                        case Localization.Language.Japanese:
                            warningText.GetComponent<Text>().text = "同じ名前が既に存在します";

                            break;
                    }
                }
                else if(value == ""){
                    warningText.SetActive(true);

                    switch(Localization.language){
                        case Localization.Language.English:
                            warningText.GetComponent<Text>().text = "Set name is invalid";

                            break;
                        case Localization.Language.Japanese:
                            warningText.GetComponent<Text>().text = "入力内容が不適切です";

                            break;
                    }
                }
            }
            else{
                if(playerNames.Contains(nameInputField.text)){
                    playerNames.Remove(nameInputField.text);
                }

                playerNames[player.transform.GetSiblingIndex()] = value;

                if(playerNames.Count < GameManager.playerLowerLimit){
                    warningText.SetActive(true);

                    switch(Localization.language){
                        case Localization.Language.English:
                            warningText.GetComponent<Text>().text = (GameManager.playerLowerLimit - playerNames.Count) + " more players to play";

                            break;
                        case Localization.Language.Japanese:
                            warningText.GetComponent<Text>().text = "プレイにはあと" + (GameManager.playerLowerLimit - playerNames.Count) + "人のプレイヤーが必要です";

                            break;
                    }
                }
                else{
                    warningText.SetActive(false);
                }
            }
        });

        removeButton.onClick.AddListener(() => {
            playerNames.Remove(playerNames[player.transform.GetSiblingIndex()]);
            DestroyImmediate(player);

            foreach(Transform content in playerList.transform){
                content.GetChild(0).GetComponent<Text>().text = (content.GetSiblingIndex() + 1).ToString();
            }

            if(playerNames.Count < GameManager.playerLowerLimit){
                settingFinishButton.GetComponent<Button>().interactable = false;
            }
            else if(playerNames.Count == GameManager.playerUpperLimit - 1){
                addPlayerButton.GetComponent<Button>().interactable = true;
            }

            if(playerNames.Count < GameManager.playerLowerLimit){
                warningText.SetActive(true);

                switch(Localization.language){
                    case Localization.Language.English:
                        warningText.GetComponent<Text>().text = (GameManager.playerLowerLimit - playerNames.Count) + " more players to play";

                        break;
                    case Localization.Language.Japanese:
                        warningText.GetComponent<Text>().text = "プレイにはあと" + (GameManager.playerLowerLimit - playerNames.Count) + "人のプレイヤーが必要です";

                        break;
                }
            }
            else{
                warningText.SetActive(false);
            }
        });

        if(GameManager.playerLowerLimit == playerNames.Count){
            settingFinishButton.GetComponent<Button>().interactable = true;
        }
        else if(GameManager.playerUpperLimit == playerNames.Count){
            addPlayerButton.GetComponent<Button>().interactable = false;
        }

        if(playerNames.Count < GameManager.playerLowerLimit){
            warningText.SetActive(true);

            switch(Localization.language){
                case Localization.Language.English:
                    warningText.GetComponent<Text>().text = (GameManager.playerLowerLimit - playerNames.Count) + " more players to play";

                    break;
                case Localization.Language.Japanese:
                    warningText.GetComponent<Text>().text = "プレイにはあと" + (GameManager.playerLowerLimit - playerNames.Count) + "人のプレイヤーが必要です";

                    break;
            }
        }
        else{
            warningText.SetActive(false);
        }
    }
    public void AddPlayer(string name){
        GameObject player = Instantiate(playerPrefab, playerList.transform);
        Text orderText = player.transform.GetChild(0).GetComponent<Text>();
        InputField nameInputField = player.transform.GetChild(1).GetComponent<InputField>();
        Button removeButton = player.transform.GetChild(2).GetComponent<Button>();

        switch(Localization.language){
            case Localization.Language.English:
                nameInputField.textComponent.font = ebGaramondFont;
                nameInputField.textComponent.fontSize = 65;

                break;
            case Localization.Language.Japanese:
                nameInputField.textComponent.font = soukouMinchoFont;
                nameInputField.textComponent.fontSize = 60;

                break;
        }

        nameInputField.text = name;

        foreach(Transform content in playerList.transform){
            content.GetChild(0).GetComponent<Text>().text = (content.GetSiblingIndex() + 1 - playerNames.Count).ToString();
        }

        nameInputField.onEndEdit.AddListener((value) => {
            if(playerNames.Contains(value) || value == ""){
                nameInputField.text = playerNames[player.transform.GetSiblingIndex()];

                if(playerNames.Contains(value)){
                    warningText.SetActive(true);

                    switch(Localization.language){
                        case Localization.Language.English:
                            warningText.GetComponent<Text>().text = "That name already exists";

                            break;
                        case Localization.Language.Japanese:
                            warningText.GetComponent<Text>().text = "同じ名前が既に存在します";

                            break;
                    }
                }
                else if(value == ""){
                    warningText.SetActive(true);

                    switch(Localization.language){
                        case Localization.Language.English:
                            warningText.GetComponent<Text>().text = "Set name is invalid";

                            break;
                        case Localization.Language.Japanese:
                            warningText.GetComponent<Text>().text = "入力内容が不適切です";

                            break;
                    }
                }
            }
            else{
                if(playerNames.Contains(nameInputField.text)){
                    playerNames.Remove(nameInputField.text);
                }

                playerNames[player.transform.GetSiblingIndex()] = value;

                if(playerNames.Count < GameManager.playerLowerLimit){
                    warningText.SetActive(true);

                    switch(Localization.language){
                        case Localization.Language.English:
                            warningText.GetComponent<Text>().text = (GameManager.playerLowerLimit - playerNames.Count) + " more players to play";

                            break;
                        case Localization.Language.Japanese:
                            warningText.GetComponent<Text>().text = "プレイにはあと" + (GameManager.playerLowerLimit - playerNames.Count) + "人のプレイヤーが必要です";

                            break;
                    }
                }
                else{
                    warningText.SetActive(false);
                }
            }
        });

        removeButton.onClick.AddListener(() => {
            playerNames.Remove(playerNames[player.transform.GetSiblingIndex()]);
            DestroyImmediate(player);

            foreach(Transform content in playerList.transform){
                content.GetChild(0).GetComponent<Text>().text = (content.GetSiblingIndex() + 1).ToString();
            }

            if(playerNames.Count < GameManager.playerLowerLimit){
                settingFinishButton.GetComponent<Button>().interactable = false;
            }
            else if(playerNames.Count == GameManager.playerUpperLimit - 1){
                addPlayerButton.GetComponent<Button>().interactable = true;
            }

            if(playerNames.Count < GameManager.playerLowerLimit){
                warningText.SetActive(true);

                switch(Localization.language){
                    case Localization.Language.English:
                        warningText.GetComponent<Text>().text = (GameManager.playerLowerLimit - playerNames.Count) + " more players to play";

                        break;
                    case Localization.Language.Japanese:
                        warningText.GetComponent<Text>().text = "プレイにはあと" + (GameManager.playerLowerLimit - playerNames.Count) + "人のプレイヤーが必要です";

                        break;
                }
            }
            else{
                warningText.SetActive(false);
            }
        });

        if(GameManager.playerLowerLimit == playerNames.Count){
            settingFinishButton.GetComponent<Button>().interactable = true;
        }
        else if(GameManager.playerUpperLimit == playerNames.Count){
            addPlayerButton.GetComponent<Button>().interactable = false;
        }

        if(playerNames.Count < GameManager.playerLowerLimit){
            warningText.SetActive(true);

            switch(Localization.language){
                case Localization.Language.English:
                    warningText.GetComponent<Text>().text = (GameManager.playerLowerLimit - playerNames.Count) + " more players to play";

                    break;
                case Localization.Language.Japanese:
                    warningText.GetComponent<Text>().text = "プレイにはあと" + (GameManager.playerLowerLimit - playerNames.Count) + "人のプレイヤーが必要です";

                    break;
            }
        }
        else{
            warningText.SetActive(false);
        }
    }

    public void DeletePlayers(){
        playerNames = new List<string>();
    }

    public void ResetScroll(GameObject screen){
        RectTransform screenTransform = screen.transform.parent.GetComponent<RectTransform>();
        screenTransform.position = new Vector3(screenTransform.position.x, 0f, screenTransform.position.z);
    }

    public void ToggleHelpScreen(){
        ToggleScreen(helpScreen);
        ToggleScreen(informationHelpPanel);

        if(actionScreen.activeSelf){
            ToggleScreen(actionHelpScreen);
        }
        else if(abilityScreen.activeSelf){
            ToggleScreen(abilityHelpScreen);
        }
        else if(moveScreen.activeSelf){
            ToggleScreen(moveHelpScreen);
        }
    }

    private void ToggleScreen(GameObject screen){
        if(screen.activeSelf){
            screen.SetActive(false);
        }
        else{
            screen.SetActive(true);
        }
    }

    private void SetPortrait(Image portrait, GameManager.CharacterName name){
        portrait.sprite = portraitSprites[(int)name];
    }

    public void Pass(){
        string confirmMessage = null;
        switch(Localization.language){
            case Localization.Language.English:
                confirmMessage = "Do you pass this turn?";

                break;
            case Localization.Language.Japanese:
                confirmMessage = "このターンをパスしますか？";

                break;
        }

        ShowConfirmWindow(confirmMessage, () => {
            GameManager.instance.OnActionFinished();
        });
    }

    private void TickTimer(){
        if(!isTimerActivated){
            return;
        }

        string buttonTextString = null;

        switch(Localization.language){
            case Localization.Language.English:
                buttonTextString = "CONTINUE";

                break;
            case Localization.Language.Japanese:
                buttonTextString = "次へ";

                break;
        }

        discussionTime -= Time.deltaTime;
        discussionTimeSpan = new TimeSpan(0, 0, (int)discussionTime);

        if(discussionTime <= 0f){
            discussionTime = 0f;
            isTimerActivated = false;
            audioSources[1].Stop();

            skipDiscussionButton.transform.GetChild(0).GetComponent<Text>().text = buttonTextString;
            PlayAudio(clockChimeAudio);
        }

        timerText.GetComponent<Text>().text = string.Format("{0:D2}:{1:D2}", discussionTimeSpan.Minutes, discussionTimeSpan.Seconds);
    }

    public void AdjustTimer(int minute){
        if(discussionTime + minute * 60 < 0){
            discussionTime = 0;
        }
        else if(60 * 30 < discussionTime + minute * 60){
            discussionTime = 60 * 30;
        }
        else{
            discussionTime += minute * 60;
        }

        if(!isTimerActivated && 0 < minute){
            audioSources[1].Play();
            isTimerActivated = true;
        }

        if(0 < minute){
            switch(Localization.language){
                case Localization.Language.English:
                    skipDiscussionButton.transform.GetChild(0).GetComponent<Text>().text = "SKIP";

                    break;
                case Localization.Language.Japanese:
                    skipDiscussionButton.transform.GetChild(0).GetComponent<Text>().text = "終了";

                    break;
            }
        }
    }

    public void StopTimer(){
        isTimerActivated = false;
        audioSources[1].Stop();
    }

    public void PlayAgain(){
        int count;
        int playerCount = playerNames.Count;
        
        for(count = 0; count < playerCount; count++){
            AddPlayer(playerNames[count]);
        }

        neighborNotificationText.SetActive(false);
    }
    
    public void Localize(Localization.Language language){
        Localization.SetLocalizationTarget(language);
        PlayerPrefs.SetInt("Language", (int)language);

        List<GameObject> texts = new List<GameObject>();
        GetTextInChildren(canvas.transform, ref texts);

        foreach(GameObject text in texts){
            if(text.GetComponent<TextIdentifier>() != null){
                Localization.LocalizeText(text.GetComponent<TextIdentifier>().id, text.GetComponent<Text>());
            }
        }

        foreach(Transform book in pictureBook.transform.GetChild(0)){
            Localization.LocalizePictureBook(book.gameObject);
        }
    }

    private void GetTextInChildren(Transform parent, ref List<GameObject> texts){
        if(parent.childCount == 0){
            return;
        }

        foreach(Transform child in parent){
            if(child.tag == "Text" || child.tag == "ButtonText"){
                texts.Add(child.gameObject);
            }

            GetTextInChildren(child, ref texts);
        }
    }
    
    public void PlayAudio(AudioClip audioClip){
        audioSources[1].PlayOneShot(audioClip);
    }

}
