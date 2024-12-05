namespace Bar_rating.ViewModels
{
    public class BarDetailsWithReviewViewModel
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] Image { get; set; }

      
        public int Rating { get; set; }
        public string RatingText { get; set; }
    }
}
