// <auto-generated />
#pragma warning disable CS0105
using MessagePack;
using ReactiveMemory.Tests;
using ReactiveMemory.Validation;
using ReactiveMemory;
using System.Collections.Generic;
using System;
using ReactiveMemory.Test2.Tables;

namespace ReactiveMemory.Test2
{
   public sealed class DbContext 
   {
		public IMemoryDatabase  Database => _database;
        private MemoryDatabase _database;

        public ITransaction  Transaction => _transaction;
        private Transaction _transaction;

        public bool IsTransactionStarted { get; private set; }

        public DbContext(byte[] dbBytes, IChangesMediatorFactory changesMediatorFactory)
        {
            _database = new  MemoryDatabase (dbBytes, changesMediatorFactory);
        }

        public void BeginTransaction()
        {
            if (IsTransactionStarted)
            {
                throw new InvalidOperationException("Transaction is already started");
            }

            IsTransactionStarted = true;

            // it just cast, but when we make changes it make copy of data, so Database will not be changed
            _transaction = _database.BeginTransaction();
        }


        public void Commit()
        {
            if (!IsTransactionStarted)
            {
                throw new InvalidOperationException("Transaction is not started");
            }

            // cast to  MemoryDatabase 
            _database = _transaction.Commit();

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