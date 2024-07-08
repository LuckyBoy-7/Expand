namespace Buildings
{
    public class SquareBuilding : Building
    {
        protected override void Awake()
        {
            base.Awake();
            soldierMoveSpeed = 2;
            MaxSoldiers = 20;
        }

    }
}