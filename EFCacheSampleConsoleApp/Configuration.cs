using EFCache;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCacheSampleConsoleApp
{
    public class Configuration:DbConfiguration
    {
       
            public Configuration()
            {
            //var transactionHandler = new CacheTransactionHandler(new InMemoryCache());

            //AddInterceptor(transactionHandler);

            //Loaded +=
            //  (sender, args) => args.ReplaceService<DbProviderServices>(
            //    (s, _) => new CachingProviderServices(s, transactionHandler,
            //      new CachingPolicy()));


            var transactionHandler = new CacheTransactionHandler(Program.Cache);

            AddInterceptor(transactionHandler);

            Loaded +=
              (sender, args) => args.ReplaceService<DbProviderServices>(
                (s, _) => new CachingProviderServices(s, transactionHandler));
        }
        
    }
}
