namespace Buildings
{
    public class SquareBuilding : Building
    {
        protected override void Awake()
        {
            base.Awake();
            _soldierMoveSpeed = 2;
            MaxSoldiers = 20;
        }

    }
}