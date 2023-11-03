using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Security.Cryptography.Xml;
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
    public class MemberProcedureController : Controller
    {
            private IUnitOfWork _unitOfWork;
            private string fileName = "MemberProcedureController/";
            Logger logger = LogManager.GetCurrentClassLogger();

            public MemberProcedureController(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            [HttpGet]
            [Route("GetAllMemberProcedures")]
            public JsonResult GetAllMemberProcedures()
            {
                try
                {
                    logger.Debug(fileName + "GetAllMemberProcedures() start");
                    var data = _unitOfWork.MemberProcedures.GetAll().AsParallel().AsOrdered().ToList()
                    .OrderBy(b => b.Name);
                    logger.Debug(fileName + "GetAllMemberProcedures() success");
                    return Json(data);

                }
                catch (NullReferenceException ex)
                {
                    logger.Error(ex, fileName + "GetAllMemberProcedures() error");
                    throw;
                }
            }

            [HttpGet]
            [Route("GetMemberProceduresByName")]
            public JsonResult GetMemberProceduresByName(string Name)
            {
                try
                {
                    logger.Debug(fileName + "GetMemberProceduresByName() start");

                    var data = _unitOfWork.MemberProcedures.GetAll().AsParallel().AsOrdered().ToList()
                    .Where(b => b.Name != null && b.Name.Length >= Name.Length &&
                     b.Name.Substring(0, Name.Length).ToLower() == Name.ToLower()).OrderBy(b => b.Name)
                    .Select(b => new
                    {
                        id = b.Id,
                        name = b.Name,
                        returntype = b.ReturnType,
                        basetypeid = b.BaseTypeId,
                        isfunction = b.IsFunction,
                        description = b.Description,
                        signature = b.Signature
                    });

                    logger.Debug(fileName + "GetMemberProceduresByName() success");

                    return Json(data);
                }
                catch (NullReferenceException ex)
                {
                    logger.Error(ex, fileName + "GetMemberProceduresByName() error");
                    throw;
                }
            }

            [HttpGet]
            [Route("GetMemberProcedureById")]
            public JsonResult GetMemberProcedureById(int Id)
            {
                try
                {
                    logger.Debug(fileName + "GetMemberProcedureById() start");

                    var data = _unitOfWork.MemberProcedures.GetAll().AsParallel().ToList()
                    .Where(b => b.Id == Id)
                    .Select(b => new
                    {
                        id = b.Id,
                        name = b.Name,
                        returntype = b.ReturnType,
                        basetypeid = b.BaseTypeId,
                        isfunction = b.IsFunction,
                        description = b.Description,
                        signature = b.Signature
                    });

                    logger.Debug(fileName + "GetMemberProcedureById() success");

                    return Json(data);
                }
                catch (NullReferenceException ex)
                {
                    logger.Error(ex, fileName + "GetMemberProcedureById() error");
                    throw;
                }
            }

            [HttpGet]
            [Route("GetMemberProceduresByBaseTypeId")]
            public JsonResult GetMemberProceduresByBaseTypeId(int Id)
            {
                try
                {
                    logger.Debug(fileName + "GetMemberProceduresByBaseTypeId() start");

                    var data = _unitOfWork.MemberProcedures.GetAll().AsParallel().AsOrdered().ToList()
                    .Where(b => b.BaseTypeId == Id).OrderBy(b => b.Name)
                    .Select(b => new
                    {
                        id = b.Id,
                        name = b.Name,
                        returntype = b.ReturnType,
                        basetypeid = b.BaseTypeId,
                        isfunction = b.IsFunction,
                        description = b.Description,
                        signature = b.Signature
                    });

                    logger.Debug(fileName + "GetMemberProceduresByBaseTypeId() success");

                    return Json(data);
                }
                catch (NullReferenceException ex)
                {
                    logger.Error(ex, fileName + "GetMemberProceduresByBaseTypeId() error");
                    throw;
                }
            }

            [HttpPost]
            [Route("AddMemberProcedure")]
            public JsonResult AddMemberProcedure(MemberProcedure data)
            {
                try
                {
                    logger.Debug(fileName + "AddMemberProcedure() start");

                    var memberProcedure = new MemberProcedure
                    {
                        Name = data.Name,
                        ReturnType = data.ReturnType,
                        BaseTypeId = data.BaseTypeId,
                        IsFunction = data.IsFunction,
                        Description = data.Description,
                        Signature = data.Signature
                    };
                    _unitOfWork.MemberProcedures.Insert(memberProcedure);
                    _unitOfWork.Complete();
                    logger.Debug(fileName + "AddMemberProcedure() success");

                    return Json("sucsess");
                }
                catch (Exception ex)
                {
                    logger.Error(ex, fileName + "AddMemberProcedure() error");
                    throw;
                }
                finally { _unitOfWork.Complete(); }
            }

            [HttpPut]
            [Route("UpdateMemberProcedure")]
            public JsonResult UpdatePropertie(MemberProcedure data)
            {
                try
                {
                    logger.Debug(fileName + "UpdateMemberProcedure() start");

                    var memberProcedure = _unitOfWork.MemberProcedures.GetAll().AsParallel().Where(s => s.Id == data.Id).FirstOrDefault();

                    if (memberProcedure != null && data.Name != null)
                    {
                        memberProcedure.Name = data.Name;
                        memberProcedure.ReturnType = data.ReturnType;
                        memberProcedure.BaseTypeId = data.BaseTypeId;
                        memberProcedure.IsFunction = data.IsFunction;
                        memberProcedure.Description = data.Description;
                        memberProcedure.Signature = data.Signature;
                    }
                    _unitOfWork.MemberProcedures.Update(memberProcedure);
                    _unitOfWork.Complete();
                    logger.Debug(fileName + "UpdateMemberProcedure() success");

                    //Return Json for ajax
                    return Json("success");
                }
                catch (Exception ex)
                {
                    logger.Error(ex, fileName + "UpdateMemberProcedure() error");
                    throw;
                }
                finally { _unitOfWork.Complete(); }
            }

            [HttpPost]
            [Route("DeleteMemberProcedure")]
            public JsonResult DeleteMemberProcedure(int Id)
            {
                try
                {
                    logger.Debug(fileName + "DeleteMemberProcedure() start");
                    _unitOfWork.MemberProcedures.Delete(Id);
                    _unitOfWork.Complete();
                    logger.Debug(fileName + "DeleteMemberProcedure() sucsess");
                    //Return Json for ajax
                    return Json("success");
                }
                catch (Exception ex)
                {
                    logger.Error(ex, fileName + "DeleteMemberProcedure() error");
                    throw;
                }
                finally { _unitOfWork.Complete(); }
            }
        }
}
