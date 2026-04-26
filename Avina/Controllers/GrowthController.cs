using Avina.Models;
using Avina.Services;
using Microsoft.AspNetCore.Mvc;

namespace Avina.Controllers;

[ApiController]
[Route("api")]
public class GrowthController(IGrowthEngineService growthEngineService) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<ActionResult<DashboardDto>> GetDashboard([FromQuery] int userId, CancellationToken cancellationToken)
    {
        if (userId <= 0)
        {
            return BadRequest(new { message = "شناسه کاربر الزامی است." });
        }

        var result = await growthEngineService.GetDashboardAsync(userId, cancellationToken);
        return Ok(result);
    }

    [HttpGet("missions/today")]
    public async Task<ActionResult<TodayMissionDto>> GetTodayMission([FromQuery] int userId, CancellationToken cancellationToken)
    {
        if (userId <= 0)
        {
            return BadRequest(new { message = "شناسه کاربر الزامی است." });
        }

        var mission = await growthEngineService.GetTodayMissionAsync(userId, cancellationToken);
        return mission is null ? NotFound(new { message = "ماموریتی پیدا نشد. ابتدا آزمون اولیه را کامل کنید." }) : Ok(mission);
    }

    [HttpPost("missions/{id:int}/submit")]
    public async Task<ActionResult<MissionSubmitResultDto>> SubmitMission(
        int id,
        [FromBody] MissionSubmitRequest request,
        CancellationToken cancellationToken)
    {
        if (request.UserId <= 0)
        {
            return BadRequest(new { message = "شناسه کاربر الزامی است." });
        }

        var result = await growthEngineService.SubmitMissionAsync(request.UserId, id, request.TextAnswer, request.MediaUrl, cancellationToken);
        return Ok(result);
    }

    [HttpGet("onboarding/questions")]
    public async Task<ActionResult<List<OnboardingQuestionDto>>> GetOnboardingQuestions(CancellationToken cancellationToken)
    {
        var questions = await growthEngineService.GetOnboardingQuestionsAsync(cancellationToken);
        return Ok(questions);
    }

    [HttpPost("onboarding/submit")]
    public async Task<ActionResult<OnboardingResultDto>> SubmitOnboarding([FromBody] OnboardingSubmitRequest request, CancellationToken cancellationToken)
    {
        if (request.UserId <= 0)
        {
            return BadRequest(new { message = "شناسه کاربر الزامی است." });
        }

        var result = await growthEngineService.SubmitOnboardingAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("growth/paths/select")]
    public async Task<IActionResult> SelectPath([FromBody] UserPathSelectionRequest request, CancellationToken cancellationToken)
    {
        if (request.UserId <= 0 || request.PathId <= 0)
        {
            return BadRequest(new { message = "شناسه کاربر و شناسه مسیر الزامی است." });
        }

        await growthEngineService.SetUserPathAsync(request, cancellationToken);
        return Ok(new { message = "انتخاب مسیر با موفقیت به‌روزرسانی شد." });
    }
}
