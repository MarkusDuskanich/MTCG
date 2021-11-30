using MTCG.DAL.Context;
using MTCG.DAL.Repository;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTCG.DAL {
    public class UnitOfWork : IDisposable {
        private readonly MTCGContext _context;

        private static readonly Semaphore s_semaphore = new(1, 1);

        public GenericRepository<User> _userRepository;
        public GenericRepository<User> UserRepository {
            get {
                if (_userRepository == null)
                    _userRepository = new(_context);
                return _userRepository;
            } 
        }


        public UnitOfWork() {
            try {
                s_semaphore.WaitOne();
                _context = new MTCGContext();
            } finally {
                s_semaphore.Release();
            }
        }

        /// <summary>
        /// </summary>
        /// <exception cref="Exceptions.StaleObjectStateException"></exception>
        public void Save() {
            try {
                s_semaphore.WaitOne();
                _context.SaveChanges();
            } finally {
                s_semaphore.Release();
            }
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing)
                    _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
