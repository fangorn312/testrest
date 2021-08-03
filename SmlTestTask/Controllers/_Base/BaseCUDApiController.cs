using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BLL.Interface;
using BLL.Interface.Exception;
//using BLL.Interface.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PL.API.Open.Controllers.Base
{
    [ApiController]
    [Route("[controller]")]
    public class BaseCUDApiController<Dto, KeyType> : ControllerBase
        where Dto : IBaseDto, IEntityWithId<KeyType>, new()
    {
        protected readonly string CONTROLLER_NAME = typeof(Dto).Name;

        private IComplexProvider _db = null;
        protected IComplexProvider db
        {
            get
            {
                if (_db == null)
                    throw new NullReferenceException();
                return _db;
            }
            set { _db = value; }
        }

        protected void UseService(Type dtoType)
        {
            _db.UseOneService(dtoType);
        }

        public BaseCUDApiController(IComplexProvider provider)
        {
            db = provider;
        }


        /// <summary>
        /// Add new record
        /// </summary>
        /// <returns>Added record</returns>
        /// <response code="400">Invalid model</response>
        /// <response code="409">Duplicate item</response>
        [HttpPost]
        public virtual object Post([FromBody] Dto item)
        {
            try
            {
                BeforeAddOrUpdate(item);
                var result = db.Set<Dto>().Add(item);
                AfterAddOrUpdateSource(item);
                AfterAddOrUpdateResult(result);
                return result;
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(e.Message);
            }
            catch (NonUniqueException e)
            {
                return StatusCode((int)HttpStatusCode.Conflict, e.Message);
            }
            catch (CustomValidationException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Update existing record
        /// </summary>
        /// <returns>Updated record</returns>
        /// <response code="404">Not found</response>
        /// <response code="400">Invalid model</response>
        [HttpPut]
        public virtual object Put([FromBody] Dto item)
        {
            try
            {
                BeforeAddOrUpdate(item);
                var result = db.Set<Dto>().Update(item);
                AfterAddOrUpdateSource(item);
                AfterAddOrUpdateResult(result);
                return result;
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
            catch (NonUniqueException e)
            {
                return StatusCode((int)HttpStatusCode.Conflict, e.Message);
            }
            catch (CustomValidationException e)
            {
                return BadRequest(e.Message);
            }
        }


        protected virtual void BeforeAddOrUpdate(Dto item) { }
        protected virtual void AfterAddOrUpdateSource(Dto item) { }
        protected virtual void AfterAddOrUpdateResult(Dto item) { }


        /// <summary>
        /// Delete record by id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="404">Not found</response>
        [HttpDelete("{id}")]
        public virtual object Delete(KeyType id)
        {
            try
            {
                db.Set<Dto, KeyType>().RemoveById(id);
                return Ok();
            }
            catch (NullReferenceException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
