using Microsoft.EntityFrameworkCore;
using Swastika.Domain.Core.Models;
using Swastika.Infrastructure.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using TTS.Lib.Models;
using TTS.Lib.ViewModels;

namespace TTS.Lib.Repositories
{
    public class CultureRepository : RepositoryBase<TtsCulture, CultureViewModel, ttsContext>
    {

        /// <summary>
        /// The instance
        /// </summary>
        private static volatile CultureRepository instance;

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static object syncRoot = new Object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static CultureRepository GetInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new CultureRepository();
                }
            }
            return instance;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="CultureRepository"/> class from being created.
        /// </summary>
        private CultureRepository() : base()
        {
        }
        public List<SupportedCulture> GetListSupportedCultureByBannerId(string bannerId)
        {
            using (ttsContext context = new ttsContext())
            {
                var query = context.TtsCulture.Include(c => c.TtsBanner)//.Where(c=>c.TtsBanner.FirstOrDefault(b=>b.Id==bannerId)!=null)
                    .Select(c => new SupportedCulture()
                    {
                        Icon = c.Icon,
                        Specificulture = c.Specificulture,
                        IsSupported = c.TtsBanner.FirstOrDefault(b => b.Id == bannerId) != null

                    });
                return query.ToList();
            }
        }
    }
}
