namespace musicCatalogDotNetAPI.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string? OriginalUserName { get; set; }

        public string Password { get; set; } //probably hash or whatever, named pass for simplicity

        public DateTime CreatedAt { get; set; }

        public string? UserRole { get; set; }



        public User(int Id, string Name, string Pass, string Role) {
            this.UserId = Id;
            this.UserName = Name;
            this.OriginalUserName = Name;
            this.Password = Pass;
            this.CreatedAt = DateTime.Now;
            this.UserRole = Role;
        }

        public User() { }       //bez tego wywalało się _context.User.whatever()


        //computed się robi tak: public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
        //public string? Summary { get; set; } /// ? - nullable



        //relacje:

        public List<MasterEmail> masterEmails { get; } = new();
    }
}