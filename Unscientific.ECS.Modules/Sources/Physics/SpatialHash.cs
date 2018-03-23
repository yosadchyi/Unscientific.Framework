using Unscientific.ECS.Modules.Core;
using Unscientific.ECS.Modules.Physics.Shapes;
using Unscientific.FixedPoint;
using Unscientific.Util.Pool;

namespace Unscientific.ECS.Modules.Physics
{
    public class SpatialHash: ISpatialDatabase
    {
        private class Proxy
        {
            public static readonly ObjectPool<Proxy> Pool = new GenericObjectPool<Proxy>(128);

            public Proxy Next;
            public Entity<Game> Entity;
            public Shape Shape;
        }

        public Fix CellSize;
        public int Size;

        private readonly Proxy[] _cells;

        public SpatialHash(Fix cellSize, int size)
        {
            CellSize = cellSize;
            _cells = new Proxy[size];
            Proxy.Pool.EnsureHaveInstances(size * 2);
        }

        public void Add(Entity<Game> entity, Shape shape, ref AABB aabb)
        {
            var l = (int) FixMath.Floor(aabb.L / CellSize);
            var b = (int) FixMath.Floor(aabb.B / CellSize);
            var t = (int) FixMath.Floor(aabb.T / CellSize);
            var r = (int) FixMath.Floor(aabb.R / CellSize);

            for (var i = b; i <= t; i += 1)
            {
                for (var j = l; j <= r; j += 1)
                {
                    if (Find(entity, j, i) == null)
                        AddProxyAt(entity, shape, j, i);
                }
            }
        }

        public void Clear()
        {
            for (var i = 0; i < _cells.Length; i++)
            {
                Proxy next;

                for (var tmp = _cells[i]; tmp != null; tmp = next)
                {
                    next = tmp.Next;
                    ReturnProxy(tmp);
                }
                _cells[i] = null;
            }
        }

        public void Remove(Entity<Game> entity, ref AABB aabb)
        {
            var l = (int) FixMath.Floor(aabb.L / CellSize);
            var b = (int) FixMath.Floor(aabb.B / CellSize);
            var r = (int) FixMath.Floor(aabb.R / CellSize);
            var t = (int) FixMath.Floor(aabb.T / CellSize);

            for (var i = b; i <= t; i++)
            {
                for (var j = l; j <= r; j++)
                {
                    RemoveProxyAt(entity, j, i);
                }
            }
        }

        public void Query(ref AABB aabb, SpatialDatabaseCallback callback)
        {
            var l = (int) FixMath.Floor(aabb.L / CellSize);
            var b = (int) FixMath.Floor(aabb.B / CellSize);
            var r = (int) FixMath.Floor(aabb.R / CellSize);
            var t = (int) FixMath.Floor(aabb.T / CellSize);

            for (var i = b; i <= t; i++)
            {
                for (var j = l; j <= r; j++)
                {
                    EnumerateBodies(j, i, callback);
                }
            }
        }

        private void RemoveProxyAt(Entity<Game> entity, int x, int y)
        {
            var hash = HashFunction(x, y);
            Proxy prev = null;

            for (var tmp = _cells[hash]; tmp != null; tmp = tmp.Next)
            {
                if (tmp.Entity.Id == entity.Id)
                {
                    if (prev != null)
                    {
                        prev.Next = tmp.Next;
                    }
                    else
                    {
                        _cells[hash] = null;
                    }
                    ReturnProxy(tmp);
                    return;
                }
                prev = tmp;
            }
        }

        private void EnumerateBodies(int x, int y, SpatialDatabaseCallback callback)
        {
            var hash = HashFunction(x, y);

            for (var tmp = _cells[hash]; tmp != null; tmp = tmp.Next)
            {
                callback(tmp.Entity, tmp.Shape);
            }
        }

        private Proxy Find(Entity<Game> entity, int x, int y)
        {
            var hash = HashFunction(x, y);

            for (var tmp = _cells[hash]; tmp != null; tmp = tmp.Next)
            {
                if (tmp.Entity.Id == entity.Id)
                    return tmp;
            }
            return null;
        }

        private void AddProxyAt(Entity<Game> entity, Shape shape, int x, int y)
        {
            var hash = HashFunction(x, y);
            var proxy = Proxy.Pool.Get();
            proxy.Entity = entity;
            proxy.Shape = shape;
            proxy.Next = _cells[hash];
            _cells[hash] = proxy;
        }

        private static void ReturnProxy(Proxy proxy)
        {
            proxy.Next = null;
            proxy.Shape = null;
            Proxy.Pool.Return(proxy);
        }

        private uint HashFunction(int x, int y)
        {
            var ux = (ulong) x;
            var uy = (ulong) y;
            var ulen = (ulong) _cells.Length;

            return (uint) (((ux * 73856093) ^ (uy * 83492791)) % ulen);
        }
    }
}