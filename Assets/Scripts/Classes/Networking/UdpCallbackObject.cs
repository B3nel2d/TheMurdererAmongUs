using System.Net.Sockets;

public class UdpCallbackObject{

    public UdpClient udpClient{
        get;
        set;
    }

    public string sentMessage{
        get;
        set;
    }

    /******************************/

    public UdpCallbackObject(UdpClient udpClient, string sentMessage){
        this.udpClient = udpClient;
        this.sentMessage = sentMessage;
    }

}
