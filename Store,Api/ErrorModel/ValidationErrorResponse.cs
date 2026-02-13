namespace Store_Api.ErrorModel
{
    public class ValidationErrorResponse
    {
        public int StatuseCode { get ; set; } = StatusCodes.Status400BadRequest;
        public string ErrorMessage { get; set; } = "Validation Errors";
        public IEnumerable<ValidationError> Errors { get; set; }
    }
}
