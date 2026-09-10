using UnityEngine;

namespace SengkalaDev
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private GameState currentState;

        public delegate void ChangeStateDelegate(GameState state);
        public event ChangeStateDelegate OnChangeStated;
        
        private void Awake()
        {
            if (Instance == null) 
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public void ChangeState(GameState newState)
        {
            Debug.Log($"State: {currentState} -> {newState}");
            
            if (newState == currentState) return;
            currentState = newState;
            OnChangeStated?.Invoke(newState);
        }          
    }
}