using System;
using System.Diagnostics;

namespace Unscientific.ECS.Benchmark
{
    public class Simulation : IScope
    {
        
    }
    
    public struct Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X + b.Y, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3 operator *(Vector3 a, float v)
        {
            return new Vector3(a.X * v, a.Y * v, a.Z * v);
        }
    }

    /// <summary>
    /// Sample position component data
    /// </summary>
    public struct Position
    {
        public Vector3 Location;
        public float Rotation;

        public Position(Vector3 location, float rotation)
        {
            Location = location;
            Rotation = rotation;
        }
    }

    /// <summary>
    /// Sample position component data
    /// </summary>
    public struct Velocity
    {
        public Vector3 Linear;
        public float Angular;

        public Velocity(Vector3 linear, float angular)
        {
            Linear = linear;
            Angular = angular;
        }
    }

    public class MoveSystem : IUpdateSystem
    {
        private Context<Simulation> _context;

        public MoveSystem(Context<Simulation> context)
        {
            _context = context;
        }

        public void Update()
        {
            foreach (var entity in _context.AllWith<Position, Velocity>())
            {
                var position = entity.Get<Position>();
                var velocity = entity.Get<Velocity>();
                const float dt = 1.0f / 60.0f;

                entity.Replace(new Position(position.Location + velocity.Linear * dt,
                    position.Rotation + velocity.Angular * dt));
            }
        }
    }

    public class Benchmark
    {
        private const int EntitiesCount = 1000;
        private const int RepeatCount = 10000;

        public static void Main(string[] args)
        {
            // register scopes & components
            new ComponentRegistrations()
                .For<Simulation>()
                    .Add<Position>()
                    .Add<Velocity>()
                .End()
                .Register();

            var context = new Context<Simulation>.Initializer()
                .WithInitialCapacity(EntitiesCount)
                .Initialize();

            var systems = new Systems.Builder()
                .Add(new MoveSystem(context))
                .Build();

            for (var i = 0; i < EntitiesCount; i++)
            {
                context.CreateEntity()
                    .Add(new Position(new Vector3(1, 2, 0), 0))
                    .Add(new Velocity(new Vector3(2, 2, 1), 0.01f));
            }

            // Warm up
            systems.Update();

            Measure("Entity", () =>
            {
                for (var i = 0; i < RepeatCount; i++)
                    systems.Update();
            });
        }

        private static void Measure(string name, Action action)
        {
            var watch = Stopwatch.StartNew();

            action();

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine("[{0}] Avg time: {1}", name, (float) elapsedMs / RepeatCount);
        }
    }}