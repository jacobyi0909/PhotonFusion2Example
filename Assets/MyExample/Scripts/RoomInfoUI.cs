using Fusion;
using TMPro;
using UnityEngine;

public class RoomInfoUI : MonoBehaviour
{
    public TextMeshProUGUI textSessionName;

    SessionInfo sessionInfo;
    public void Init(SessionInfo sessionInfo)
    {
        this.sessionInfo = sessionInfo;
        textSessionName.SetText(sessionInfo.Name);
    }

    public void OnClickJoin()
    {

    }

    void Start()
    {
        
    }

   
    void Update()
    {
        
    }
}
