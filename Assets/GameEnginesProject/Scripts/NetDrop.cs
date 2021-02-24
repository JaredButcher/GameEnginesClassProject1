/*Attached to the player and incharge of the NetDrops
*
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NetDrop : MonoBehaviour
{
    public GameObject drop;
    public string address;
    public int drops = 1;

    int usedDrops = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Send request to server to see if any drops are avaliable
        StartCoroutine(Request());
        //Notify that this exists
        FindObjectOfType<NotificationHUDManager>().CreateNotification("Press F to drop a NetDrop here for another player");
    }

    void Update() {
		//If the NetDrop button (f) is pressed, send a netdrop
        if (Input.GetButtonDown("NetDrop")) {
            if(drops > usedDrops) {
                SendNetDrop();
                ++usedDrops;
            }
        }
    }

    IEnumerator Request() {
        //Asyncrinously send request to server to see if any drops are avaliable for the level
        List<IMultipartFormSection> postReq = new List<IMultipartFormSection>();
        postReq.Add(new MultipartFormDataSection($"level={SceneManager.GetActiveScene().name}"));
        UnityWebRequest netDropReq = UnityWebRequest.Post($"http://{address}:3493/level", postReq);
        yield return netDropReq.SendWebRequest();

		//
        if(netDropReq.isNetworkError || netDropReq.isHttpError) {
            Debug.LogWarning(netDropReq.error);
        } else {
            if(netDropReq.downloadHandler.text.Length == 1) {
                //Create notifcation for player that no drop was avaliable
				FindObjectOfType<NotificationHUDManager>().CreateNotification("No netdrop avaliable");
            } else {
                //If a drop is avialble, instantiate it
                Debug.Log("Netdrop avaliable");
                string[] cords = netDropReq.downloadHandler.text.Split(',');
                Instantiate(drop, new Vector3(float.Parse(cords[0]), float.Parse(cords[1]), float.Parse(cords[2])), Quaternion.identity);
				//Create notifcation for player
                FindObjectOfType<NotificationHUDManager>().CreateNotification("A blue NetDrop has been received");
            }
        }
    }

    void SendNetDrop() {
        //Send a drop for the current location to the server
        Vector3 pos = FindObjectOfType<PlayerCharacterController>().transform.position;
        StartCoroutine(Send(pos));
    }

    IEnumerator Send(Vector3 pos) {
        //Send the drop asyncrinously to the server
        List<IMultipartFormSection> postReq = new List<IMultipartFormSection>();
        postReq.Add(new MultipartFormDataSection($"level={SceneManager.GetActiveScene().name}&cords={pos.x},{pos.y},{pos.z}"));
        UnityWebRequest netDropReq = UnityWebRequest.Post($"http://{address}:3493/drop", postReq);
        yield return netDropReq.SendWebRequest();

        if (netDropReq.isNetworkError || netDropReq.isHttpError) {
            Debug.LogWarning(netDropReq.error);
        } else {
			//Notify the player that it was sucessful
            FindObjectOfType<NotificationHUDManager>().CreateNotification("You have sent a NetDrop");
        }
    }
}
