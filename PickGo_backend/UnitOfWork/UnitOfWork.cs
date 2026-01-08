using PickGo_backend.Context;
using PickGo_backend.Models;
using PickGo_backend.Repositories;
using System.Threading.Tasks;

namespace PickGo_backend
{
    public class UnitOfWork
    {
        private readonly DelieveryAppContext _db;

        // Repositories
        private UserRepositories _userRepo;
        private PackageRepository _packageRepo;
        private RequestRepository _requestRepo;
        private InvoiceRepository _invoiceRepo;
        private DeliveryProofRepository _deliveryProofRepo;
        private CourierLocationRepository _courierLocationRepo;
        private SupplierRepository _supplierRepo;
        private CourierRepository _courierRepo;
        private CourierTransactionRepository _courierTransactionRepo;
        private CustomerRepository _customerRepo;
        private RoleRepository _roleRepo;
        private DisputeRepository _disputeRepo;
        private AssignmentObservationRepository _assignmentObservationRepo;

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

        public SupplierRepository SupplierRepo
        {
            get
            {
                if (_supplierRepo == null)
                    _supplierRepo = new SupplierRepository(_db);
                return _supplierRepo;
            }
        }

        public RoleRepository RoleRepo
        {
            get
            {
                if (_roleRepo == null)
                    _roleRepo = new RoleRepository(_db);
                return _roleRepo;
            }
        }

        public PackageRepository PackageRepo
        {
            get
            {
                if (_packageRepo == null)
                    _packageRepo = new PackageRepository(_db);
                return _packageRepo;
            }
        }

        public RequestRepository RequestRepo
        {
            get
            {
                if (_requestRepo == null)
                    _requestRepo = new RequestRepository(_db);
                return _requestRepo;
            }
        }

        public InvoiceRepository InvoiceRepo
        {
            get
            {
                if (_invoiceRepo == null)
                    _invoiceRepo = new InvoiceRepository(_db);
                return _invoiceRepo;
            }
        }



        public DisputeRepository DisputeRepo
        {
            get
            {
                if (_disputeRepo == null)
                    _disputeRepo = new DisputeRepository(_db);
                return _disputeRepo;
            }
        }

        public DeliveryProofRepository DeliveryProofRepo
        {
            get
            {
                if (_deliveryProofRepo == null)
                    _deliveryProofRepo = new DeliveryProofRepository(_db);
                return _deliveryProofRepo;
            }
        }

        public CourierLocationRepository CourierLocationRepo
        {
            get
            {
                if (_courierLocationRepo == null)
                    _courierLocationRepo = new CourierLocationRepository(_db);
                return _courierLocationRepo;
            }
        }

        public CourierRepository CourierRepo
        {
            get
            {
                if (_courierRepo == null)
                    _courierRepo = new CourierRepository(_db);
                return _courierRepo;
            }
        }

        public CourierTransactionRepository CourierTransactionRepo
        {
            get
            {
                if (_courierTransactionRepo == null)
                    _courierTransactionRepo = new CourierTransactionRepository(_db);
                return _courierTransactionRepo;
            }
        }

        public CustomerRepository CustomerRepo
        {
            get
            {
                if (_customerRepo == null)
                    _customerRepo = new CustomerRepository(_db);
                return _customerRepo;
            }
        }

        public AssignmentObservationRepository AssignmentObservationRepo
        {
            get
            {
                if (_assignmentObservationRepo == null)
                    _assignmentObservationRepo = new AssignmentObservationRepository(_db);
                return _assignmentObservationRepo;
            }
        }

        // Save changes
        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
