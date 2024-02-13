namespace CloudStorageSystem.Blocks
{
    public struct BuildingStructureSendingData
    {
        public int X;
        public int Y;
        public int Z;
        public int Hp;
        public int Level;

        public BuildingStructureSendingData(int x, int y, int z, int hp, int level)
        {
            X = x;
            Y = y;
            Z = z;
            Hp = hp;
            Level = level;
        }
    }
}