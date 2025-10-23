using UnityEngine;
using UnityEngine.SceneManagement;

namespace IGDC
{
    public class ReadyToPlay : MonoBehaviour
    {

        public void ReadyToPlayGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}
