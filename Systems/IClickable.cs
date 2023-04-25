using ML.Player;

namespace ML.Systems
{
    public interface IClickable
    {
        public bool InteractWithRaycast(PlayerController playerController);

        public CursorType GetCursorType();

    }
}