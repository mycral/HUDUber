using System.Collections.Generic;


public static class ObjectPool<T> where T : new()
{
    private static readonly Queue<T> pool;

    static ObjectPool()
    {
        pool = new Queue<T>();
    }

    public static T GetObject()
    {
        if (pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else
        {
            return new T();
        }
    }

    public static void ReturnObject(T obj)
    {
        pool.Enqueue(obj);
    }
}

