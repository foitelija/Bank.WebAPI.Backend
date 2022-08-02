
namespace Bank.WebAPI.Backend.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string AccountName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal CurrentAccountBalance { get; set; } 
        public AccountType AccountType { get; set; }
        public string AccountNumberGenerated { get; set; }  

        //hash and salt for transaction
        public byte[] PinHash { get; set; }
        public byte[] PinSalt { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime DateLastUpdated { get; set; } = DateTime.Now;

        //Generate AccountNumberGenerated
        public Account()
        {
            Random rnd = new Random();
            AccountNumberGenerated = Convert.ToString((long) rnd.NextDouble() * 9_000_000_000L + 1_000_000_000L);
            AccountName = $"{FirstName} {LastName}";
        }
    }



    public enum AccountType
    {
        Savings,
        Current,
        Corporate,
        Goverment
    }
}
