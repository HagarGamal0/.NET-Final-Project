using PickGo_backend.Context;
using PickGo_backend.Models;
using PickGo_backend.Repositories;
using PickGo_backend.Repositries;

namespace PickGo_backend.UnitOfWork
{
    public class UnitOfWork
    {
        private readonly DelieveryAppContext _db;

        private UserRepositories _userRepo;
        private PackageRepositories _packageRepo;
        private RequestRepositories _requestRepo;
        private InvoiceRepositories _invoiceRepo;

        public UnitOfWork(DelieveryAppContext db)
        {
            _db = db;
        }

        public UserRepositories UserRepo
        {
            get
            {
                if (_userRepo == null)
                    _userRepo = new UserRepositories(_db);

                return _userRepo;
            }
        }

        public PackageRepositories PackageRepo
        {
            get
            {
                if (_packageRepo == null)
                    _packageRepo = new PackageRepositories(_db);

                return _packageRepo;
            }
        }

        public RequestRepositories RequestRepo
        {
            get
            {
                if (_requestRepo == null)
                    _requestRepo = new RequestRepositories(_db);

                return _requestRepo;
            }
        }

        public InvoiceRepositories InvoiceRepo
        {
            get
            {
                if (_invoiceRepo == null)
                    _invoiceRepo = new InvoiceRepositories(_db);

                return _invoiceRepo;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
