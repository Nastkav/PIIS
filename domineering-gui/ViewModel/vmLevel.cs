namespace domineering_gui.Model
{
    public class VmLevel
    {
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public VmLevel(int value,string name)
        {
            Id = value;
            Name = name;
        }
    }
}
