using UnityEngine.UI;

public class GetChangePasswordData : GeneralPanelTest
{
    public InputField oldPassword;
    public InputField newPassword;
    public ChangePasswordData getChangePasswordData ()
    {
        ChangePasswordData c = new ChangePasswordData ();
        c.oldPassword = oldPassword.text;
        c.newPassword = newPassword.text;
        return c;
    }
}
