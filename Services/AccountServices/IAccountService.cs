namespace Bank.WebAPI.Backend.Services.AccountServices
{
    public interface IAccountService
    {
        Account Authenticate(string AccountNumber, string Pin);
        IEnumerable<Account> GetAllAccounts();
        Account Create(Account account, string Pin, string ConfirmPin);
        void Update(Account account, string Pin = null);
        void Delete(int id);
        Account GetById(int id);
        Account GetByAccountNumber(string AccountNumber);
    }
}
