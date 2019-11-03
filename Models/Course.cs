namespace SkipTakeRepro.Models
{
    public class Course
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public Money CurrentPrice { get; private set; }
    }
}