using System;
using System.Threading;

public static class Server
{
    private static int _count = 0;
    private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

    public static int GetCount()
    {
        // Разрешается всем, если никто не пишет, читатели ждут окончания записи
        _lock.EnterReadLock();
        try
        {
            // Читатели безопасно читают параллельно, не блокируя друг друга, если никто не пишет
            return _count;
        }
        finally
        {
            // Выход из потока
            _lock.ExitReadLock();
        }
    }

    public static void AddToCount(int value)
    {
        // Ждёт всех: читателей и писателей, когда они завершат работу
        _lock.EnterWriteLock();
        try
        {
            // Писатель безопасно пишет
            _count += value;
        }
        finally
        {
            // Выход из потока
            _lock.ExitWriteLock();
        }
    }
}