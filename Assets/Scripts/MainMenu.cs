using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
   public void PlayGame()
   {
       // buildindex +1 can be edited under "File"-> Build settings and you will see the settings there. 
       SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
   }
   public void Easy()
   {
       SceneManager.LoadScene("Easy");
   }
   public void Medium()
   {
       SceneManager.LoadScene("Medium");
   }
   public void Hard()
   {
       SceneManager.LoadScene("Hard");
   }

   public void QuitGame()
   {
       Application.Quit();
   }
   public void MainMenuOption()
   {
        SceneManager.LoadScene("MainMenu");
   }
}
