namespace Features.Obstacles
{
    public class VanishWall : Operated
    {
        public override void Action()
        {
            Destroy(gameObject);
        }
    }
}