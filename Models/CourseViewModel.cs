namespace SkipTakeRepro.Models
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Money CurrentPrice { get; set; }
        public static CourseViewModel FromEntity(Course course)
        {
            return new CourseViewModel {
                Id = course.Id,
                Title = course.Title,
                CurrentPrice = course.CurrentPrice
            };
        }
    }
}