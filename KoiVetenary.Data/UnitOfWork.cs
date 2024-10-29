using KoiVetenary.Data.Models;
using KoiVetenary.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KoiVetenary.Data
{
    public class UnitOfWork
    {
        private FA24_SE1716_PRN231_G3_KoiVetenaryContext _unitOfWorkContext;
        private AnimalRepository _animal;
        private ServiceRepository _service;
        private CategoryRepository _category;
        private VeterinarianRepository _veterinarian;
        private AppointmentRepository _appointment;
        private AppointmentDetailRepository _appointmentDetail;
        private OwnerRepository _owner;
        private MedicalRecordRepository _medicalRecord;
        private AnimalTypeRepository _animalType;
        private PaymentRepository _payment;

        public UnitOfWork()
        {
            _unitOfWorkContext = new FA24_SE1716_PRN231_G3_KoiVetenaryContext();
        }
        public AnimalRepository AnimalRepository
        {
            get { return _animal ??= new AnimalRepository(); }
        }

        public ServiceRepository ServiceRepository
        {
            get { return _service ??= new ServiceRepository(); }
        }

        public CategoryRepository CategoryRepository
        {
            get { return _category ??= new CategoryRepository(); }
        }

        public VeterinarianRepository VeterinarianRepository
        {
            get { return _veterinarian ??= new VeterinarianRepository(_unitOfWorkContext); }
        }

        public AppointmentRepository AppointmentRepository
        {
            get { return _appointment ??= new AppointmentRepository(); }
        }  
        public PaymentRepository PaymentRepository
        {
            get { return _payment ??= new PaymentRepository(); }
        }

        public AppointmentDetailRepository AppointmentDetailRepository
        {
            get { return _appointmentDetail ??= new AppointmentDetailRepository(); }
        }

        public OwnerRepository OwnerRepository
        {
            get { return _owner ??= new OwnerRepository(); }
        }

        public MedicalRecordRepository MedicalRecordRepository
        {
            get { return _medicalRecord ??= new MedicalRecordRepository(); }
        }
        public AnimalTypeRepository AnimalTypeRepository
        {
            get { return _animalType ??= new AnimalTypeRepository(); }
        }

        ////TO-DO CODE HERE/////////////////

        #region Set transaction isolation levels

        /*
        Read Uncommitted: The lowest level of isolation, allows transactions to read uncommitted data from other transactions. This can lead to dirty reads and other issues.

        Read Committed: Transactions can only read data that has been committed by other transactions. This level avoids dirty reads but can still experience other isolation problems.

        Repeatable Read: Transactions can only read data that was committed before their execution, and all reads are repeatable. This prevents dirty reads and non-repeatable reads, but may still experience phantom reads.

        Serializable: The highest level of isolation, ensuring that transactions are completely isolated from one another. This can lead to increased lock contention, potentially hurting performance.

        Snapshot: This isolation level uses row versioning to avoid locks, providing consistency without impeding concurrency. 
         */

        public int SaveChangesWithTransaction()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _unitOfWorkContext.Database.BeginTransaction())
            {
                try
                {
                    result = _unitOfWorkContext.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _unitOfWorkContext.Database.BeginTransaction())
            {
                try
                {
                    result = await _unitOfWorkContext.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }



        #endregion
    }
}