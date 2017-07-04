using Swastika.Infrastructure.Data.Repository;
using System;
using TTS.Lib.Models;
using TTS.Lib.ViewModels.Admin;

namespace TTS.Lib.Repositories
{
    public class BannerRepository : RepositoryBase<TtsBanner, BannerViewModel, ttsContext>
    {

        /// <summary>
        /// The instance
        /// </summary>
        private static volatile BannerRepository instance;

        /// <summary>
        /// The synchronize root
        /// </summary>
        private static object syncRoot = new Object();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns></returns>
        public static BannerRepository GetInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                        instance = new BannerRepository();
                }
            }
            return instance;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="BannerRepository"/> class from being created.
        /// </summary>
        private BannerRepository() : base() {
        }

    }
}
