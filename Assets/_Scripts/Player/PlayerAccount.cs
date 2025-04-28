using System;

[Serializable]
public class PlayerAccount
{
    public string UserID { get; private set; }
    public string DisplayName { get; private set; }
    public bool IsLocalPlayer { get; private set; }

    public bool IsConnected { get; private set; }

    public bool HasAnyActiveUnits { get; private set; }

    public PlayerAccount(string userID, string displayName, bool isLocal)
    {
        UserID = userID;
        DisplayName = displayName;
        IsLocalPlayer = isLocal;
        HasAnyActiveUnits = true;
    }

    public override string ToString()
    {
        return $"{DisplayName} ({UserID})";
    }

    public void Eliminate()
    {
        HasAnyActiveUnits = false;
        if (IsLocalPlayer)
        {
            //Show game over screen for that player
        }
        else
        {
            //Show that a player is out
        }
    }
}