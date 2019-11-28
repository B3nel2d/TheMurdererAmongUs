using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server{

    public enum Command{
        SetName,
        GetReady
    }

    /******************************/

    public Socket serverSocket{
        get;
        set;
    }

    public IPEndPoint localEndPoint{
        get;
        set;
    }

    public List<Client> clients{
        get;
        set;
    }

    public List<Client> disconnectedClients{
        get;
        set;
    }

    private const int maximumConnections = 12;

    private readonly object lockObject = new object();

    /******************************/

    public Server(){
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        clients = new List<Client>();
        disconnectedClients = new List<Client>();
    }

    public void Listen(string ipString, int port, int backlog){
        if(serverSocket == null){
            return;
        }

        IPAddress ipAddress;
        if(!IPAddress.TryParse(ipString, out ipAddress)){
            ipAddress = IPAddress.Parse("127.0.0.1");
        }

        localEndPoint = new IPEndPoint(ipAddress, port);

        serverSocket.Bind(localEndPoint);
        serverSocket.Listen(backlog);

        try{
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), serverSocket);
        }
        catch(SocketException exception){
            Debug.Log("Cannot access the socket: " + exception.Message);
        }
        catch(ObjectDisposedException exception){
            Debug.Log("Client socket is closed: " + exception.Message);
        }
    }

    private void AcceptCallback(IAsyncResult result){
        Socket clientSocket = null;
        Client client = null;
        TcpCallbackObject callbackObject = null;
        
        try{
            lock(lockObject){
                clientSocket = serverSocket.EndAccept(result);
                client = new Client(clientSocket);
                callbackObject = new TcpCallbackObject(clientSocket);
            }
        }
        catch(SocketException exception){
            Debug.Log("Cannot access the socket: " + exception.Message);
            client.Close();

            return;
        }
        catch(ObjectDisposedException exception){
            Debug.Log("Client socket is closed: " + exception.Message);
            client.Close();

            return;
        }

        if(maximumConnections <= clients.Count){
            client.Close();
        }
        else{
            clients.Add(client);

            client.Disconnected += new Client.ClientEventHandler(RemoveClient);
            client.CommandReceived += new Client.CommandEventHandler(ReceiveCommand);

            OnClientAccepted(client);

            try{
                clientSocket.BeginReceive(callbackObject.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(ReceiveCallback), callbackObject);
            }
            catch(SocketException exception){
                Debug.Log("Cannot access the socket: " + exception.Message);
            }
            catch(ObjectDisposedException exception){
                Debug.Log("Client socket is closed: " + exception.Message);
            }
        }

        try{
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), serverSocket); //Write EndAccept somewhere
        }
        catch(SocketException exception){
            Debug.Log("Cannot access the socket: " + exception.Message);
        }
        catch(ObjectDisposedException exception){
            Debug.Log("Client socket is closed: " + exception.Message);
        }
    }

    private void ReceiveCallback(IAsyncResult asyncResult){
        Socket clientSocket = null;
        TcpCallbackObject callbackObject = null;
        int receivedDataSize = 0;
        SocketError errorCode;

        try{
            lock(lockObject){
                callbackObject = (TcpCallbackObject)asyncResult.AsyncState;
                clientSocket = callbackObject.socket;
                
                receivedDataSize = clientSocket.EndReceive(asyncResult, out errorCode);
                if(errorCode != SocketError.Success){
                    receivedDataSize = 0;
                }
            }
        }
        catch(SocketException exception){
            Debug.Log("Cannot access the socket: " + exception.Message);
            clientSocket.Close();

            return;
        }
        catch(ObjectDisposedException exception){
            Debug.Log("Client socket is closed: " + exception.Message);
            clientSocket.Close();

            return;
        }

        if(receivedDataSize <= 0){
            clientSocket.Close();
            clients.Remove(new Client(clientSocket));

            return;
        }
        else{
            OnCommandReceived(serverSocket, callbackObject.buffer);

            try{
                clientSocket.BeginReceive(callbackObject.buffer, 0, 1024, SocketFlags.None, new AsyncCallback(ReceiveCallback), callbackObject);
            }
            catch(SocketException exception){
                Debug.Log("Cannot access the socket: " + exception.Message);
            }
            catch(ObjectDisposedException exception){
                Debug.Log("Client socket is closed: " + exception.Message);
            }
        }
    }

    public void Send(Socket clientSocket, byte[] data){
        try{
            clientSocket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), clientSocket);
        }
        catch(SocketException exception){
            Debug.Log("Cannot access the socket: " + exception.Message);
        }
        catch(ObjectDisposedException exception){
            Debug.Log("Client socket is closed: " + exception.Message);
        }
    }

    public void SendToAllClients(byte[] data){
        lock(((ICollection)clients).SyncRoot){
            foreach(Client client in clients){
                Send(client.clientSocket, data);
            }
        }
    }

    private void SendCallback(IAsyncResult result){
        Socket clientSocket = null;
        int sentDataSize = 0;

        try{
            lock(lockObject){
                clientSocket = (Socket)result.AsyncState;
                sentDataSize = serverSocket.EndSend(result);
            }
        }
        catch(SocketException exception){
            Debug.Log("Cannot access the socket: " + exception.Message);
            clientSocket.Close();
        }
        catch(ObjectDisposedException exception){
            Debug.Log("Client socket is closed: " + exception.Message);
            clientSocket.Close();
        }
    }

    public void Close(){
        Stop();
        DisconnectAllClients();
    }

    public void Stop(){
        lock(lockObject){
            serverSocket.Close();
            serverSocket = null;
        }
    }

    private void DisconnectAllClients(){
        if(clients.Count == 0){
            return;
        }

        lock(lockObject){
            foreach(Client client in clients){
                client?.Close();
            }

            //clients.Clear();
        }
    }

	private void ReceiveCommand(Socket socket, byte[] command){
		OnCommandReceived(socket, command);
	}

	private void RemoveClient(Client client){
        disconnectedClients.Add(client);
		OnClientDisconnected(client);
	}

    /******************************/

    public delegate void ServerEventHandler(Client client);
    public delegate void CommandEventHandler(Socket socket, byte[] command);
    public delegate void ErrorEventHandler(string errorMessage);

    public event ServerEventHandler ClientAccepted;
    private void OnClientAccepted(Client client){
        ClientAccepted?.Invoke(client);
    }

    public event ServerEventHandler ClientDisconnected;
    private void OnClientDisconnected(Client client){
        ClientDisconnected?.Invoke(client);
    }

    public event CommandEventHandler CommandReceived;
    private void OnCommandReceived(Socket socket, byte[] command){
        CommandReceived?.Invoke(socket, command);
    }

	public event ErrorEventHandler ErrorReceived;
    private void OnErrorRececived(string errorMessage){
        ErrorReceived?.Invoke(errorMessage);
    }

}
