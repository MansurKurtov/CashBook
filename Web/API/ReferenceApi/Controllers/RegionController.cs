using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Entitys.Models.Reference;
using Entitys.ViewModels.Reference;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReferenceService.Interfaces;

namespace ReferenceApi.Controllers
{
  //  [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        IRefRegionService _refRegion;

        public RegionController(IRefRegionService refRegion)
        {
            _refRegion = refRegion;
        }



        /// <summary>
        /// Вилоятлар–					 					
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet("api/RefRegion/PageGetAll")]
        public async Task<ResponseCoreData> PageGetAll([FromQuery] int skip = 0, [FromQuery] int take = 10, [FromQuery] string searchText = "")
        {
            try
            {
                var eventList = _refRegion.FindAll().Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.ShortName,
                    CountryName = x.RefCountryModel.Name,
                    x.CountryId
                }).ToList();
                if (!string.IsNullOrEmpty(searchText))
                {
                    eventList = eventList.Where(w => w.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                return new ResponseCoreData(new { data = eventList.Skip(skip).Take(take), total = eventList.Count() }, ResponseStatusCode.OK);

            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        /// <summary>
        /// Вилоятлар– By id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("api/RefRegion/GetById/{id}")]
        public async Task<ResponseCoreData> GetById(int id)
        {
            var currentDate = DateTime.Now;
            try
            {
                var model = _refRegion.Get(id);


                return new ResponseCoreData(model, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {

                return ex;
            }
        }
        /// <summary>
        /// Вилоятлар– Добавить 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("api/RefRegion/Add")]
        public async Task<ResponseCoreData> Add([FromBody] RefRegionViewsModel model)
        {
            var currentDate = DateTime.Now;
            try
            {
                var refatr = new RefRegion
                {
                    Name = model.Name,
                    ShortName = model.ShortName,
                    DisplayOrder = model.DisplayOrder,
                    CountryId = model.CountryId
                };
                _refRegion.Add(refatr);
                return true;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        /// <summary>
        /// Вилоятлар–  Изменить 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("api/RefRegion/Update")]
        public async Task<ResponseCoreData> Update([FromBody] RefRegionViewsModel model)
        {
            try
            {
                var eventModel = _refRegion.Get(model.Id.Value);
                eventModel.Name = model.Name;
                eventModel.ShortName = model.ShortName;
                eventModel.DisplayOrder = model.DisplayOrder;
                eventModel.CountryId = model.CountryId;
                _refRegion.Update(eventModel);
                return true;
            }
            catch (Exception ex)
            {
                return ex;
            }

        }
        /// <summary>
        /// Вилоятлар– Удалить  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("api/RefRegion/Delete/{id}")]
        public async Task<ResponseCoreData> Delete(int id)
        {
            try
            {
                _refRegion.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

    }
}