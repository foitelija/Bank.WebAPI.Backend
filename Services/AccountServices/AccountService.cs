namespace Bank.WebAPI.Backend.Services
{
    public class AccountService : IAccountService
    {
        public Account Authenticate(string AccountNumber, string Pin)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            throw new NotImplementedException();
        }

        public Account GetByAccountNumber(string AccountNumber)
        {
            throw new NotImplementedException();
        }

        public Account GetById(int id)
        {
            throw new NotImplementedException();
        }


        public Account Create(Account account, string Pin, string ConfirmPin)
        {
            throw new NotImplementedException();
        }
        public void Update(Account account, string Pin = null)
        {
            throw new NotImplementedException();
        }
        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
