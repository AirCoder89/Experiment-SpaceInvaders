using UnityEngine;

namespace UI.HUD
{
    public class LivesHandler : MonoBehaviour
    {
        [SerializeField] private GameObject[] lives;

        public void UpdateLives(int inLives)
        {
            if (inLives >= lives.Length) inLives = lives.Length;
            for (var i = 0; i < lives.Length; i++)
                lives[i].SetActive(i < inLives);
        }
    }
}