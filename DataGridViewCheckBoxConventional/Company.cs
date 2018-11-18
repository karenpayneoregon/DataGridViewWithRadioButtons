namespace DataGridViewCheckBoxConventional
{
    public class Company
    {
        public int id { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return $"{id}, {Name}";
        }
    }
}
