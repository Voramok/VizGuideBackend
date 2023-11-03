using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using VizGuideBackend.Data;
using VizGuideBackend.Models;

namespace VizGuideBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Route("scripts/api/[controller]")]
    [Route("scripts/search/api/[controller]")]
    [Route("scripts/update/api/[controller]")]
    
    public class ScriptController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private string fileName = "ScriptController/";
        Logger logger = LogManager.GetCurrentClassLogger();

        public ScriptController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetAllScripts")]
        public JsonResult GetAllScripts()
        {
            try
            {
                logger.Debug(fileName + "GetAllScripts() start");
                var data = _unitOfWork.Scripts.GetAll().AsParallel().AsOrdered().ToList()
                .OrderBy(b => b.Name);
                logger.Debug(fileName + "GetAllScripts() success");
                return Json(data);

            }
            catch (NullReferenceException ex)
            {
                logger.Error(ex, fileName + "GetAllScripts() error");
                throw;
            }
        }

        [HttpGet]
        [Route("GetScriptsByName")]
        public JsonResult GetScriptsByName(string Name)
        {
            try
            {
                logger.Debug(fileName + "GetScriptsByName() start");

                var data = _unitOfWork.Scripts.GetAll().AsParallel().AsOrdered().ToList()
                .Where(b => b.Name != null && b.Name.Length >= Name.Length &&
                 b.Name.Substring(0, Name.Length).ToLower() == Name.ToLower()).OrderBy(b => b.Name)
                .Select(b => new
                {
                    id = b.Id,
                    name = b.Name,
                    description = b.Description,
                    code = b.Code,
                });

                logger.Debug(fileName + "GetScriptsByName() success");

                return Json(data);
            }
            catch (NullReferenceException ex)
            {
                logger.Error(ex, fileName + "GetScriptsByName() error");
                throw;
            }
        }

        [HttpGet]
        [Route("GetScriptById")]
        public JsonResult GetScriptById(int Id)
        {
            try
            {
                logger.Debug(fileName + "GetScriptById() start");

                var data = _unitOfWork.Scripts.GetAll().AsParallel().ToList()
                .Where(b => b.Id == Id)
                .Select(b => new
                {
                    id = b.Id,
                    name = b.Name,
                    description = b.Description,
                    code = b.Code,
                });

                logger.Debug(fileName + "GetScriptById() success");

                return Json(data);
            }
            catch (NullReferenceException ex)
            {
                logger.Error(ex, fileName + "GetScriptById() error");
                throw;
            }
        }


        [HttpPost]
        [Route("AddScript")]
        public JsonResult AddScript(Script data)
        {
            try
            {
                logger.Debug(fileName + "AddScript() start");

                var script = new Script
                {
                    Name = data.Name,
                    Description = data.Description,
                    Code = data.Code,
                };
                _unitOfWork.Scripts.Insert(script);
                _unitOfWork.Complete();
                logger.Debug(fileName + "AddScript() success");

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
        [Route("UpdateScript")]
        public JsonResult UpdateScript(Script data)
        {
            try
            {
                logger.Debug(fileName + "UpdateScript() start");

                var script = _unitOfWork.Scripts.GetAll().AsParallel().Where(s => s.Id == data.Id).FirstOrDefault();

                if (script != null && data.Name != null)
                {
                    script.Name = data.Name;
                    script.Description = data.Description;
                    script.Code = data.Code;
                }
                _unitOfWork.Scripts.Update(script);
                _unitOfWork.Complete();
                logger.Debug(fileName + "UpdateScript() success");

                //Return Json for ajax
                return Json("success");
            }
            catch (Exception ex)
            {
                logger.Error(ex, fileName + "UpdateScript() error");
                throw;
            }
            finally { _unitOfWork.Complete(); }
        }

        [HttpPost]
        [Route("DeleteScript")]
        public JsonResult DeleteScript(int Id)
        {
            try
            {
                logger.Debug(fileName + "DeleteScript() start");
                _unitOfWork.Scripts.Delete(Id);
                _unitOfWork.Complete();
                logger.Debug(fileName + "DeleteScript() sucsess");
                //Return Json for ajax
                return Json("success");
            }
            catch (Exception ex)
            {
                logger.Error(ex, fileName + "DeleteScript() error");
                throw;
            }
            finally { _unitOfWork.Complete(); }
        }
    }
}
