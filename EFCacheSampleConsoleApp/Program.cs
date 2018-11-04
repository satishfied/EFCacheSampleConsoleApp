using EFCache;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCacheSampleConsoleApp
{
    class Program
    {

        public class Airline
        {
            [Key]
            public string Code { get; set; }

            public string Name { get; set; }

            public virtual ICollection<Aircraft> Aircraft { get; set; }
        }

        public class Aircraft
        {
            public int Id { get; set; }

            public string EquipmentCode { get; set; }

            public virtual Airline Airline { get; set; }
        }

        public class AirTravelContext : DbContext
        {
            static AirTravelContext()
            {
                Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AirTravelContext>());
            }

            public DbSet<Airline> Airlines { get; set; }

            public DbSet<Aircraft> Aircraft { get; set; }
        }

        internal static readonly InMemoryCache Cache = new InMemoryCache();

        private static void Seed()
        {
            using (var ctx = new AirTravelContext())
            {
                ctx.Airlines.Add(
                  new Airline
                  {
                      Code = "UA",
                      Name = "United Airlines",
                      Aircraft = new List<Aircraft>
                    {
            new Aircraft {EquipmentCode = "788"},
            new Aircraft {EquipmentCode = "763"}
                    }
                  });

                ctx.Airlines.Add(
                  new Airline
                  {
                      Code = "FR",
                      Name = "Ryan Air",
                      Aircraft = new List<Aircraft>
                    {
            new Aircraft {EquipmentCode = "738"},
                    }
                  });

                ctx.SaveChanges();
            }
        }

        private static void RemoveData()
        {
            using (var ctx = new AirTravelContext())
            {
                ctx.Database.ExecuteSqlCommand("DELETE FROM Aircraft");
                ctx.Database.ExecuteSqlCommand("DELETE FROM Airlines");
            }
        }

        private static void PrintAirlinesAndAircraft()
        {
            using (var ctx = new AirTravelContext())
            {
                foreach (var airline in ctx.Airlines.Include(a => a.Aircraft))
                {
                    Console.WriteLine("{0}: {1}", airline.Code,
                      string.Join(", ", airline.Aircraft.Select(a => a.EquipmentCode)));
                }
            }
        }

        private static void PrintAirlineCount()
        {
            using (var ctx = new AirTravelContext())
            {
                Console.WriteLine("Airline Count: {0}", ctx.Airlines.Count());
            }
        }

        static void Main(string[] args)
        {
            // populate and print data
            Console.WriteLine("Entries in cache: {0}", Cache.Count);
            //RemoveData();

            //Seed();
            PrintAirlinesAndAircraft();

            Console.WriteLine("\nEntries in cache: {0}", Cache.Count);
            // remove data bypassing cache
           // RemoveData();
            // not cached - goes to the database and counts airlines
            PrintAirlineCount();
            // prints results from cache
            PrintAirlinesAndAircraft();
            Console.WriteLine("\nEntries in cache: {0}", Cache.Count);
            // repopulate data - invalidates cache
           // Seed();
            Console.WriteLine("\nEntries in cache: {0}", Cache.Count);
            // print data
            PrintAirlineCount();
           // PrintAirlinesAndAircraft();
            Console.WriteLine("\nEntries in cache: {0}", Cache.Count);
        }

        
    }
}
