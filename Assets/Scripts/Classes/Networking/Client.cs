using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Client{

    public enum Command{
        SetName,
        GetReady
    }

    /******************************/

    public Socket clientSocket{
        get;
        set;
    }

    public IPEndPoint localEndPoint{
        get;
        set;
    }
    
    public IPEndPoint remoteEndPoint{
        get;
        set;
    }
    
    public bool isClosed{
        get{
            return clientSocket == null;
        }
    }

    public int maximumDataSize = int.MaxValue;

    private bool isReceiving = false;

    private readonly object lockObject = new object();

    /******************************/

    public Client(){
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }
    public Client(Socket socket){
        clientSocket = socket;

        localEndPoint = (IPEndPoint)socket.LocalEndPoint;
        remoteEndPoint = (IPEndPoint)socket.RemoteEndPoint;
    }

    public void Connect(string ipString, int port){
        if(isClosed){
            return;
        }

        IPAddress ipAddress;
        if(!IPAddress.TryParse(ipString, out ipAddress)){
            foreach(IPAddress ip in Dns.GetHostEntry(ipString).AddressList){
                if(ip.AddressFamily == AddressFamily.InterNetwork){
                    ipAddress = ip;
                }
            }
        }

        clientSocket.Connect(new IPEndPoint(ipAddress, port));

        localEndPoint = (IPEndPoint)clientSocket.LocalEndPoint;
        remoteEndPoint = (IPEndPoint)clientSocket.RemoteEndPoint;

        OnConnected(this);

        Receive();
    }

    private void Receive(){
        if(isClosed || isReceiving){
            return;
        }

        isReceiving = true;
        byte[] buffer = new byte[1024];

        try{
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), buffer);
        }
        catch(SocketException exception){
            Debug.Log(exception.Message);
        }
        catch(ObjectDisposedException exception){
            Debug.Log(exception.Message);
        }
    }

    private void ReceiveCallback(IAsyncResult result){
        byte[] buffer = null;
		int receivedDataSize = -1;
		
		try{
			lock(lockObject){
                receivedDataSize = clientSocket.EndReceive(result);
			}
		}
        catch(SocketException exception){
            Debug.Log(exception.Message);
            Close();

            return;
        }
        catch(ObjectDisposedException exception){
            Debug.Log(exception.Message);
            Close();

            return;
        }
		
		if(receivedDataSize <= 0){
			Close();

			return;
		}
        else{
            buffer = (byte[])result.AsyncState;
            OnCommandReceived(clientSocket, buffer);
        }
        
        try{
		    lock(lockObject){
                isReceiving = false;
			    clientSocket.BeginReceive(buffer, 0, 1024, SocketFlags.None, new AsyncCallback(ReceiveCallback), buffer);
		    }
        }
        catch(SocketException exception){
            Debug.Log(exception.Message);
        }
        catch(ObjectDisposedException exception){
            Debug.Log(exception.Message);
        }
	}

    public void Send(byte[] data){
        if(isClosed){
            return;
        }

        lock(lockObject){
            try{
                clientSocket.Send(data);
            }
            catch(SocketException exception){
                Debug.Log(exception.Message);
            }
            catch(ObjectDisposedException exception){
                Debug.Log(exception.Message);
            }
        }
    }

    public void Close(){
        lock(lockObject){
            if(isClosed){
                return;
            }
            
            try{
                clientSocket.Shutdown(SocketShutdown.Both);
            }
            catch(ObjectDisposedException exception){
                Debug.Log("Cannot access a disposed socket: " + exception.Message);
            }

            clientSocket.Close();
            clientSocket = null;

            OnDisconnected(this);
        }
    }

    /******************************/

    public delegate void ClientEventHandler(Client client);
    public delegate void CommandEventHandler(Socket socket, byte[] command);
    public delegate void ErrorEventHandler(string errorMessage);

	public event ClientEventHandler Connected;
	private void OnConnected(Client client){
        Connected?.Invoke(this);
	}

	public event ClientEventHandler Disconnected;
	private void OnDisconnected(Client client){
        Disconnected?.Invoke(this);
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
