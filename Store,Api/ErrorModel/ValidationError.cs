namespace Store_Api.ErrorModel
{
    public class ValidationError
    {
        public string Field { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
