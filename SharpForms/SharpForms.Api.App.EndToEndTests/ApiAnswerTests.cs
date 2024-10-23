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
    }
}
