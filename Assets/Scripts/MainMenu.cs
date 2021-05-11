using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.yiran.SimpleCS{
    public class MainMenu : MonoBehaviour
    {
        public Launcher launcher;

        private void Start(){
            Pause.paused = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void JoinMatch(){
            launcher.Join();
        }
        public void CreateMatch(){
            launcher.Create();

        }
        public void de_dust2(){
            SceneManager.LoadScene("dust2");
        }
        public void Quit(){
            Application.Quit();
        }
    }
}
