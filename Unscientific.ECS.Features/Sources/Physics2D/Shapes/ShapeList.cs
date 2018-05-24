using System;
using Unscientific.Util.Pool;

namespace Unscientific.ECS.Features.Physics2D.Shapes
{
    public class ShapeList
    {
        public static readonly GenericObjectPool<ShapeList> Pool = new GenericObjectPool<ShapeList>(16);

        public bool Empty => _head == _tail;
        public Shape First => _head;

        private Shape _head;
        private Shape _tail;

        public static ShapeList New()
        {
            var instance = Pool.Get();

            return instance;
        }

        public ShapeList AddCircle(Action<CircleShape> initialize)
        {
            return Add(CircleShape.New(initialize));
        }

        public ShapeList AddAABB(Action<AABBShape> initialize)
        {
            return Add(AABBShape.New(initialize));
        }

        public ShapeList Add(Shape shape)
        {
            if (_head == null)
            {
                _head = shape;
                _tail = shape;
            }
            else
            {
                _tail.Next = shape;
                _tail = shape;
            }

            return this;
        }
        
        public ShapeList Remove(Shape shape)
        {
            if (Empty) return this;

            if (_head == shape)
            {
                _head = _head.Next;
                if (_head == null) _tail = null;
                return this;
            }

            var prev = _head;

            for (var curr = _head.Next; curr != null; curr = curr.Next)
            {
                if (curr == shape)
                {
                    if (curr == _tail) _tail = prev;
                    prev.Next = curr.Next;
                    shape.ReturnToPool();
                    return this;
                }

                prev = curr;
            }

            throw new ArgumentException("Item not found!");
        }

        public void Clear()
        {
            var tmp = _head;

            while (tmp != null)
            {
                var next = tmp.Next;

                tmp.ReturnToPool();
                tmp = next;
            }
        }
    }
}