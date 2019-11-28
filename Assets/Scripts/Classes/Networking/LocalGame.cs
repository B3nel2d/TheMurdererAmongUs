using System.Net;

public class LocalGame{

    public string name{
        get;
        set;
    }

    public string password{
        get;
        set;
    }

    public IPAddress ipAddress{
        get;
        set;
    }

    /******************************/

    public LocalGame(IPAddress ipAddress, string name, string password){
        this.name = name;
        this.password = password;
        this.ipAddress = ipAddress;
    }
    public LocalGame(IPAddress ipAddress, string name){
        this.name = name;
        this.password = "";
        this.ipAddress = ipAddress;
    }

}
