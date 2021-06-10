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
   // [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        IRefDistrictService _refDistrict;

        public DistrictController(IRefDistrictService refDistrict)
        {
            _refDistrict = refDistrict;
        }



        /// <summary>
        /// Туманлар рўйхати						
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet("api/RefDistrict/PageGetAll")]
        public async Task<ResponseCoreData> PageGetAll([FromQuery] int skip = 0, [FromQuery] int take = 10, [FromQuery] int regionId = default)
        {
            try
            {
                if (regionId == default)
                {
                    var result = _refDistrict.FindAll();
                    return new ResponseCoreData(new { data = result.Skip(skip).Take(take), total = result.Count() }, ResponseStatusCode.OK);
                }
                else
                {
                    var result = _refDistrict.Find(f => f.RegionId == regionId);
                    return new ResponseCoreData(new { data = result.Skip(skip).Take(take), total = result.Count() }, ResponseStatusCode.OK);
                }


            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        /// <summary>
        /// Туманлар рўйхати 	By id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("api/RefDistrict/GetById/{id}")]
        public async Task<ResponseCoreData> GetById(int id)
        {
            var currentDate = DateTime.Now;
            try
            {
                var model = _refDistrict.Get(id);


                return new ResponseCoreData(model, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {

                return ex;
            }
        }
        /// <summary>
        /// Туманлар рўйхати	 Добавить 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("api/RefDistrict/Add")]
        public async Task<ResponseCoreData> Add([FromBody] RefDistrictViewsModel model)
        {
            var currentDate = DateTime.Now;
            try
            {
                var refatr = new RefDistrict
                {
                    Name = model.Name,
                    ShortName = model.ShortName,
                    DisplayOrder = model.DisplayOrder,
                    RegionId = model.RegionId
                };
                _refDistrict.Add(refatr);
                return true;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        /// <summary>
        /// Туманлар рўйхати	 Изменить 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("api/RefDistrict/Update")]
        public async Task<ResponseCoreData> Update([FromBody] RefDistrictViewsModel model)
        {
            try
            {
                var eventModel = _refDistrict.Get(model.Id.Value);
                eventModel.Name = model.Name;
                eventModel.ShortName = model.ShortName;
                eventModel.DisplayOrder = model.DisplayOrder;
                eventModel.RegionId = model.RegionId;
                _refDistrict.Update(eventModel);
                return true;
            }
            catch (Exception ex)
            {
                return ex;
            }

        }
        /// <summary>
        /// Туманлар рўйхати 	 Удалить  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("api/RefDistrict/Delete/{id}")]
        public async Task<ResponseCoreData> Delete(int id)
        {
            try
            {
                _refDistrict.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

    }
}