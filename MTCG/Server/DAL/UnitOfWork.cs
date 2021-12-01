using MTCG.DAL.Context;
using MTCG.DAL.Exceptions;
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

        public GenericRepository<Card> _cardRepository;
        public GenericRepository<Card> CardRepository {
            get {
                if (_cardRepository == null)
                    _cardRepository = new(_context);
                return _cardRepository;
            }
        }

        public GenericRepository<TradeOffer> _tradeOfferRepository;
        public GenericRepository<TradeOffer> TradeOfferRepository {
            get {
                if (_tradeOfferRepository == null)
                    _tradeOfferRepository = new(_context);
                return _tradeOfferRepository;
            }
        }

        public GenericRepository<Package> _packageRepository;
        public GenericRepository<Package> PackageRepository {
            get {
                if (_packageRepository == null)
                    _packageRepository = new(_context);
                return _packageRepository;
            }
        }

        public UnitOfWork() {
            try {
                s_semaphore.WaitOne();
                _context = new MTCGContext();
            }finally {
                s_semaphore.Release();
            }
        }

        public void Save() {
            try {
                s_semaphore.WaitOne();
                _context.SaveChanges();
            } finally {
                s_semaphore.Release();
            }
        }

        public bool TrySave() {
            try {
                Save();
            } catch (Exception e) {
                Console.WriteLine(e);
                return false;
            }
            return true;
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
