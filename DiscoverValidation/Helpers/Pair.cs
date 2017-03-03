namespace DiscoverValidation.Helpers
{
    internal class Pair<TLeft, TRigth>
    {
        public TLeft Left { get; set; }
        public TRigth Rigth { get; set; }

        public Pair(TLeft left, TRigth rigth)
        {
            Left = left;
            Rigth = rigth;
        }
    }
}