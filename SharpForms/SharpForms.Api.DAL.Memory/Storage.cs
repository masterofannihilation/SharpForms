using System.Collections.Generic;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Common.Enums;

namespace SharpForms.Api.DAL.Memory
{
    public class Storage
    {
        public IList<FormEntity> Forms { get; } = new List<FormEntity>();
        public IList<QuestionEntity> Questions { get; } = new List<QuestionEntity>();
        public IList<SelectOptionEntity> SelectOptions { get; } = new List<SelectOptionEntity>();
        public IList<AnswerEntity> Answers { get; } = new List<AnswerEntity>();
        public IList<UserEntity> Users { get; } = new List<UserEntity>();
        public IList<CompletedFormEntity> CompletedForms { get; } = new List<CompletedFormEntity>();

        public Storage()
        {
            SeedData();
        }

        private void SeedData()
        {
            var user1 = new UserEntity
            {
                Id = new Guid("26744e13-77c9-49bf-90cd-0310e379e46d"), Name = "John Doe", Role = UserRole.General,
            };
            var user2 = new UserEntity
            {
                Id = new Guid("eebf7395-5e10-4cc5-8c10-a05a0c0f8783"), Name = "Jane Doe", Role = UserRole.General,
            };
            Users.Add(user1);
            Users.Add(user2);

            var form1 = new FormEntity
            {
                Id = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"),
                Name = "Customer Feedback",
                CreatorId = user1.Id,
                OpenSince = DateTime.UtcNow.AddDays(-10),
                OpenUntil = DateTime.UtcNow.AddDays(10),
            };
            var form2 = new FormEntity
            {
                Id = new Guid("8e1c3878-d661-4a57-86b4-d30ed1592558"),
                Name = "Job Application",
                CreatorId = user2.Id,
                OpenSince = DateTime.UtcNow.AddDays(-5),
                OpenUntil = DateTime.UtcNow.AddDays(30),
            };
            Forms.Add(form1);
            Forms.Add(form2);

            var question1 = new QuestionEntity
            {
                Id = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"),
                FormId = form1.Id,
                Order = 1,
                Text = "How satisfied are you with our service?",
                AnswerType = AnswerType.Selection
            };
            var question2 = new QuestionEntity
            {
                Id = new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"),
                FormId = form1.Id,
                Order = 2,
                Text = "Please provide additional feedback.",
                AnswerType = AnswerType.Text
            };
            var question3 = new QuestionEntity
            {
                Id = new Guid("28974ee4-fda7-4357-ac75-d7a2388d51cf"),
                FormId = form2.Id,
                Order = 1,
                Text = "What position are you applying for?",
                AnswerType = AnswerType.Text
            };
            var question4 = new QuestionEntity
            {
                Id = new Guid("a09b47c0-71cb-4294-93e8-92941fb8f1fd"),
                FormId = form2.Id,
                Order = 2,
                Text = "How many years of experience do you have in this field?",
                AnswerType = AnswerType.Integer
            };
            Questions.Add(question1);
            Questions.Add(question2);
            Questions.Add(question3);
            Questions.Add(question4);

            form1.Questions.Add(question1);
            form1.Questions.Add(question2);
            form2.Questions.Add(question3);
            form2.Questions.Add(question4);

            // SelectOptions for question1
            var option1 = new SelectOptionEntity
            {
                Id = new Guid("b8189619-b77f-4038-9441-f6785db3e25b"),
                QuestionId = question1.Id,
                Option = "Very Satisfied"
            };
            var option2 = new SelectOptionEntity
            {
                Id = new Guid("00fc620d-c945-412f-9555-08e3cb076884"),
                QuestionId = question1.Id,
                Option = "Satisfied"
            };
            var option3 = new SelectOptionEntity
            {
                Id = new Guid("8bbecf1d-9aef-4490-988d-57656790a4e4"), QuestionId = question1.Id, Option = "Average"
            };
            var option4 = new SelectOptionEntity
            {
                Id = new Guid("8d929a82-d180-4505-8676-6c81a6a270a9"),
                QuestionId = question1.Id,
                Option = "Dissatisfied"
            };
            var option5 = new SelectOptionEntity
            {
                Id = new Guid("ca10f647-ffd2-45d8-bdc9-5782840f2582"),
                QuestionId = question1.Id,
                Option = "Very Dissatisfied"
            };
            SelectOptions.Add(option1);
            SelectOptions.Add(option2);
            SelectOptions.Add(option3);
            SelectOptions.Add(option4);
            SelectOptions.Add(option5);

            // Add the select options to the question
            question1.Options.Add(option1);
            question1.Options.Add(option2);
            question1.Options.Add(option3);
            question1.Options.Add(option4);
            question1.Options.Add(option5);

            var completedForm1 = new CompletedFormEntity
            {
                Id = new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"),
                FormId = form1.Id,
                UserId = user2.Id,
                CompletedDate = DateTime.UtcNow
            };
            var completedForm2 = new CompletedFormEntity
            {
                Id = new Guid("07b2be2c-6fab-4303-b554-e1e742f526e0"),
                FormId = form2.Id,
                UserId = user1.Id,
                CompletedDate = DateTime.UtcNow
            };
            CompletedForms.Add(completedForm1);
            CompletedForms.Add(completedForm2);

            var answer1 = new AnswerEntity
            {
                Id = new Guid("b4505f75-f177-4076-832d-8fd1677c9a18"),
                CompletedFormId = completedForm1.Id,
                QuestionId = question1.Id,
                SelectOptionId = option1.Id,
            };
            var answer2 = new AnswerEntity
            {
                Id = new Guid("cbe36665-4fe4-4b9e-ae57-0b2e288c4d74"),
                CompletedFormId = completedForm1.Id,
                QuestionId = question2.Id,
                TextAnswer = "Excellent customer service!"
            };
            var answer3 = new AnswerEntity
            {
                Id = new Guid("f42f95fb-d11e-49c5-88c0-4592d6131425"),
                CompletedFormId = completedForm2.Id,
                QuestionId = question3.Id,
                TextAnswer = "Software Engineer"
            };
            var answer4 = new AnswerEntity
            {
                Id = new Guid("bb48d07c-b010-412c-91cd-9b68c9742791"),
                CompletedFormId = completedForm2.Id,
                QuestionId = question4.Id,
                NumberAnswer = 3
            };
            Answers.Add(answer1);
            Answers.Add(answer2);
            Answers.Add(answer3);
            Answers.Add(answer4);

            // Link answers to the completed forms
            completedForm1.Answers.Add(answer1);
            completedForm1.Answers.Add(answer2);
            completedForm2.Answers.Add(answer3);
            completedForm2.Answers.Add(answer4);
        }
    }
}
