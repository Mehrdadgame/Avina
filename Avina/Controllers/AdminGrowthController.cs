using Avina.Models;
using Avina.Services;
using Microsoft.AspNetCore.Mvc;

namespace Avina.Controllers;

[ApiController]
[Route("api/admin/growth")]
public class AdminGrowthController(IGrowthEngineService growthEngineService) : ControllerBase
{
    [HttpGet("paths")]
    public async Task<ActionResult<List<GrowthPath>>> GetPaths(CancellationToken cancellationToken)
        => Ok(await growthEngineService.GetPathsAsync(cancellationToken));

    [HttpPost("paths")]
    public async Task<ActionResult<GrowthPath>> CreatePath([FromBody] GrowthPathUpsertRequest request, CancellationToken cancellationToken)
    {
        var path = await growthEngineService.CreatePathAsync(request, cancellationToken);
        return Ok(path);
    }

    [HttpPut("paths/{id:int}")]
    public async Task<ActionResult<GrowthPath>> UpdatePath(int id, [FromBody] GrowthPathUpsertRequest request, CancellationToken cancellationToken)
    {
        var path = await growthEngineService.UpdatePathAsync(id, request, cancellationToken);
        return path is null ? NotFound() : Ok(path);
    }

    [HttpGet("skills")]
    public async Task<ActionResult<List<Skill>>> GetSkills([FromQuery] int? pathId, CancellationToken cancellationToken)
        => Ok(await growthEngineService.GetSkillsAsync(pathId, cancellationToken));

    [HttpPost("skills")]
    public async Task<ActionResult<Skill>> CreateSkill([FromBody] SkillUpsertRequest request, CancellationToken cancellationToken)
    {
        var skill = await growthEngineService.CreateSkillAsync(request, cancellationToken);
        return Ok(skill);
    }

    [HttpPut("skills/{id:int}")]
    public async Task<ActionResult<Skill>> UpdateSkill(int id, [FromBody] SkillUpsertRequest request, CancellationToken cancellationToken)
    {
        var skill = await growthEngineService.UpdateSkillAsync(id, request, cancellationToken);
        return skill is null ? NotFound() : Ok(skill);
    }

    [HttpGet("missions")]
    public async Task<ActionResult<List<Mission>>> GetMissions([FromQuery] int? pathId, [FromQuery] int? skillId, CancellationToken cancellationToken)
        => Ok(await growthEngineService.GetMissionsAsync(pathId, skillId, cancellationToken));

    [HttpPost("missions")]
    public async Task<ActionResult<Mission>> CreateMission([FromBody] MissionUpsertRequest request, CancellationToken cancellationToken)
    {
        var mission = await growthEngineService.CreateMissionAsync(request, cancellationToken);
        return Ok(mission);
    }

    [HttpPut("missions/{id:int}")]
    public async Task<ActionResult<Mission>> UpdateMission(int id, [FromBody] MissionUpsertRequest request, CancellationToken cancellationToken)
    {
        var mission = await growthEngineService.UpdateMissionAsync(id, request, cancellationToken);
        return mission is null ? NotFound() : Ok(mission);
    }

    [HttpGet("submissions/pending")]
    public async Task<ActionResult<List<MissionSubmission>>> GetPendingSubmissions(CancellationToken cancellationToken)
        => Ok(await growthEngineService.GetPendingSubmissionsAsync(cancellationToken));

    [HttpPost("submissions/{id:int}/review")]
    public async Task<ActionResult<MissionSubmission>> ReviewSubmission(
        int id,
        [FromBody] ReviewMissionSubmissionRequest request,
        CancellationToken cancellationToken)
    {
        var reviewed = await growthEngineService.ReviewSubmissionAsync(id, request.Approve, request.AdminFeedback, cancellationToken);
        return reviewed is null ? NotFound() : Ok(reviewed);
    }
}

