using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviour{

    private static NetworkManager singleton;
    public static NetworkManager instance{
        get{
            return singleton;
        }
        private set{
            singleton = value;
        }
    }

    private Server server;
    private Client client;

    private IPAddress ipAddress;
    private const int tcpPort = 45000;

    UdpClient udpSenderClient;
    UdpClient udpReceiverClient;

    private const int udpSenderPort = 45001;
    private const int udpReceiverPort = 45002;
    private IPEndPoint senderMulticastEndPoint;
    private IPEndPoint receiverMulticastEndPoint;
    private IPEndPoint remoteEndPoint;

    private LocalGame createdGame;
    private LocalGame selectedGame;
    public List<LocalGame> localGames{
        get;
        set;
    }

    bool listeningOnLocal;
    bool searchingGame;

    [SerializeField] GameObject localNetworkPlayScreen;
    [SerializeField] GameObject gameListScreen;
    [SerializeField] GameObject gameRoomScreen;
    [SerializeField] GameObject overlay;

    [SerializeField] GameObject gameList;
    [SerializeField] GameObject joinButton;
    [SerializeField] GameObject gameNameText;
    [SerializeField] GameObject playerList;
    [SerializeField] GameObject readyButton;
    [SerializeField] GameObject backButton;

    [SerializeField] GameObject gameCreateWindowPrefab;
    [SerializeField] GameObject localGamePrefab;
    [SerializeField] GameObject lobbyPlayerPrefab;

    /******************************/

    void Awake(){
        Setup();
    }

    void Start(){
        Initialize();
    }

    /******************************/

    private void Setup(){
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Destroy(gameObject);
        }
    }

    private void Initialize(){
        server = null;
        client = null;

        ipAddress = null;

        udpSenderClient = null;
        udpReceiverClient = null;

        senderMulticastEndPoint = null;
        remoteEndPoint = null;

        createdGame = null;
        selectedGame = null;
        localGames = new List<LocalGame>();

        listeningOnLocal = false;
        searchingGame = false;
    }

    private void Listen(string ipAddress){
		if(server != null){
			Debug.Log("Server is already listening.");
			return;
		}

		server = new Server();

		try{
			server.Listen(ipAddress, tcpPort, 100);
		}
		catch(Exception exception){
			Debug.Log("Failed to listen: " + exception.Message);

			return;
		}

        this.ipAddress = IPAddress.Parse(ipAddress);
        
		server.ClientAccepted += new Server.ServerEventHandler(AcceptClient);
		server.ClientDisconnected += new Server.ServerEventHandler(DisconnectClient);
        server.CommandReceived += new Server.CommandEventHandler(ExecuteCommandFromClient);
        server.ErrorReceived += new Server.ErrorEventHandler(ReceiveError);

		Debug.Log("Started to listen " + server.localEndPoint.ToString() + ".");
    }
	private void Listen(){
        IPAddress ipAddress = null;

        try{
            foreach(IPAddress ip in Dns.Resolve(Dns.GetHostName()).AddressList){
                if(ip.AddressFamily == AddressFamily.InterNetwork){
                    ipAddress = ip;
                }
            }
        }
        catch(Exception exception){
            Debug.Log(exception.Message);
        }

        if(ipAddress != null){
            //this.Listen(ipAddress);
        }
        else{
            Debug.Log("Failed to get IP address.");
        }
	}

	private void StopListen(){
		if(server == null){
			Debug.Log("Server is not listening.");
			return;
		}

		server.Close();
		server = null;
        ipAddress = null;

		Debug.Log("Stopped listening.");
	}

	private void Connect(string ipAddress){
		if(client != null){
			Debug.Log("Client is already connected to server.");
			return;
		}

		client = new Client();
        
		client.Connected += new Client.ClientEventHandler(ConnectToServer);
		client.Disconnected += new Client.ClientEventHandler(DisconnectFromServer);
        client.CommandReceived += new Client.CommandEventHandler(ExecuteCommandFromServer);
        client.ErrorReceived += new Client.ErrorEventHandler(ReceiveError);
        
		try{
			client.Connect(ipAddress, tcpPort);
		}
		catch(Exception exception){
			client.Close();
			Debug.Log("Failed to connect to server: " + exception.Message);
			return;
		}
	}
    
    private void Disconnect(){
        if(client != null){
            client.Close();
            client = null;
        }
    }

	private void SendToServer(byte[] data){
		if(client == null){
			Debug.Log("Not connected to server.");
			return;
		}
        

		try{
		    client.Send(data);
		}
		catch(Exception exception){
			Debug.Log("Failed to send data: " + exception.Message);
			return;
		}
	}

    private void HostGame(){
        Listen("127.0.0.1");
        Connect("127.0.0.1");
    }

    private void SendUdpMessage(string message){
        senderMulticastEndPoint = new IPEndPoint(IPAddress.Parse("239.0.0.0"), udpReceiverPort);
        udpSenderClient = new UdpClient(udpReceiverPort, AddressFamily.InterNetwork);

        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
        UdpCallbackObject callbackObject = new UdpCallbackObject(udpSenderClient, message);

        udpSenderClient.JoinMulticastGroup(senderMulticastEndPoint.Address, 50);
        udpSenderClient.BeginSend(buffer, buffer.Length, senderMulticastEndPoint, SendUdpMessageCallback, callbackObject);
    }

    private void SendUdpMessageCallback(IAsyncResult result){
        udpSenderClient = ((UdpCallbackObject)result).udpClient; Debug.Log("SENT");

        try{
            udpSenderClient.EndSend(result); Debug.Log(((UdpCallbackObject)result).sentMessage);
        }
        catch(Exception exception){
            Debug.Log(exception.Message);
        }

        if(listeningOnLocal){
            SendUdpMessage(((UdpCallbackObject)result).sentMessage);
        }
        else{
            udpSenderClient.DropMulticastGroup(senderMulticastEndPoint.Address);
            udpSenderClient.Close();
            udpSenderClient = null;
        }
    }

    private void ReceiveUdpMessage(){
        receiverMulticastEndPoint = new IPEndPoint(IPAddress.Parse("239.0.0.0"), udpReceiverPort);
        udpReceiverClient = new UdpClient(AddressFamily.InterNetwork);
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, udpSenderPort);
        udpReceiverClient.Client.Bind(localEndPoint);

        udpReceiverClient.JoinMulticastGroup(receiverMulticastEndPoint.Address, 50);
        udpReceiverClient.BeginReceive(ReceiveUdpMessageCallBack, udpReceiverClient);
    }

    private void ReceiveUdpMessageCallBack(IAsyncResult result){
        udpReceiverClient = (UdpClient)result;
        remoteEndPoint = null;
        byte[] buffer = null; Debug.Log("RECEIVED");

        try{
            buffer = udpReceiverClient.EndReceive(result, ref remoteEndPoint);
        }
        catch(Exception exception){
            Debug.Log(exception.Message);
        }

        string receivedMessage = System.Text.Encoding.UTF8.GetString(buffer);
        string[] splitMessage = receivedMessage.Split('/');
        LocalGame foundGame = new LocalGame(IPAddress.Parse(splitMessage[0]), splitMessage[1], splitMessage[2]);

        if(!localGames.Exists(game => game.ipAddress == foundGame.ipAddress)){
            localGames.Add(foundGame);
        }
        
        if(searchingGame){
            udpReceiverClient.BeginReceive(ReceiveUdpMessageCallBack, udpReceiverClient);
        }
        else{
            udpReceiverClient.DropMulticastGroup(receiverMulticastEndPoint.Address);
            udpReceiverClient.Close();
            udpReceiverClient = null;
        }
    }

    private void AcceptClient(Client client){
		Debug.Log(client.remoteEndPoint.Address.ToString() + " connected to server.");
	}

	private void DisconnectClient(Client client){
		Debug.Log(client.remoteEndPoint.Address.ToString() + " disconnected from server.");
	}

    private void ExecuteCommandFromClient(Socket socket, byte[] command){
        Debug.Log("Received command(" + command + ") from " + socket.LocalEndPoint.ToString());
    }

	private void ConnectToServer(Client client){
		this.client = client;
		Debug.Log("Connected to server.");
	}
    
	private void DisconnectFromServer(Client client){
		this.client = client;
		Debug.Log("Disconnected from server.");
	}

    private void ExecuteCommandFromServer(Socket socket, byte[] command){
        Debug.Log("Received command(" + command + ") from " + socket.LocalEndPoint.ToString());
    }

    private void ReceiveError(string errorMessage){
        Debug.Log(errorMessage);
    }

    /******************************/

    public void CreateLocalGame(){
        GameObject window = Instantiate(gameCreateWindowPrefab, overlay.transform);
        overlay.SetActive(true);

        InputField gameName = window.transform.GetChild(1).GetComponent<InputField>();
        InputField password = window.transform.GetChild(2).GetComponent<InputField>();
        Button button = window.transform.GetChild(3).GetComponent<Button>();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => {
            if(gameName.text == "" || 12 < gameName.text.Length || 10 < password.text.Length){
                Debug.Log("Invalid name or password.");
                return;
            }
            if(localGames.Exists(game => game.name == gameName.text)){
                Debug.Log("There is already game that has same name.");
                return;
            }

            LocalGame localGame = null;
            if(password.text == ""){
                localGame = new LocalGame(ipAddress, gameName.text);
            }
            else{
                localGame = new LocalGame(ipAddress, gameName.text, password.text);
            }

            //createdGame = localGame;

            //HostGame();
            //SendUdpMessage(localGame.ipAddress + "/" + localGame.name + "/" + localGame.password);
            listeningOnLocal = true;

            UIManager.instance.ChangeScreen(gameRoomScreen);
            gameNameText.GetComponent<Text>().text = localGame.name;

            backButton.GetComponent<Button>().onClick.RemoveAllListeners();
            backButton.GetComponent<Button>().onClick.AddListener(() => {
                //DeleteLocalGame();
                UIManager.instance.ChangeScreen(localNetworkPlayScreen);
            });

            Destroy(window.gameObject);
            overlay.SetActive(false);
        });
    }

    public void DeleteLocalGame(){
        Disconnect();
        StopListen();

        if(createdGame != null){
            createdGame = null;
        }

        listeningOnLocal = false;
        udpSenderClient.DropMulticastGroup(senderMulticastEndPoint.Address);
        udpSenderClient.Close();
        udpSenderClient = null;
    }

    public void SearchLocalGame(){
        searchingGame = true;
        //ReceiveUdpMessage();
    }

    public void UpdateGameList(){
        foreach(Transform game in gameList.transform){
            Destroy(game.gameObject);
        }

        Vector2 gameListSize = gameList.GetComponent<RectTransform>().sizeDelta;
        gameListSize = new Vector2(gameListSize.x, 0);

        int count = 0;
        foreach(LocalGame game in localGames){
            count++;

            gameList.GetComponent<RectTransform>().sizeDelta = new Vector2(gameListSize.x, count * localGamePrefab.GetComponent<RectTransform>().rect.height + count * 10);
            GameObject localGame = Instantiate(localGamePrefab, gameList.transform);

            localGame.GetComponentInChildren<Text>().text = game.name;
            localGame.transform.GetChild(1).gameObject.SetActive(game.password != "");

            localGame.GetComponent<Button>().onClick.AddListener(() => {
                selectedGame = game;
            });
        }
    }

    public void JoinLocalGame(){
        try{
            Connect(selectedGame.ipAddress.ToString());
        }
        catch(Exception exception){
            Debug.Log("Failed to join the game: " + exception.Message);
            return;
        }

        UIManager.instance.ChangeScreen(gameRoomScreen);

        backButton.GetComponent<Button>().onClick.RemoveAllListeners();
        backButton.GetComponent<Button>().onClick.AddListener(() => {
            StopSearchLocalGame();
            UIManager.instance.ChangeScreen(gameListScreen);
        });
    }

    public void StopSearchLocalGame(){
        searchingGame = false;
        selectedGame = null;
        localGames.Clear();

        //udpReceiverClient.DropMulticastGroup(receiverMulticastEndPoint.Address);
        //udpReceiverClient.Close();
        //udpReceiverClient = null;
    }

    public void OnApplicationQuit(){
        if(server == null){
            return;
        }

        server.Close();
    }

}
