// <auto-generated />
#pragma warning disable CS0105
using MessagePack;
using ReactiveMemory.Benchmark.DataAccess.Models;
using ReactiveMemory.Validation;
using ReactiveMemory;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using ReactiveMemory.Benchmark.Tables;
using System.Security.Cryptography;

namespace ReactiveMemory.Benchmark
{
   public sealed class DbContext 
   {
        public bool IsTransactionStarted { get; private set; }
        public event Action OnUnauthorizedMemoryModification;
		public IMemoryDatabase  Database => _database ??= new MemoryDatabase(_data, _changesConveyor);
        public ITransaction  Transaction => _transaction;

        private MemoryDatabase _database;
        private Transaction _transaction;
        private ChangesConveyor _changesConveyor;
        private byte[] _data;
        private byte[] _hash;
        private MD5 _md5;

        public DbContext(byte[] dbBytes, IChangesMediatorFactory changesMediatorFactory, string md5 = "")
        {
            _changesConveyor = new ChangesConveyor(changesMediatorFactory);
            _data = dbBytes;
            if(!string.IsNullOrWhiteSpace(md5))
			{
				_md5 = MD5.Create(md5);
				_hash = _md5.ComputeHash(_data);
			}
        }

        public ITransaction BeginTransaction()
        {
            if (IsTransactionStarted)
            {
                throw new InvalidOperationException("Transaction is already started");
            }

            IsTransactionStarted = true;
            if (_database == null)
            {
                // serialization of db from bytes, it's slow
                _database = new MemoryDatabase(_data, _changesConveyor);
            }
            else if(_md5 != null)
            {
                // calc hash of current db data
                var prevDataHash = _md5.ComputeHash(_data); 
                if (!prevDataHash.SequenceEqual(_hash) || !prevDataHash.SequenceEqual(_md5.ComputeHash(ToBytes())))
                {
                    // detected memory modifications
                    OnUnauthorizedMemoryModification?.Invoke();
                }
            }
            // it just cast, but when we make changes it make copy of data, so Database will not be changed
            _transaction = _database.BeginTransaction();
            return _transaction;
        }


        public void Commit()
        {
            if (!IsTransactionStarted)
            {
                throw new InvalidOperationException("Transaction is not started");
            }

            // cast to  MemoryDatabase 
            _database = _transaction.Commit();
            if(_md5 != null)
			{
				_data = ToBytes();
				_hash = _md5.ComputeHash(_data);
			}
            IsTransactionStarted = false;
        }

        public void Rollback()
        {
            // all changes in Transaction, so we just set it to null to discard changes
            _database.ChangesConveyor.Clear();
            _transaction = null;
            IsTransactionStarted = false;
        }
        
        public byte[] ToBytes()
        {
            return _database.ToDatabaseBuilder().Build();
        }
   }
}