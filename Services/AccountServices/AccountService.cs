
namespace Bank.WebAPI.Backend.Services
{
    public class AccountService : IAccountService
    {
        private DataContext _dbContext; // для работы с БД
        public AccountService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Account Authenticate(string AccountNumber, string Pin)
        {
            //аутентификация кста
            var account = _dbContext.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).SingleOrDefault();
            if(account == null)
            {
                return null;
            }

            if(!VerifyPinHash(Pin, account.PinHash, account.PinSalt))
            {
                return null;
            }
            //Аутентификация прошла
            return account;
        }

        private static bool VerifyPinHash(string Pin, byte[] pinHash, byte[] pinSalt)
        {
            if(string.IsNullOrEmpty(Pin))
            {
                throw new ArgumentNullException("Пин");
            }
            //верефикация пина
            using(var hmac = new System.Security.Cryptography.HMACSHA512(pinSalt))
            {
                var computedPinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Pin));
                for(int i =0; i<computedPinHash.Length; i++)
                {
                    if (computedPinHash[i] != pinHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public Account GetByAccountNumber(string AccountNumber)
        {
            throw new NotImplementedException();
        }

        public Account GetById(int id)
        {
            throw new NotImplementedException();
        }

        //CRUD IMPLEMENTATION
        public IEnumerable<Account> GetAllAccounts()
        {
            throw new NotImplementedException();
        }
        public Account Create(Account account, string Pin, string ConfirmPin)
        {
            if(_dbContext.Accounts.Any(x=>x.Email == account.Email))
            {
                throw new ApplicationException("Аккаунт с таким Email уже сущетсвует.");
            }
            if(!Pin.Equals(ConfirmPin))
            {
                throw new ApplicationException("PIN и ConfrimPin не совпадают!");
            }
            //хэш для пина
            byte[] pinHash, pinSalt;
            CreatePinHash(Pin, out pinHash, out pinSalt); // метод для шифра

            account.PinHash = pinHash;
            account.PinSalt = pinSalt;

            _dbContext.Accounts.Add(account);
            _dbContext.SaveChanges();

            return account;
        }

        private static void CreatePinHash(string pin, out byte[] pinHash, out byte[] pinSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                pinSalt = hmac.Key;
                pinHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(pin));
            }
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
