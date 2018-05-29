using System;
using SuivA.Data.Entity;

namespace SuivA.Data.Context
{
    public class GlobalAppContext : IDisposable
    {
        private bool _disposed;

        public GlobalAppContext()
        {
            DataContext = new DataContext();
        }

        public DataContext DataContext { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed || !disposing)
                return;

            DataContext?.Dispose();

            _disposed = true;
        }

        protected static class Check
        {
            public static void Require(string value)
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (value.Trim().Length == 0)
                    throw new ArgumentException();
            }

            public static void Require(int value)
            {
                if (value == 0)
                    throw new ArgumentException();
            }

            public static void ValidDate(DateTime date)
            {
                if (date > DateTime.Now)
                    throw new ArgumentOutOfRangeException();
                if (date.Year < DateTime.Now.AddYears(-1).Year)
                    throw new ArgumentOutOfRangeException();
            }

            public static void ValidTimeSuperior(TimeSpan inferiorTime, TimeSpan? supposedSuperiorTime)
            {
                if (inferiorTime > supposedSuperiorTime)
                    throw new ArgumentOutOfRangeException();
            }

            public static void ValidTimeSuperior(TimeSpan? inferiorTime, TimeSpan? supposedSuperiorTime)
            {
                if (inferiorTime > supposedSuperiorTime)
                    throw new ArgumentOutOfRangeException();
            }

            public static void ValidDoctor(Doctor doctor, User user)
            {
                //if (user == null) throw new ArgumentNullException(nameof(user));
                if (!user.Doctors.Contains(doctor))
                    throw new ArgumentException();
            }

//            public static void ValidVisitor(User user)
//            {
//                // if (user == null) throw new ArgumentNullException(nameof(user));
//                if (user?.Role.Name != "visiteur")
//                    throw new ArgumentException();
//            }
        }
    }
}