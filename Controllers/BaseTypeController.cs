using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Security.AccessControl;
using System.Xml.Linq;
using VizGuideBackend.Data;
using VizGuideBackend.Models;

namespace VizGuideBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("basetypes/api/[controller]")]
    [Route("basetypes/search/api/[controller]")]
    [Route("basetypes/update/api/[controller]")]
    [Route("basetypes/detailed/api/[controller]")]
    public class BaseTypeController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private string fileName = "BaseTypeController/";
        Logger logger = LogManager.GetCurrentClassLogger();
        private bool unique = false;

        public BaseTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAllBaseTypes")]
        public JsonResult GetAllBaseTypes()
        {
            try
            {
                logger.Debug(fileName + "GetAllBaseTypes() start");
                var data = _unitOfWork.BaseTypes.GetAll().AsParallel().AsOrdered().ToList()
                .OrderBy(b => b.Name);
                logger.Debug(fileName + "GetAllBaseTypes() success");
                return Json(data);

            }
            catch (NullReferenceException ex)
            {
                logger.Error(ex, fileName + "GetAllBaseTypes() error");
                throw;
            }
        }

        [HttpGet]
        [Route("GetBaseTypesByName")]
        public JsonResult GetBaseTypesByName(string Name)
        {
            try
            {
                logger.Debug(fileName + "GetBaseTypesByName() start");

                var data = _unitOfWork.BaseTypes.GetAll().AsParallel().ToList()
                .Where(b => b.Name != null && b.Name.Length >= Name.Length &&
                 b.Name.Substring(0, Name.Length).ToLower() == Name.ToLower())
                .Select(b => new
                {
                    id = b.Id,
                    name = b.Name,
                    description = b.Description
                });

                logger.Debug(fileName + "GetBaseTypesByName() success");

                return Json(data);
            }
            catch (NullReferenceException ex)
            {
                logger.Error(ex, fileName + "GetBaseTypesByName() error");
                throw;
            }
        }

        [HttpGet]
        [Route("GetBaseTypeById")]
        public JsonResult GetBaseTypeById(int Id)
        {
            try
            {
                logger.Debug(fileName + "GetBaseTypeById() start");

                var data = _unitOfWork.BaseTypes.GetAll().AsParallel().ToList()
                .Where(b => b.Id == Id)
                .Select(b => new
                {
                    id = b.Id,
                    name = b.Name,
                    description = b.Description
                });

                logger.Debug(fileName + "GetBaseTypeById() success");

                return Json(data);
            }
            catch (NullReferenceException ex)
            {
                logger.Error(ex, fileName + "GetBaseTypeById() error");
                throw;
            }
        }

        [HttpPost]
        [Route("AddBaseType")]
        public JsonResult AddBaseType(BaseType data)
        {
            try
            {
                logger.Debug(fileName + "AddBaseType() start");

                //Check for unique BaseType name
                var dataPrev = _unitOfWork.BaseTypes.GetAll().AsParallel().ToList()
                .Where(s => s.Name == data.Name);

                if (!dataPrev.Any())
                {
                    var baseType = new BaseType
                    {
                        Name = data.Name,
                        Description = data.Description
                    };
                    _unitOfWork.BaseTypes.Insert(baseType);
                    _unitOfWork.Complete();
                    unique = true;
                    logger.Debug(fileName + "AddBaseType() success");
                } else {
                    unique = false;
                    logger.Debug(fileName + "AddBaseType() failed");
                }
                
                //Return Json for ajax
                return Json(unique);
            }
            catch (Exception ex)
            {
                logger.Error(ex, fileName + "AddBaseType() error");
                throw;
            }
            finally { _unitOfWork.Complete(); }
        }

        [HttpPut]
        [Route("UpdateBaseType")]
        public JsonResult UpdateBaseType(BaseType data)
        {
            try
            {
                logger.Debug(fileName + "UpdateBaseType() start");

                var dataPrev = _unitOfWork.BaseTypes.GetAll().AsParallel().ToList()
                .Where(s => s.Name == data.Name && s.Id != data.Id);

                if (!dataPrev.Any())
                {
                    //var baseType = new BaseType();
                    var baseType = _unitOfWork.BaseTypes.GetAll().AsParallel().Where(s => s.Id == data.Id).FirstOrDefault(); ;

                    if (baseType != null && data.Name != null)
                    {
                        baseType.Name = data.Name;
                        baseType.Description = data.Description;
                    }
                    _unitOfWork.BaseTypes.Update(baseType);
                    _unitOfWork.Complete();
                    unique = true;
                    logger.Debug(fileName + "UpdateBaseType() success");
                }
                else
                {
                    unique = false;
                    logger.Debug(fileName + "UpdateBaseType() failed");
                }
                //Return Json for ajax
                return Json(unique);
            }
            catch (Exception ex)
            {
                logger.Error(ex, fileName + "UpdateBaseType() error");
                throw;
            }
            finally { _unitOfWork.Complete(); }
        }

        [HttpPost]
        [Route("DeleteBaseType")]
        public JsonResult DeleteBaseType(int Id)
        {
            try
            {
                logger.Debug(fileName + "DeleteBaseType() start");
                _unitOfWork.BaseTypes.Delete(Id);
                _unitOfWork.Complete();
                logger.Debug(fileName + "DeleteBaseType() sucsess");
                //Return Json for ajax
                return Json("success");
            }
            catch (Exception ex)
            {
                logger.Error(ex, fileName + "DeleteBaseType() error");
                throw;
            }
            finally { _unitOfWork.Complete(); }
        }
    }
}
