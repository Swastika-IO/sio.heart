using Swastika.Domain.Core.Interfaces;
using Swastika.Domain.Core.Models;
using Swastika.Domain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TTS.Lib.Models;
using TTS.Lib.Repositories;

namespace TTS.Lib.ViewModels.Admin
{
    public class BannerViewModel : ViewModelBase<TtsBanner, BannerViewModel>
    {
        private readonly BannerRepository _repo;        
        public string Id { get; set; }
        public string Specificulture { get; set; }
        public string Url { get; set; }
        public string Alias { get; set; }
        [Required]
        public string Image { get; set; }           
        public DateTime? CreatedDate { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
        public string ModifiedBy { get; set; }

        //View
        public string ImageURL { get; set; }

        public SupportedCulture CurrentCulture
        {
            get
            {
                return ListSupportedCulture.FirstOrDefault(c => c.Specificulture == this.Specificulture);
            }
        }
        public BannerViewModel(TtsBanner model) : base(model)
        {
            this.Model = model;
            _repo = BannerRepository.GetInstance();
            ListSupportedCulture = CultureRepository.GetInstance().GetListSupportedCultureByBannerId(model.Id);
            ParseView();
        }
        public BannerViewModel()
        {
            _repo = BannerRepository.GetInstance();
        }
        public BannerViewModel(string culture) : base()
        {
            _repo = BannerRepository.GetInstance();
            Specificulture = culture;
            ListSupportedCulture = CultureRepository.GetInstance().GetListSupportedCultureByBannerId(string.Empty);
            ListSupportedCulture.FirstOrDefault(c => c.Specificulture == Specificulture).IsSupported = true;
        }

        public override BannerViewModel ParseView()
        {
            base.ParseView();
            this.ImageURL = string.Format("{0}{1}", DomainName, Image);
            return this;
        }

        public async Task<RepositoryResponse<BannerViewModel>> SaveModelAsync()
        {
            var result = await _repo.SaveModelAsync(this.ParseModel());
            foreach (var supportedCulture in ListSupportedCulture.Where(c => c.Specificulture != Specificulture))
            {

                var banner = await _repo.GetSingleModelAsync(b => b.Id == Id && b.Specificulture == supportedCulture.Specificulture);
                if (banner == null && supportedCulture.IsSupported)
                {
                    banner = new BannerViewModel(this.Model)
                    {
                        Id = Id,
                        Specificulture = supportedCulture.Specificulture
                    };
                    var cloneResult = await _repo.SaveModelAsync(banner.ParseModel());
                }
                else if (banner != null)
                {
                    await _repo.RemoveModelAsync(b => b.Id == banner.Id && b.Specificulture == supportedCulture.Specificulture);
                }
            }
            return result;
        }
    }
}
