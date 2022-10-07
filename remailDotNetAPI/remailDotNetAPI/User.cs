namespace remailDotNetAPI
{
    public class User
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? Username { get; set; }

        public string? Pass { get; set; } //probably hash or whatever, named pass for simplicity



        //computed się robi tak: public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        //public string? Summary { get; set; } /// ? - nullable
    }
}