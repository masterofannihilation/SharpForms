using System.Net;
using System.Net.Http.Json;
using SharpForms.Common.Models.Answer;

using Xunit;

namespace SharpForms.Api.App.EndToEndTests
{
    public class ApiAnswerTests : BaseControllerTests
    {
        [Fact]
        public async Task GetAllAnswers()
        {
            var response = await Client.Value.GetAsync("/api/answer");

            response.EnsureSuccessStatusCode();

            var answers = await response.Content.ReadFromJsonAsync<ICollection<AnswerListModel>>();
            Assert.NotNull(answers);
            Assert.NotEmpty(answers);

            var answer = answers.FirstOrDefault(a => a.Id == new Guid("b4505f75-f177-4076-832d-8fd1677c9a18"));
            Assert.NotNull(answer);

            var expected = new AnswerListModel()
            {
                Id = new Guid("b4505f75-f177-4076-832d-8fd1677c9a18"),
                Order = 1,
                UserName = "Jane Doe",
                Answer = "Very Satisfied",
            };

            Assert.Equal(expected, answer);
        }

        [Fact]
        public async Task GetAnswersFromCompletedForm()
        {
            var completedFormId = new Guid("07b2be2c-6fab-4303-b554-e1e742f526e0"); // Seed data form 2
            var response = await Client.Value.GetAsync($"/api/answer/?formId={completedFormId}");

            response.EnsureSuccessStatusCode();

            var answers = await response.Content.ReadFromJsonAsync<ICollection<AnswerListModel>>();
            Assert.NotNull(answers);
            Assert.NotEmpty(answers);

            var answer = answers.FirstOrDefault(a => a.Id == new Guid("d1395798-8d8c-4379-b918-9077f3a2b896"));
            Assert.NotNull(answer);
        }

        [Fact]
        public async Task GetAnswersForQuestion()
        {
            var questionId = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"); // Seed data question 11

            var response = await Client.Value.GetAsync($"/api/answer/?questionId={questionId}");

            response.EnsureSuccessStatusCode();

            var answers = await response.Content.ReadFromJsonAsync<ICollection<AnswerListModel>>();
            Assert.NotNull(answers);
            Assert.NotEmpty(answers);
            Assert.Equal(2, answers.Count);

            var answer = answers.FirstOrDefault(a => a.Id == new Guid("b4505f75-f177-4076-832d-8fd1677c9a18")); // Seed data answer 111
            Assert.NotNull(answer);
            answer = answers.FirstOrDefault(a => a.Id == new Guid("f1085f56-0899-4da0-ae53-f26c92981fda")); // Seed data answer 121
            Assert.NotNull(answer);
        }

        [Fact]
        public async Task GetAnswerDetail()
        {
            var answerId = new Guid("41d5e284-449a-4acf-bd7a-4defe94e5a26"); // Seed data answer 125

            var response = await Client.Value.GetAsync($"/api/answer/{answerId}");

            response.EnsureSuccessStatusCode();

            var answer = await response.Content.ReadFromJsonAsync<AnswerDetailModel>();
            
            Assert.NotNull(answer);
            Assert.Equal(answerId, answer.Id);
            Assert.Equal("Your services not being terrible", answer.Answer);
            Assert.Equal("What could we improve?", answer.Question.Text);
        }

        [Fact]
        public async Task DeleteAnswer()
        {
            var answerId = new Guid("f42f95fb-d11e-49c5-88c0-4592d6131425"); // Seed data answer 3

            var response = await Client.Value.DeleteAsync($"/api/answer/{answerId}");
            response.EnsureSuccessStatusCode();

            // Verify the answer no longer exists
            response = await Client.Value.GetAsync($"/api/answer/{answerId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task CreateAnswer()
        {
            var newAnswer = new AnswerSubmitModel
            {
                Text = "What's updog?",
                Id = new Guid(),
                QuestionId = new Guid("a09b47c0-71cb-4294-93e8-92941fb8f1fd"), // Seed question
                FilledFormId = new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783"), // Seed form
                AnswerType = Common.Enums.AnswerType.Integer,
                NumberAnswer = 10
            };

            var response = await Client.Value.PostAsJsonAsync("/api/answer", newAnswer);
            var answerId = await response.Content.ReadFromJsonAsync<Guid?>();
            response.EnsureSuccessStatusCode();

            var createdAnswerResponse = await Client.Value.GetAsync($"/api/answer/{answerId}");
            createdAnswerResponse.EnsureSuccessStatusCode();
            var createdAnswer = await createdAnswerResponse.Content.ReadFromJsonAsync<AnswerDetailModel>();

            Assert.NotNull(createdAnswer);
            Assert.Equal("10", createdAnswer.Answer);
        }

        [Fact]
        public async Task UpdateAnswer()
        {
            var existingAnswerId = new Guid("bb48d07c-b010-412c-91cd-9b68c9742791"); // Seed data Answer 4

            var updateAnswer = new AnswerSubmitModel
            {
                Text = "How many years of experience do you have in this field?",
                Id = existingAnswerId,
                QuestionId = new Guid("a09b47c0-71cb-4294-93e8-92941fb8f1fd"), // Seed question
                FilledFormId = new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783"), // Seed form
                AnswerType = Common.Enums.AnswerType.Integer,
                NumberAnswer = 5
            };

            var response = await Client.Value.PostAsJsonAsync($"/api/answer", updateAnswer);
            response.EnsureSuccessStatusCode();

            var updatedAnswerResponse = await Client.Value.GetAsync($"/api/answer/{existingAnswerId}");
            updatedAnswerResponse.EnsureSuccessStatusCode();
            var updatedAnswer = await updatedAnswerResponse.Content.ReadFromJsonAsync<AnswerDetailModel>();

            Assert.NotNull(updatedAnswer);
            Assert.Equal("5", updatedAnswer.Answer);
        }
    }
}
