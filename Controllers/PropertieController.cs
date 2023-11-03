using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Xml.Linq;
using VizGuideBackend.Data;
using VizGuideBackend.Models;

namespace VizGuideBackend.Controllers.Home
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("basetypes/api/[controller]")]
    [Route("basetypes/search/api/[controller]")]
    [Route("basetypes/update/api/[controller]")]
    [Route("basetypes/detailed/api/[controller]")]
    public class PropertieController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private string fileName = "PropertieController/";
        Logger logger = LogManager.GetCurrentClassLogger();

        public PropertieController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAllProperties")]
        public JsonResult GetAllProperties()
        {
            try
            {
                logger.Debug(fileName + "GetAllProperties() start");
                var data = _unitOfWork.Properties.GetAll().AsParallel().AsOrdered().ToList()
                .OrderBy(b => b.Name);
                logger.Debug(fileName + "GetAllProperties() success");
                return Json(data);

            }
            catch (NullReferenceException ex)
            {
                logger.Error(ex, fileName + "GetAllProperties() error");
                throw;
            }
        }

        [HttpGet]
        [Route("GetPropertiesByName")]
        public JsonResult GetPropertiesByName(string Name)
        {
            try
            {
                logger.Debug(fileName + "GetPropertiesByName() start");

                var data = _unitOfWork.Properties.GetAll().AsParallel().AsOrdered().ToList()
                .Where(b => b.Name != null && b.Name.Length >= Name.Length &&
                 b.Name.Substring(0, Name.Length).ToLower() == Name.ToLower()).OrderBy(b => b.Name)
                .Select(b => new
                {
                    id = b.Id,
                    name = b.Name,
                    returntype = b.ReturnType,
                    basetypeid = b.BaseTypeId,
                    ireadonly = b.IsReadOnly,
                    description = b.Description
                });

                logger.Debug(fileName + "GetPropertiesByName() success");

                return Json(data);
            }
            catch (NullReferenceException ex)
            {
                logger.Error(ex, fileName + "GetPropertiesByName() error");
                throw;
            }
        }

        [HttpGet]
        [Route("GetPropertieById")]
        public JsonResult GetPropertieById(int Id)
        {
            try
            {
                logger.Debug(fileName + "GetPropertieById() start");

                var data = _unitOfWork.Properties.GetAll().AsParallel().ToList()
                .Where(b => b.Id == Id)
                .Select(b => new
                {
                    id = b.Id,
                    name = b.Name,
                    returntype = b.ReturnType,
                    basetypeid = b.BaseTypeId,
                    isreadonly = b.IsReadOnly,
                    description = b.Description
                });

                logger.Debug(fileName + "GetPropertieById() success");

                return Json(data);
            }
            catch (NullReferenceException ex)
            {
                logger.Error(ex, fileName + "GetPropertieById() error");
                throw;
            }
        }

        [HttpGet]
        [Route("GetPropertiesByBaseTypeId")]
        public JsonResult GetPropertiesByBaseTypeId(int Id)
        {
            try
            {
                logger.Debug(fileName + "GetPropertiesByBaseTypeId() start");

                var data = _unitOfWork.Properties.GetAll().AsParallel().AsOrdered().ToList()
                .Where(b => b.BaseTypeId == Id).OrderBy(b => b.Name)
                .Select(b => new
                {
                    id = b.Id,
                    name = b.Name,
                    returntype = b.ReturnType,
                    basetypeid = b.BaseTypeId,
                    isreadonly = b.IsReadOnly,
                    description = b.Description
                });

                logger.Debug(fileName + "GetPropertiesByBaseTypeId() success");

                return Json(data);
            }
            catch (NullReferenceException ex)
            {
                logger.Error(ex, fileName + "GetPropertiesByBaseTypeId() error");
                throw;
            }
        }

        [HttpPost]
        [Route("AddPropertie")]
        public JsonResult AddPropertie(Propertie data)
        {
            try
            {
                logger.Debug(fileName + "AddPropertie() start");

                    var propertie = new Propertie
                    {
                        Name = data.Name,
                        ReturnType = data.ReturnType,
                        BaseTypeId = data.BaseTypeId,
                        IsReadOnly = data.IsReadOnly,
                        Description = data.Description
                    };
                    _unitOfWork.Properties.Insert(propertie);
                    _unitOfWork.Complete();
                    logger.Debug(fileName + "AddPropertie() success");

                    //Return Json for ajax
                    return Json("success");
            }
            catch (Exception ex)
            {
                logger.Error(ex, fileName + "AddPropertie() error");
                throw;
            }
            finally { _unitOfWork.Complete(); }
        }

        [HttpPut]
        [Route("UpdatePropertie")]
        public JsonResult UpdatePropertie(Propertie data)
        {
            try
            {
                logger.Debug(fileName + "UpdatePropertie() start");

                    var propertie = _unitOfWork.Properties.GetAll().AsParallel().Where(s => s.Id == data.Id).FirstOrDefault(); 

                    if (propertie != null && data.Name != null)
                    {
                        propertie.Name = data.Name;
                        propertie.ReturnType = data.ReturnType;
                        propertie.BaseTypeId = data.BaseTypeId;
                        propertie.IsReadOnly = data.IsReadOnly;
                        propertie.Description = data.Description;
                    }
                    _unitOfWork.Properties.Update(propertie);
                    _unitOfWork.Complete();
                    logger.Debug(fileName + "UpdatePropertie() success");
           
                //Return Json for ajax
                return Json("success");
            }
            catch (Exception ex)
            {
                logger.Error(ex, fileName + "UpdatePropertie() error");
                throw;
            }
            finally { _unitOfWork.Complete(); }
        }

        [HttpPost]
        [Route("DeletePropertie")]
        public JsonResult DeletePropertie(int Id)
        {
            try
            {
                logger.Debug(fileName + "DeletePropertie() start");
                _unitOfWork.Properties.Delete(Id);
                _unitOfWork.Complete();
                logger.Debug(fileName + "DeletePropertie() sucsess");
                //Return Json for ajax
                return Json("success");
            }
            catch (Exception ex)
            {
                logger.Error(ex, fileName + "DeletePropertie() error");
                throw;
            }
            finally { _unitOfWork.Complete(); }
        }
    }
}
