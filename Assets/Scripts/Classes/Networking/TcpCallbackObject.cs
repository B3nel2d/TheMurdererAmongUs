using System.Net.Sockets;

public class TcpCallbackObject{
    
    public Socket socket{
        get;
        set;
    }

    public byte[] buffer{
        get;
        set;
    }

    public TcpCallbackObject(Socket socket){
        this.socket = socket;
        buffer = new byte[1024];
    }

}
