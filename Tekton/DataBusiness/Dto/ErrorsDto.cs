namespace BusinessLayer.Dto
{
    public class ErrorsDto
    {
        public bool IsValid { get; set; }

        public string ErrorMessage { get; set; }

        public ErrorsDto()
        {
            IsValid = true;
            ErrorMessage = string.Empty;
        }
    }
}
