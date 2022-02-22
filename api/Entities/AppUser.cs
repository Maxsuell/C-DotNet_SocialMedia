namespace api.Entities
{
    public class AppUser
    {
        
        public int Id { get; set; }

        public string Users { get; set;}

        public byte[] PasswordHash {get;set;}

        public byte[] PasswordSalt { get; set; }

        
    }
}