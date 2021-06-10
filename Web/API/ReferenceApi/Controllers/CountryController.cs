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
    public class CountryController : ControllerBase
    {
        IRefCountryService _refCountry;

        public CountryController(IRefCountryService refCountry)
        {
            _refCountry = refCountry;
        }



        /// <summary>
        /// Давлатлар рўйхати 							
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        [HttpGet("api/RefCountry/PageGetAll")]
        public async Task<ResponseCoreData> PageGetAll([FromQuery] int skip = 0, [FromQuery] int take = 10, [FromQuery] string searchText = "")
        {
            try
            {



                //   var eventList = _refCountry.Paging(skip, take);
                // return new ResponseCoreData(new { data = eventList.Results, total = eventList.RowCount }, ResponseStatusCode.OK);

                var eventList = _refCountry.FindAll();
                if (!string.IsNullOrEmpty(searchText))
                {
                    eventList = eventList.Where(w => w.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase));
                }
                return new ResponseCoreData(new { data = eventList.Skip(skip * take).Take(take), total = eventList.Count() }, ResponseStatusCode.OK);


            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        /// <summary>
        /// Давлатлар рўйхати 	By id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("api/RefCountry/GetById/{id}")]
        public async Task<ResponseCoreData> GetById(int id)
        {
            var currentDate = DateTime.Now;
            try
            {
                var model = _refCountry.Get(id);


                return new ResponseCoreData(model, ResponseStatusCode.OK);
            }
            catch (Exception ex)
            {

                return ex;
            }
        }
        /// <summary>
        /// Давлатлар рўйхати Добавить 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("api/RefCountry/Add")]
        public async Task<ResponseCoreData> Add([FromBody] RefCountryViewsModel model)
        {
            var currentDate = DateTime.Now;
            try
            {
                var refatr = new RefCountry
                {
                    Name = model.Name,
                    Code = model.Code,
                    DisplayOrder = model.DisplayOrder
                };
                _refCountry.Add(refatr);
                return true;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
        /// <summary>
        /// Давлатлар рўйхати Изменить 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("api/RefCountry/Update")]
        public async Task<ResponseCoreData> Update([FromBody] RefCountryViewsModel model)
        {
            try
            {
                var eventModel = _refCountry.Get(model.Id.Value);
                eventModel.Name = model.Name;
                eventModel.Code = model.Code;
                eventModel.DisplayOrder = model.DisplayOrder;
                _refCountry.Update(eventModel);
                return true;
            }
            catch (Exception ex)
            {
                return ex;
            }

        }
        /// <summary>
        /// Давлатлар рўйхати Удалить  
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("api/RefCountry/Delete/{id}")]
        public async Task<ResponseCoreData> Delete(int id)
        {
            try
            {
                _refCountry.Delete(id);
                return true;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

    }
}