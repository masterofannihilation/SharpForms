using System.Net;
using System.Net.Http.Json;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.SelectOption;
using Xunit;

namespace SharpForms.Api.App.EndToEndTests
{
    public class ApiQuestionTests : BaseControllerTests
    {
        [Fact]
        public async Task GetAllQuestions()
        {
            var response = await Client.Value.GetAsync("/api/question");

            response.EnsureSuccessStatusCode();

            var questions = await response.Content.ReadFromJsonAsync<ICollection<QuestionListModel>>();
            Assert.NotNull(questions);
            Assert.NotEmpty(questions);
            Assert.Equal(20, questions.Count);
        }

        [Fact]
        public async Task QuestionsFromForm()
        {
            var response = await Client.Value.GetAsync("/api/question?formId=8e1c3878-d661-4a57-86b4-d30ed1592558");

            response.EnsureSuccessStatusCode();

            var questions = await response.Content.ReadFromJsonAsync<ICollection<QuestionListModel>>();
            Assert.NotNull(questions);
            Assert.Equal(6, questions.Count);
            Assert.NotNull(questions.FirstOrDefault(q => q.Text == "What position are you applying for?"));
            Assert.NotNull(questions.FirstOrDefault(q =>
                q.Text == "How many years of experience do you have in this field?"));
        }

        [Fact]
        public async Task SearchQuestionsByText()
        {
            var response = await Client.Value.GetAsync("/api/question?text=Additional");

            response.EnsureSuccessStatusCode();

            var questions = await response.Content.ReadFromJsonAsync<ICollection<QuestionListModel>>();
            Assert.NotNull(questions);
            Assert.NotNull(questions.FirstOrDefault(q => q.Text == "Please provide any additional feedback."));
        }

        [Fact]
        public async Task GetQuestionDetail()
        {
            var questionId = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"); // Seed data question

            var response = await Client.Value.GetAsync($"/api/question/{questionId}");

            response.EnsureSuccessStatusCode();

            var question = await response.Content.ReadFromJsonAsync<QuestionDetailModel>();

            Assert.NotNull(question);
            Assert.Equal(questionId, question.Id);
            Assert.Equal("How satisfied are you with our service?", question.Text);
            Assert.Equal(1, question.Order);
        }

        [Fact]
        public async Task GetQuestionDetailCheckIncludedEntities()
        {
            var questionId = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"); // Seed data question

            var response = await Client.Value.GetAsync($"/api/question/{questionId}");

            response.EnsureSuccessStatusCode();

            var question = await response.Content.ReadFromJsonAsync<QuestionDetailModel>();

            Assert.NotNull(question);
            Assert.NotEmpty(question.Options);
            Assert.Equal(5, question.Options.Count);
            Assert.Equal("Very Satisfied", question.Options[0].Value);
            Assert.NotEmpty(question.Answers);
            Assert.Equal(questionId, question.Id);
            Assert.Equal("How satisfied are you with our service?", question.Text);
            Assert.Equal(1, question.Order);
        }

        [Fact]
        public async Task CreateQuestion()
        {
            var newQuestion = new QuestionDetailModel
            {
                Id = new Guid(),
                FormId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"), // Seed form
                Order = 3,
                Text = "What is your favorite feature?",
                AnswerType = Common.Enums.AnswerType.Text,
                FormName = "Customer Feedback"
            };

            var response = await Client.Value.PostAsJsonAsync("/api/question", newQuestion);
            var questionId = await response.Content.ReadFromJsonAsync<Guid>();
            response.EnsureSuccessStatusCode();

            // Retrieve the newly created question to verify
            var createdQuestionResponse = await Client.Value.GetAsync($"/api/question/{questionId}");
            createdQuestionResponse.EnsureSuccessStatusCode();
            var createdQuestion = await createdQuestionResponse.Content.ReadFromJsonAsync<QuestionDetailModel>();

            Assert.NotNull(createdQuestion);
            Assert.Equal(newQuestion.Text, createdQuestion.Text);
        }

        [Fact]
        public async Task CreateQuestionWithSelection()
        {
            var newQuestion = new QuestionDetailModel
            {
                Id = Guid.NewGuid(),
                FormId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"), // Seed form
                Order = 3,
                Text = "What is your favorite feature?",
                AnswerType = Common.Enums.AnswerType.Selection,
                FormName = "Customer Feedback"
            };

            var sOpt1 = new SelectOptionModel { Id = Guid.NewGuid(), QuestionId = newQuestion.Id, Value = "That the program works" };
            var sOpt2 = new SelectOptionModel { Id = Guid.NewGuid(), QuestionId = newQuestion.Id, Value = "Nothing" };


            newQuestion.Options.Add(sOpt1);
            newQuestion.Options.Add(sOpt2);

            var response = await Client.Value.PostAsJsonAsync("/api/question", newQuestion);
            var questionId = await response.Content.ReadFromJsonAsync<Guid>();
            response.EnsureSuccessStatusCode();

            // Retrieve the newly created question to verify
            var createdQuestionResponse = await Client.Value.GetAsync($"/api/question/{questionId}");
            createdQuestionResponse.EnsureSuccessStatusCode();
            var createdQuestion = await createdQuestionResponse.Content.ReadFromJsonAsync<QuestionDetailModel>();

            Assert.NotNull(createdQuestion);
            Assert.Equal(newQuestion.Text, createdQuestion.Text);
            Assert.NotEmpty(createdQuestion.Options);
            Assert.Contains(sOpt1, createdQuestion.Options);
            Assert.Contains(sOpt2, createdQuestion.Options);
        }

        [Fact]
        public async Task UpdateQuestionAddSelectOptions()
        {
            var existingQuestionId = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"); // Seed data question

            var updatedQuestion = new QuestionDetailModel
            {
                Id = existingQuestionId,
                FormId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"), // Seed form
                Order = 1,
                Text = "Updated question text",
                AnswerType = Common.Enums.AnswerType.Selection,
                FormName = "Customer Feedback"
            };

            var sOpt1 = new SelectOptionModel { Id = Guid.NewGuid(), QuestionId = updatedQuestion.Id, Value = "Moze byt" };
            var sOpt2 = new SelectOptionModel { Id = Guid.NewGuid(), QuestionId = updatedQuestion.Id, Value = "Nic moc" };

            updatedQuestion.Options.Add(sOpt1);
            updatedQuestion.Options.Add(sOpt2);

            var response = await Client.Value.PutAsJsonAsync($"/api/question", updatedQuestion);
            response.EnsureSuccessStatusCode();

            // Verify update
            var updatedQuestionResponse = await Client.Value.GetAsync($"/api/question/{existingQuestionId}");
            updatedQuestionResponse.EnsureSuccessStatusCode();
            var updatedQuestionResult = await updatedQuestionResponse.Content.ReadFromJsonAsync<QuestionDetailModel>();

            Assert.NotNull(updatedQuestionResult);
            Assert.NotEmpty(updatedQuestionResult.Options);
            Assert.Equal(2, updatedQuestionResult.Options.Count);
            Assert.Equal("Updated question text", updatedQuestionResult.Text);
            Assert.Contains(sOpt1, updatedQuestionResult.Options);
            Assert.Contains(sOpt2, updatedQuestionResult.Options);
        }

        [Fact]
        public async Task DeleteQuestion()
        {
            var questionId = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"); // Seed data question

            var response = await Client.Value.DeleteAsync($"/api/question/{questionId}");
            response.EnsureSuccessStatusCode();

            // Verify deletion
            response = await Client.Value.GetAsync($"/api/question/{questionId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
