using UnityEngine;
using UnityEngine.SceneManagement;

namespace ML.Systems
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        [SerializeField] private LevelType currentLevelType;

        public static GameManager Instance
        {
            get { return instance; }
        }

        public LevelType CurrentLevelType
        {
            get { return currentLevelType; }
        }

        private void Awake()
        {
            if ((instance != null) && (instance != this))
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }

}