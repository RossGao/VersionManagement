using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VersionManagement.BusinessLogics;
using VersionManagement.Dtos;
using VersionManagement.Models;

namespace VersionManagement.Controllers
{
    // TODO: 增加上传图片功能
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
        /// <param name="department">产品部门. 0:所有;1:泛耘;2:泛员网;3:易社保;4:昊天;5:泛优</param>
        /// <param name="status">0:所有, 1:已审核, 2:未审核</param>
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
        /// 新增或更新版本信息, 新增时不用填写Id的值。
        /// </summary>
        /// <param name="version">一个版本实例</param>
        /// <returns>新增或更新的版本</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateVersion([FromBody] VersionInfo version)
        {
            if (!ModelState.IsValid || version.Department == Department.All)
            {
                return BadRequest();
            }

            if (version != null)
            {
                return Ok(await logicHandler.UpdateVersionAsync(version));
            }
            else
            {
                return NotFound();
            }
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
                logicHandler.DeleteVersion(id);
                return Ok();
            }

            return BadRequest();
        }

        /// <summary>
        /// 提交版本. 提交后版本进入发布流程，版本信息不可再修改
        /// </summary>
        /// <param name="id">版本ID</param>
        /// <param name="releaseNote">版本描述</param>
        /// <returns>提交的版本</returns>
        [HttpPut]
        public IActionResult SubmitVersion(Guid id, string releaseNote)
        {
            if (id != Guid.Empty)
            {
                var version = logicHandler.SubmitVersion(id, releaseNote);

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
            if (pageIndex > 0 && pageSize > 0)
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
        /// <param name="detailId">详情ID</param>
        /// <returns>版本详情</returns>
        [Route("detail")]
        [HttpGet]
        public IActionResult GetVersionDetailById(Guid detailId)
        {
            if (detailId != Guid.Empty)
            {
                var detail = logicHandler.GetVersionDetailById(detailId);

                if (detail != null)
                {
                    return Ok(detail);
                }

                return NotFound();
            }

            return BadRequest();
        }

        /// <summary>
        /// 新增或更新一个版本详情信息, 新增时不用填写Id的值。
        /// </summary>
        /// <param name="detail">详情内容</param>
        /// <returns>新增或更新的版本详情</returns>
        [Route("detail")]
        [HttpPost]
        public IActionResult UpdateVersionDetail([FromBody] VersionDetailDto detail)
        {
            if (!ModelState.IsValid || detail.Type == TaskType.All)
            {
                return BadRequest();
            }

            var detailItem = logicHandler.UpdateVersionDetail(detail);

            if (detailItem == null)
            {
                return NotFound();
            }

            return Ok(DtoTransfer.ConvertToDetailDto(detailItem));
        }

        /// <summary>
        /// 根据详情ID删除某条版本详情
        /// </summary>
        /// <param name="detailId">详情ID</param>
        /// <returns>被删除的版本详情</returns>
        [Route("detail")]
        [HttpDelete]
        public IActionResult DeleteVersionDetail(Guid detailId)
        {
            if (detailId != Guid.Empty)
            {
                logicHandler.DeleteVersionDetail(detailId);
                return Ok();
            }

            return BadRequest();
        }
    }
}
