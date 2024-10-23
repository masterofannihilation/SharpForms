using System.Net.Http.Json;
using SharpForms.Common.Models.Question;
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
            Assert.Equal(4, questions.Count);
        }

        [Fact]
        public async Task QuestionsFromForm()
        {
            var response = await Client.Value.GetAsync("/api/question?formId=8e1c3878-d661-4a57-86b4-d30ed1592558");

            response.EnsureSuccessStatusCode();

            var questions = await response.Content.ReadFromJsonAsync<ICollection<QuestionListModel>>();
            Assert.NotNull(questions);
            Assert.Equal(2, questions.Count);
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
            Assert.NotNull(questions.FirstOrDefault(q => q.Text == "Please provide additional feedback."));
        }
    }
}
