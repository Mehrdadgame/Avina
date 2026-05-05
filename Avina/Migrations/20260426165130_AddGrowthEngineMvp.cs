using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Avina.Migrations
{
    /// <inheritdoc />
    public partial class AddGrowthEngineMvp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvatarUrl",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "GrowthPaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PathType = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrowthPaths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GrowthProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    AnalyticalScore = table.Column<int>(type: "int", nullable: false),
                    CreativityScore = table.Column<int>(type: "int", nullable: false),
                    SocialScore = table.Column<int>(type: "int", nullable: false),
                    DisciplineScore = table.Column<int>(type: "int", nullable: false),
                    ResilienceScore = table.Column<int>(type: "int", nullable: false),
                    FocusScore = table.Column<int>(type: "int", nullable: false),
                    ConfidenceScore = table.Column<int>(type: "int", nullable: false),
                    ResponsibilityScore = table.Column<int>(type: "int", nullable: false),
                    CuriosityScore = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrowthProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrowthProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnboardingQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RewardTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SourceType = table.Column<int>(type: "int", nullable: false),
                    SourceId = table.Column<int>(type: "int", nullable: false),
                    XPAmount = table.Column<int>(type: "int", nullable: false),
                    CoinAmount = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RewardTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnboardingAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RecommendedPathId = table.Column<int>(type: "int", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnboardingAttempts_GrowthPaths_RecommendedPathId",
                        column: x => x.RecommendedPathId,
                        principalTable: "GrowthPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OnboardingAttempts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PathId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    IsCoreSkill = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_GrowthPaths_PathId",
                        column: x => x.PathId,
                        principalTable: "GrowthPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGrowthPaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GrowthPathId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGrowthPaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGrowthPaths_GrowthPaths_GrowthPathId",
                        column: x => x.GrowthPathId,
                        principalTable: "GrowthPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGrowthPaths_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnboardingOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    OptionKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnalyticalScoreDelta = table.Column<int>(type: "int", nullable: false),
                    CreativityScoreDelta = table.Column<int>(type: "int", nullable: false),
                    SocialScoreDelta = table.Column<int>(type: "int", nullable: false),
                    DisciplineScoreDelta = table.Column<int>(type: "int", nullable: false),
                    ResilienceScoreDelta = table.Column<int>(type: "int", nullable: false),
                    FocusScoreDelta = table.Column<int>(type: "int", nullable: false),
                    ConfidenceScoreDelta = table.Column<int>(type: "int", nullable: false),
                    ResponsibilityScoreDelta = table.Column<int>(type: "int", nullable: false),
                    CuriosityScoreDelta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnboardingOptions_OnboardingQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "OnboardingQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Missions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Goal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PathId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    MissionType = table.Column<int>(type: "int", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    EstimatedMinutes = table.Column<int>(type: "int", nullable: false),
                    RequiredOutputType = table.Column<int>(type: "int", nullable: false),
                    ValidationType = table.Column<int>(type: "int", nullable: false),
                    RewardXP = table.Column<int>(type: "int", nullable: false),
                    RewardCoin = table.Column<int>(type: "int", nullable: false),
                    SkillProgressGain = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Missions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Missions_GrowthPaths_PathId",
                        column: x => x.PathId,
                        principalTable: "GrowthPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Missions_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SkillDependencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    RequiredSkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillDependencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillDependencies_Skills_RequiredSkillId",
                        column: x => x.RequiredSkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SkillDependencies_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSkillProgresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    ProgressPercent = table.Column<int>(type: "int", nullable: false),
                    CurrentLevel = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkillProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSkillProgresses_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkillProgresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OnboardingAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AttemptId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    OptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnboardingAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OnboardingAnswers_OnboardingAttempts_AttemptId",
                        column: x => x.AttemptId,
                        principalTable: "OnboardingAttempts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OnboardingAnswers_OnboardingOptions_OptionId",
                        column: x => x.OptionId,
                        principalTable: "OnboardingOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OnboardingAnswers_OnboardingQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "OnboardingQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MissionSubmissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MissionId = table.Column<int>(type: "int", nullable: false),
                    TextAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MediaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AdminFeedback = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RewardXPGranted = table.Column<int>(type: "int", nullable: false),
                    RewardCoinGranted = table.Column<int>(type: "int", nullable: false),
                    IsRewardGranted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MissionSubmissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MissionSubmissions_Missions_MissionId",
                        column: x => x.MissionId,
                        principalTable: "Missions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MissionSubmissions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "GrowthPaths",
                columns: new[] { "Id", "Description", "Icon", "IsActive", "PathType", "Title" },
                values: new object[,]
                {
                    { 1, "Logic, problem solving, data and decision thinking.", "analytics", true, 1, "Analytical Path" },
                    { 2, "Storytelling, design, content and ideation.", "palette", true, 2, "Creative Path" },
                    { 3, "Communication, teamwork and leadership growth.", "groups", true, 3, "Social Leadership Path" },
                    { 4, "Build with hands-on projects and practical output.", "build", true, 4, "Maker Practical Path" },
                    { 5, "Empathy, responsibility and meaningful impact.", "volunteer_activism", true, 5, "Care Meaning Path" }
                });

            migrationBuilder.InsertData(
                table: "OnboardingQuestions",
                columns: new[] { "Id", "DisplayOrder", "IsActive", "QuestionText" },
                values: new object[,]
                {
                    { 1, 1, true, "In a team conflict, what do you usually do first?" },
                    { 2, 2, true, "You have a free afternoon. Which activity is most exciting for you?" },
                    { 3, 3, true, "A task feels difficult. What is your default response?" },
                    { 4, 4, true, "When learning a new topic, how do you start?" },
                    { 5, 5, true, "Which outcome makes you proudest?" },
                    { 6, 6, true, "When others need support, how do you react?" },
                    { 7, 7, true, "How do you manage distractions?" },
                    { 8, 8, true, "Which challenge sounds most motivating?" }
                });

            migrationBuilder.InsertData(
                table: "OnboardingOptions",
                columns: new[] { "Id", "AnalyticalScoreDelta", "ConfidenceScoreDelta", "CreativityScoreDelta", "CuriosityScoreDelta", "DisciplineScoreDelta", "FocusScoreDelta", "OptionKey", "OptionText", "QuestionId", "ResilienceScoreDelta", "ResponsibilityScoreDelta", "SocialScoreDelta" },
                values: new object[,]
                {
                    { 1, 3, 0, 0, 1, 0, 0, "A", "Analyze root cause of conflict", 1, 0, 0, 0 },
                    { 2, 0, 1, 3, 0, 0, 0, "B", "Propose a creative alternative", 1, 0, 0, 0 },
                    { 3, 0, 1, 0, 0, 0, 0, "C", "Mediate between members", 1, 0, 0, 3 },
                    { 4, 0, 0, 0, 0, 1, 0, "D", "Split work and set clear owners", 1, 0, 2, 0 },
                    { 5, 2, 0, 0, 0, 0, 1, "A", "Solve logic puzzles", 2, 0, 0, 0 },
                    { 6, 0, 0, 3, 1, 0, 0, "B", "Create art/content", 2, 0, 0, 0 },
                    { 7, 0, 1, 0, 0, 0, 0, "C", "Join group activities", 2, 0, 0, 2 },
                    { 8, 0, 0, 0, 0, 0, 0, "D", "Build something practical", 2, 1, 2, 0 },
                    { 9, 2, 0, 0, 0, 1, 0, "A", "Break it into smaller steps", 3, 0, 0, 0 },
                    { 10, 0, 0, 2, 2, 0, 0, "B", "Try a new experimental route", 3, 0, 0, 0 },
                    { 11, 0, 1, 0, 0, 0, 0, "C", "Ask for peer support", 3, 0, 0, 2 },
                    { 12, 0, 0, 0, 0, 1, 0, "D", "Keep trying consistently", 3, 2, 0, 0 },
                    { 13, 3, 0, 0, 0, 0, 0, "A", "Read structure and concepts first", 4, 0, 0, 0 },
                    { 14, 0, 0, 2, 1, 0, 0, "B", "Explore examples and inspiration", 4, 0, 0, 0 },
                    { 15, 0, 1, 0, 0, 0, 0, "C", "Discuss it with others", 4, 0, 0, 2 },
                    { 16, 0, 0, 0, 0, 0, 1, "D", "Do a practical mini-project", 4, 0, 2, 0 },
                    { 17, 2, 1, 0, 0, 0, 0, "A", "Making a smart decision", 5, 0, 0, 0 },
                    { 18, 0, 0, 3, 0, 0, 0, "B", "Creating something unique", 5, 0, 0, 0 },
                    { 19, 0, 0, 0, 0, 0, 0, "C", "Helping a team succeed", 5, 0, 1, 2 },
                    { 20, 0, 0, 0, 0, 2, 0, "D", "Finishing a hard commitment", 5, 1, 0, 0 },
                    { 21, 2, 0, 0, 0, 0, 0, "A", "Offer a clear analysis", 6, 0, 1, 0 },
                    { 22, 0, 1, 2, 0, 0, 0, "B", "Bring a hopeful creative idea", 6, 0, 0, 0 },
                    { 23, 0, 1, 0, 0, 0, 0, "C", "Listen and empathize", 6, 0, 0, 2 },
                    { 24, 0, 0, 0, 0, 0, 0, "D", "Take responsibility to help directly", 6, 1, 2, 0 },
                    { 25, 0, 0, 0, 0, 2, 1, "A", "Use a structured checklist", 7, 0, 0, 0 },
                    { 26, 0, 0, 1, 0, 0, 1, "B", "Change environment and restart", 7, 1, 0, 0 },
                    { 27, 0, 1, 0, 0, 1, 0, "C", "Study with accountability partner", 7, 0, 0, 1 },
                    { 28, 0, 0, 0, 0, 0, 2, "D", "Set timer and force completion", 7, 1, 0, 0 },
                    { 29, 3, 0, 0, 0, 0, 0, "A", "Data-driven challenge", 8, 0, 0, 0 },
                    { 30, 0, 0, 3, 0, 0, 0, "B", "Creative project challenge", 8, 0, 0, 0 },
                    { 31, 0, 1, 0, 0, 0, 0, "C", "Community leadership challenge", 8, 0, 0, 3 },
                    { 32, 0, 0, 0, 0, 1, 0, "D", "Hands-on real-world mission", 8, 1, 2, 0 }
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Description", "IsCoreSkill", "Level", "Order", "PathId", "Title" },
                values: new object[,]
                {
                    { 1, "Maintain attention on one task.", true, 1, 1, 1, "Focus" },
                    { 2, "Notice patterns and details.", true, 1, 2, 1, "Observation" },
                    { 3, "Reasoning and argument structure.", true, 2, 3, 1, "Logical Thinking" },
                    { 4, "Break down and solve real problems.", true, 2, 4, 1, "Problem Solving" },
                    { 5, "Generate multiple creative options.", true, 1, 1, 2, "Idea Generation" },
                    { 6, "Explain ideas with narrative.", true, 1, 2, 2, "Storytelling" },
                    { 7, "Ship a creative output.", true, 2, 3, 2, "Creative Execution" },
                    { 8, "Listen and reflect effectively.", true, 1, 1, 3, "Active Listening" },
                    { 9, "Resolve disagreement safely.", true, 2, 2, 3, "Conflict Mediation" },
                    { 10, "Lead small teams with clarity.", true, 3, 3, 3, "Team Leadership" },
                    { 11, "Build practical mini-projects.", true, 1, 1, 4, "Hands-on Build" },
                    { 12, "Improve output through feedback.", true, 2, 2, 4, "Iteration" },
                    { 13, "Understand others' perspective.", true, 1, 1, 5, "Empathy" },
                    { 14, "Take ownership of impact.", true, 2, 2, 5, "Responsibility" },
                    { 15, "Contribute to community growth.", true, 3, 3, 5, "Community Support" }
                });

            migrationBuilder.InsertData(
                table: "Missions",
                columns: new[] { "Id", "Description", "Difficulty", "EstimatedMinutes", "Goal", "IsActive", "MissionType", "PathId", "RequiredOutputType", "RewardCoin", "RewardXP", "SkillId", "SkillProgressGain", "Title", "ValidationType" },
                values: new object[,]
                {
                    { 1, "Work on one task for 20 minutes without phone distraction.", 1, 20, "Build focus stamina", true, 2, 1, 5, 12, 30, 1, 18, "Deep Focus Sprint", 1 },
                    { 2, "Observe a daily system and write 3 repeating patterns.", 2, 15, "Improve analytical observation", true, 1, 1, 2, 8, 25, 2, 15, "Pattern Hunt", 1 },
                    { 3, "Solve one logic puzzle and explain your strategy.", 3, 25, "Strengthen logical reasoning", true, 2, 1, 2, 18, 40, 3, 20, "Logic Puzzle Review", 2 },
                    { 4, "Pick one small real-life problem and propose 2 solutions.", 4, 35, "Real-world problem solving", true, 3, 1, 2, 35, 70, 4, 28, "Solve a Real Problem", 3 },
                    { 5, "Write 10 different solutions for a simple daily challenge.", 1, 15, "Expand creativity", true, 5, 2, 2, 14, 32, 5, 18, "10 Ideas Challenge", 1 },
                    { 6, "Create a short 6-frame visual or text story.", 2, 30, "Practice storytelling", true, 5, 2, 3, 22, 45, 6, 22, "Story in 6 Frames", 2 },
                    { 7, "Publish one final creative piece and reflect on feedback.", 4, 40, "Ship creative work", true, 3, 2, 3, 36, 75, 7, 30, "Publish Creative Output", 3 },
                    { 8, "Listen to someone for 10 minutes and summarize 3 insights.", 1, 15, "Improve listening and empathy", true, 4, 3, 2, 16, 35, 8, 18, "Active Listening Practice", 1 },
                    { 9, "Run a roleplay and document how you de-escalated conflict.", 3, 25, "Practice mediation skills", true, 4, 3, 2, 24, 50, 9, 23, "Conflict Roleplay", 2 },
                    { 10, "Lead a short group discussion and collect one feedback point.", 4, 30, "Develop leadership", true, 3, 3, 4, 40, 80, 10, 32, "Lead a 20-min Session", 3 },
                    { 11, "Build a simple practical output with available tools.", 2, 35, "Hands-on execution", true, 2, 4, 3, 20, 48, 11, 22, "Build Mini Tool", 2 },
                    { 12, "Improve yesterday's output based on one feedback item.", 3, 25, "Learn iterative improvement", true, 6, 4, 2, 28, 65, 12, 26, "Iteration Loop", 3 },
                    { 13, "Ask someone about a hard experience and write what you learned.", 2, 20, "Grow empathy", true, 4, 5, 2, 18, 38, 13, 20, "Empathy Interview", 1 },
                    { 14, "Define one commitment and prove completion by end of day.", 3, 20, "Build accountability", true, 3, 5, 2, 27, 58, 14, 24, "Responsibility Promise", 2 },
                    { 15, "Take one action that helps your community and share evidence.", 5, 45, "Create meaningful impact", true, 3, 5, 3, 45, 95, 15, 35, "Community Support Action", 3 }
                });

            migrationBuilder.InsertData(
                table: "SkillDependencies",
                columns: new[] { "Id", "RequiredSkillId", "SkillId" },
                values: new object[,]
                {
                    { 1, 1, 2 },
                    { 2, 2, 3 },
                    { 3, 3, 4 },
                    { 4, 5, 6 },
                    { 5, 6, 7 },
                    { 6, 8, 9 },
                    { 7, 9, 10 },
                    { 8, 11, 12 },
                    { 9, 13, 14 },
                    { 10, 14, 15 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrowthProfiles_UserId",
                table: "GrowthProfiles",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Missions_PathId_SkillId_Difficulty_IsActive",
                table: "Missions",
                columns: new[] { "PathId", "SkillId", "Difficulty", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Missions_SkillId",
                table: "Missions",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_MissionSubmissions_MissionId",
                table: "MissionSubmissions",
                column: "MissionId");

            migrationBuilder.CreateIndex(
                name: "IX_MissionSubmissions_UserId_SubmittedAt",
                table: "MissionSubmissions",
                columns: new[] { "UserId", "SubmittedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingAnswers_AttemptId_QuestionId",
                table: "OnboardingAnswers",
                columns: new[] { "AttemptId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingAnswers_OptionId",
                table: "OnboardingAnswers",
                column: "OptionId");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingAnswers_QuestionId",
                table: "OnboardingAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingAttempts_RecommendedPathId",
                table: "OnboardingAttempts",
                column: "RecommendedPathId");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingAttempts_UserId",
                table: "OnboardingAttempts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingOptions_QuestionId",
                table: "OnboardingOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_OnboardingQuestions_IsActive_DisplayOrder",
                table: "OnboardingQuestions",
                columns: new[] { "IsActive", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_RewardTransactions_UserId_SourceType_SourceId",
                table: "RewardTransactions",
                columns: new[] { "UserId", "SourceType", "SourceId" });

            migrationBuilder.CreateIndex(
                name: "IX_SkillDependencies_RequiredSkillId",
                table: "SkillDependencies",
                column: "RequiredSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillDependencies_SkillId_RequiredSkillId",
                table: "SkillDependencies",
                columns: new[] { "SkillId", "RequiredSkillId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_PathId_Order",
                table: "Skills",
                columns: new[] { "PathId", "Order" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserGrowthPaths_GrowthPathId",
                table: "UserGrowthPaths",
                column: "GrowthPathId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGrowthPaths_UserId_GrowthPathId",
                table: "UserGrowthPaths",
                columns: new[] { "UserId", "GrowthPathId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSkillProgresses_SkillId",
                table: "UserSkillProgresses",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkillProgresses_UserId_SkillId",
                table: "UserSkillProgresses",
                columns: new[] { "UserId", "SkillId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrowthProfiles");

            migrationBuilder.DropTable(
                name: "MissionSubmissions");

            migrationBuilder.DropTable(
                name: "OnboardingAnswers");

            migrationBuilder.DropTable(
                name: "RewardTransactions");

            migrationBuilder.DropTable(
                name: "SkillDependencies");

            migrationBuilder.DropTable(
                name: "UserGrowthPaths");

            migrationBuilder.DropTable(
                name: "UserSkillProgresses");

            migrationBuilder.DropTable(
                name: "Missions");

            migrationBuilder.DropTable(
                name: "OnboardingAttempts");

            migrationBuilder.DropTable(
                name: "OnboardingOptions");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "OnboardingQuestions");

            migrationBuilder.DropTable(
                name: "GrowthPaths");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AvatarUrl",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Users");
        }
    }
}
