using System.Collections.Generic;

[System.Serializable]
public class ProfileResponse
{
    public bool isConfirmed;
    public ulong points;
    public int dailyStreaks;
    public int keys;
    public float powerStones;
    public List<ScannedObject> scannedObjects;
    public string email;
    public string country;
    public string firstName;
    public string lastName;
    public string playerType;
    public string avatarId;
    public string birthDate;
    public string gender;
    public string nickName;
}
/*{"success":true,"data":{"profile":{"_id":"5e79e27cc467540017b8374a","isConfirmed":true,"points":0,"dailyStreaks":0,"keys":0,"powerStones":0,"scannedObjects":[],"email":"mahmoudhypatia@gmail.com","country":"egypt","firstName":"mahmoud","lastName":"rabah","playerType":"registered"}}}*/
