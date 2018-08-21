using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VersionManagement.BusinessLogics;
using VersionManagement.Dtos;
using VersionManagement.Models;

namespace VersionManagement.Controllers
{
    [Route("version")]
    public class VersionController : Controller
    {
        private IVersionLogic logicHandler;

        public VersionController(IVersionLogic logic)
        {
            logicHandler = logic;
        }

        /// <summary>
        /// 获取版本列表
        /// </summary>
        /// <param name="department">0:All, 1:BPO, 2:FYU, 3:ESB, 4:HaoTian, 5:FanYou, 6:SDB</param>
        /// <param name="status">0:Undefined, 1:未审核, 2:已审核</param>
        /// <param name="pageNumber">页面号码</param>
        /// <param name="pageSize">一页列表长度</param>
        /// <returns>版本列表</returns>
        [Route("list")]
        [HttpGet]
        public IActionResult GetVersions(Department department, VersionStatus status, int pageNumber = 1, int pageSize = 20)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            long totalNumber = 0;
            var versions = logicHandler.GetPagedVersionList(department, status, out totalNumber, pageNumber, pageSize);

            if (versions != null && versions.Count > 0)
            {
                return Ok(DtoTransfer.ConvertToVersionDto(versions, totalNumber));
            }

            return NotFound();
        }

        /// <summary>
        /// 根据版本ID获取版本信息
        /// </summary>
        /// <param name="id">版本 Id</param>
        /// <returns>一个版本信息</returns>
        [HttpGet]
        public IActionResult GetVersionById(Guid id)
        {
            if (id != Guid.Empty)
            {
                return Ok(logicHandler.GetversionById(id));
            }

            return NotFound();
        }

        /// <summary>
        /// 新增或更新版本信息
        /// </summary>
        /// <param name="version">一个版本实例</param>
        /// <returns>新增或更新的版本</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateVersion(VersionInfo version)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (version != null)
            {
                return Ok(await logicHandler.UpdateVersionAsync(version));
            }

            return BadRequest();
        }

        /// <summary>
        /// 根据版本ID删除版本
        /// </summary>
        /// <param name="id">版本ID</param>
        /// <returns>删除的版本</returns>
        [HttpDelete]
        public IActionResult DeleteVersion(Guid id)
        {
            if (id != Guid.Empty)
            {
                return Ok(logicHandler.DeleteVersion(id));
            }

            return BadRequest();
        }

        /// <summary>
        /// 提交版本. 提交后版本进入发布流程，版本信息不可再修改
        /// </summary>
        /// <param name="id">版本ID</param>
        /// <returns>提交的版本</returns>
        [HttpPut]
        public IActionResult SubmitVersion(Guid id)
        {
            if (id != Guid.Empty)
            {
                var version = logicHandler.SubmitVersion(id);

                if (version != null)
                {
                    return Ok(version);
                }

                return NotFound();
            }

            return BadRequest();
        }

        /// <summary>
        /// 获取版本详情列表
        /// </summary>
        /// <param name="versionId">版本 ID</param>
        /// <returns>版本详情列表</returns>
        /// <param name="pageIndex">查询页码</param>
        /// <param name="pageSize">列表大小</param>
        /// <param name="applicant">申请人名字</param>
        [Route("details")]
        [HttpGet]
        public IActionResult GetVersionDetails(Guid versionId, int pageIndex = 1, int pageSize = 20, string applicant = "")
        {
            if (versionId != Guid.Empty && pageIndex > 0 && pageSize > 0)
            {
                long totalCount;
                var detailList = logicHandler.GetVersionDetails(versionId, out totalCount, pageIndex, pageSize, applicant);
                return Ok(DtoTransfer.ConvertToDetailsDto(detailList, totalCount));
            }

            return BadRequest();
        }

        /// <summary>
        /// 获取一个版本详情
        /// </summary>
        /// <param name="versionId">版本ID</param>
        /// <param name="detailId">详情ID</param>
        /// <returns>版本详情</returns>
        [Route("detail")]
        [HttpGet]
        public IActionResult GetVersionDetailById(Guid versionId, Guid detailId)
        {
            if (versionId != Guid.Empty && detailId != Guid.Empty)
            {
                var detail = logicHandler.GetVersionDetailById(versionId, detailId);

                if (detail != null)
                {
                    return Ok(detailId);
                }

                return NotFound();
            }

            return BadRequest();
        }

        /// <summary>
        /// 新增或更新一个版本详情信息
        /// </summary>
        /// <param name="versionId">版本 ID</param>
        /// <param name="detail">详情内容</param>
        /// <returns>新增或更新的版本详情</returns>
        [Route("detail")]
        [HttpPost]
        public IActionResult UpdateVersionDetail(Guid versionId, VersionDetail detail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var detailItem = logicHandler.UpdateVersionDetail(versionId, detail);

            if (detailItem == null)
            {
                return NotFound();
            }

            return Ok(detailItem);
        }

        /// <summary>
        /// 根据详情ID删除某条版本详情
        /// </summary>
        /// <param name="versionId">版本ID</param>
        /// <param name="detailId">详情ID</param>
        /// <returns>被删除的版本详情</returns>
        [Route("detail")]
        [HttpDelete]
        public IActionResult DeleteVersionDetail(Guid versionId, Guid detailId)
        {
            if (versionId != Guid.Empty && detailId != Guid.Empty)
            {
                return Ok(logicHandler.DeleteVersionDetail(versionId, detailId));
            }

            return BadRequest();
        }
    }
}
