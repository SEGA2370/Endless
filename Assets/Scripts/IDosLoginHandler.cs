using UnityEngine;
using IDosGames;
using System.Threading.Tasks;
using Org.BouncyCastle.Tls;

public class IDosLoginHandler : MonoBehaviour
{
    public async void OnLoginButtonClicked()
    {
        AuthService.Instance.LoginWithDeviceID(
            resultCallback: (result) =>
            {
                Debug.Log("Login successful! User ID: " + AuthService.UserID);
            },
            errorCallback: (error) =>
            {
                Debug.LogError("Login failed: " + error);
            }
        );
    }
}
