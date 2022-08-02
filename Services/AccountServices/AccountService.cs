
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
            var account = _dbContext.Accounts.Where(x => x.AccountNumberGenerated == AccountNumber).FirstOrDefault();
            if(account == null)
            {
                return null;
            }
            return account;
        }

        public Account GetById(int id)
        {
            var account = _dbContext.Accounts.Where(x => x.Id == id).FirstOrDefault();
            if(account == null)
            {
                return null;
            }
            return account;
            //return _dbContext.Accounts.FirstOrDefault(ac => ac.Id == id);
        }

        //CRUD IMPLEMENTATION
        public IEnumerable<Account> GetAllAccounts()
        {
            return _dbContext.Accounts.ToList();
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
            var accountToUpdate = _dbContext.Accounts.Where(x => x.Email == account.Email).SingleOrDefault();
            if(accountToUpdate == null)
            {
                throw new ApplicationException("Аккаунта не существует.");
            }
            //если есть, смотрим что хотим поменять
            if(!string.IsNullOrWhiteSpace(account.Email))
            {
                //обновить Email
                if(_dbContext.Accounts.Any(x=>x.Email == account.Email))
                {
                    throw new ApplicationException("Эта почта " + account.Email + " уже используется.");
                }
                accountToUpdate.Email = account.Email;
            }

            //разрешаем изменить только телефон и мыло 
            if(!string.IsNullOrWhiteSpace(account.PhoneNumber))
            {
                if(_dbContext.Accounts.Any(p=>p.PhoneNumber == account.PhoneNumber))
                {
                    throw new ApplicationException("Этот телефон уже используется");
                }
                accountToUpdate.PhoneNumber = account.PhoneNumber;
            }

            if (!string.IsNullOrWhiteSpace(Pin))
            {
                byte[] pinHash, pinSalt;
                CreatePinHash(Pin, out pinHash, out pinSalt);
                accountToUpdate.PinHash = pinHash;
                accountToUpdate.PinSalt = pinSalt;
            }

            //обновляем БД
            _dbContext.Accounts.Update(accountToUpdate);
            _dbContext.SaveChanges();
        }
        public void Delete(int id)
        {
            var account = _dbContext.Accounts.Find(id);
            if(account != null)
            {
                _dbContext.Accounts.Remove(account);
                _dbContext.SaveChanges();
            }
        }
    }
}
