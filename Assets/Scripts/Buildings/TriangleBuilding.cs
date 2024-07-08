namespace Buildings
{
    public class TriangleBuilding : Building
    {
        protected override void Awake()
        {
            base.Awake();
            soldierMoveSpeed = 3;
        }
    }
}