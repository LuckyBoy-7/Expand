namespace Buildings
{
    public class TriangleBuilding : Building
    {
        protected override void Awake()
        {
            base.Awake();
            _soldierMoveSpeed = 3;
            MaxSoldiers = 20;
        }
    }
}