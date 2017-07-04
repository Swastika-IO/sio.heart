using Swastika.Domain.Core.Interfaces;
using Swastika.Domain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using TTS.Lib.Models;
using TTS.Lib.Repositories;

namespace TTS.Lib.ViewModels
{
    /// <summary>
    /// Ref: https://www.codeproject.com/Tips/595061/SQL-Server-culture-mapping-with-NET-Framework
    /// </summary>
    public class CultureViewModel: ViewModelBase<TtsCulture, CultureViewModel>
    {
        private readonly CultureRepository _repo;
        public int Id { get; set; }
        public string Specificulture { get; set; }
        public string Lcid { get; set; }
        public string Alias { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }        

        public CultureViewModel(TtsCulture model):base(model)
        {
            this.Model = model;
            _repo = CultureRepository.GetInstance();
            ParseView();
        }
        public CultureViewModel():base()
        {
            _repo = CultureRepository.GetInstance();
        }
        public override CultureViewModel ParseView()
        {
            base.ParseView();            
            return this;
        }

        public async Task<RepositoryResponse<CultureViewModel>> SaveModeAsync()
        {
            return await _repo.SaveModelAsync(this.ParseModel());
        }
    }
}
