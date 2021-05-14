using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

namespace com.yiran.SimpleCS{
    [System.Serializable]
    public class mapData{
        public string name;
        public int scene;
    }


    public class Launcher : MonoBehaviourPunCallbacks
    {
        public GameObject tabMain;
        public GameObject tabRoom;
        public GameObject tabCreate;
        public GameObject buttonRoom;
        public List<RoomInfo> roomList;

        public InputField roomnameField;

        public Text mapValue;
        public mapData[] maps;
        private int currentMap;

        public void Awake(){
            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();
        }
        public override void OnConnectedToMaster(){
            Debug.Log("Connected");

            PhotonNetwork.JoinLobby();
            base.OnConnectedToMaster();
        }
        public override void OnJoinedRoom(){
            StartGame();
            base.OnJoinedRoom();
        }

        public override void OnJoinRandomFailed(short returnCode, string message){
            Create();
            base.OnJoinRandomFailed(returnCode, message);
        }
        public void Connect(){
            Debug.Log("Connecting...");
            PhotonNetwork.GameVersion = "0.0.0";
            PhotonNetwork.ConnectUsingSettings();
        }
        public void Join(){
            PhotonNetwork.JoinRandomRoom();
        }
        public void Create(){
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 8;

            options.CustomRoomPropertiesForLobby = new string[]{"map"};

            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add("map", currentMap);
            options.CustomRoomProperties = properties;

            PhotonNetwork.CreateRoom(roomnameField.text, options);
        }
        public void ChangeMap(){
            currentMap++;
            if (currentMap >= maps.Length) currentMap = 0;
            mapValue.text = "MAP: " + maps[currentMap].name;

        }
        public void TabCloseAll(){
            tabMain.SetActive(false);
            tabRoom.SetActive(false);
            tabCreate.SetActive(false);
        }
        public void TabOpenMain(){
            TabCloseAll();
            tabMain.SetActive(true);
        }
        public void TabOpenRoom(){
            TabCloseAll();
            tabRoom.SetActive(true);
        }
        public void TabOpenCreate(){
            TabCloseAll();
            tabCreate.SetActive(true);

            roomnameField.text = "";
            currentMap = 0;
            mapValue.text = "MAP: " + maps[currentMap].name;

        }
        private void ClearRoomList(){
            Transform content = tabRoom.transform.Find("Scroll View/Viewport/Content");
            foreach (Transform trans in content) Destroy(trans.gameObject);
        }
        public override void OnRoomListUpdate(List<RoomInfo> t_List){
            roomList = t_List;
            ClearRoomList();

            Debug.Log("Loaded Room @" + Time.time);
            Transform content = tabRoom.transform.Find("Scroll View/Viewport/Content");

            foreach (RoomInfo a in roomList){
                GameObject newRoomButton = Instantiate(buttonRoom, content) as GameObject;

                newRoomButton.transform.Find("Name").GetComponent<Text>().text = a.Name;
                newRoomButton.transform.Find("Player").GetComponent<Text>().text = a.PlayerCount + " / "+ a.MaxPlayers;


                newRoomButton.GetComponent<Button>().onClick.AddListener(delegate { JoinRoom(newRoomButton.transform); });
            }
            base.OnRoomListUpdate(roomList);
        }
        public void JoinRoom(Transform t_button){
            string t_roomName = t_button.transform.Find("Name").GetComponent<Text>().text;
            PhotonNetwork.JoinRoom(t_roomName);
        }
        public void StartGame(){
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1){
                PhotonNetwork.LoadLevel(maps[currentMap].scene);
            }

        }
    }
}