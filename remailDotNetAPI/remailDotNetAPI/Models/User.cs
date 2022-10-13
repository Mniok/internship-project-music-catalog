namespace remailDotNetAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        public string? UserName { get; set; }

        public string? OriginalUserName { get; set; }

        public string? Password { get; set; } //probably hash or whatever, named pass for simplicity

        public DateTime CreatedAt { get; set; }

        public string? UserRole { get; set; }



        //computed się robi tak: public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        //public string? Summary { get; set; } /// ? - nullable
    }
}