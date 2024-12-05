using System.Collections.Generic;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Seeds;
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
            /***************************************************************************************
            * Users
            */
            var user1 = UserSeeds.User1;
            var user2 = UserSeeds.User2;
            var user3 = UserSeeds.User3;
            var user4 = UserSeeds.User4;
            var user5 = UserSeeds.User5;
            Users.Add(user1);
            Users.Add(user2);
            Users.Add(user3);
            Users.Add(user4);
            Users.Add(user5);

            /***************************************************************************************
            * Forms
            */
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
            var form3 = new FormEntity
            {
                Id = new Guid("fba15bb5-371b-4850-9b5f-7fcbbcdc05d5"),
                Name = "Event Registration",
                CreatorId = user1.Id,
                OpenSince = DateTime.UtcNow,
                OpenUntil = DateTime.UtcNow.AddDays(5),
            };
            var form4 = new FormEntity
            {
                Id = new Guid("58237ec7-36dd-4c0d-bb00-118027f90005"),
                Name = "Volunteer Signup",
                CreatorId = user2.Id,
                OpenSince = DateTime.UtcNow.AddDays(-13),
                OpenUntil = DateTime.UtcNow.AddDays(2),
            };
            var form5 = new FormEntity
            {
                Id = new Guid("5be6ddb2-1c20-46e2-8b73-d9291d5afd3a"),
                Name = "Course Evaluation",
                CreatorId = user3.Id,
                OpenSince = DateTime.UtcNow,
                OpenUntil = DateTime.UtcNow.AddDays(30),
            };
            Forms.Add(form1);
            Forms.Add(form2);
            Forms.Add(form3);
            Forms.Add(form4);
            Forms.Add(form5);

            /***************************************************************************************
            * Questions for Form 1 - customer feedback
            */
            var question11 = new QuestionEntity
            {
                Id = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"),
                FormId = form1.Id,
                Order = 1,
                Text = "How satisfied are you with our service?",
                AnswerType = AnswerType.Selection
            };
            var question12 = new QuestionEntity
            {
                Id = new Guid("b4387eeb-6f6d-4b83-97db-5408a9f92c91"),
                FormId = form1.Id,
                Order = 3,
                Text = "What product or service did you use?",
                AnswerType = AnswerType.Selection
            };
            var question13 = new QuestionEntity
            {
                Id = new Guid("43e50a0d-2f95-45b1-9884-ea5d5987b4f3"),
                FormId = form1.Id,
                Order = 4,
                Text = "Would you recommend us to others?",
                AnswerType = AnswerType.Selection
            };
            var question14 = new QuestionEntity
            {
                Id = new Guid("4b04120f-63b5-4f95-b5b2-3ec0f1bff0d6"),
                FormId = form1.Id,
                Order = 5,
                Text = "How often do you use our services?",
                AnswerType = AnswerType.Selection
            };
            var question15 = new QuestionEntity
            {
                Id = new Guid("62e13887-0f55-4d1c-95d8-89f55de3f73a"),
                FormId = form1.Id,
                Order = 7,
                Text = "What could we improve?",
                AnswerType = AnswerType.Text
            };
            var question16 = new QuestionEntity
            {
                Id = new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52"),
                FormId = form1.Id,
                Order = 2,
                Text = "Please provide any additional feedback.",
                AnswerType = AnswerType.Text
            };

            /***************************************************************************************
            * Questions for Form 2 - Job application
            */
            var question21 = new QuestionEntity
            {
                Id = new Guid("28974ee4-fda7-4357-ac75-d7a2388d51cf"),
                FormId = form2.Id,
                Order = 1,
                Text = "What position are you applying for?",
                AnswerType = AnswerType.Text
            };
            var question22 = new QuestionEntity
            {
                Id = new Guid("a09b47c0-71cb-4294-93e8-92941fb8f1fd"),
                FormId = form2.Id,
                Order = 2,
                Text = "How many years of experience do you have in this field?",
                AnswerType = AnswerType.Integer
            };
            var question23 = new QuestionEntity
            {
                Id = new Guid("63a7c3a4-f89a-4740-b78d-1e567e4e273d"),
                FormId = form2.Id,
                Order = 3,
                Text = "What are your top skills?",
                AnswerType = AnswerType.Text
            };
            var question24 = new QuestionEntity
            {
                Id = new Guid("0d4622f5-bf30-4c24-a8d3-7eebf1e1c857"),
                FormId = form2.Id,
                Order = 4,
                Text = "Why do you want to work with us?",
                AnswerType = AnswerType.Text
            };
            var question25 = new QuestionEntity
            {
                Id = new Guid("b26fc96a-4be5-4d67-85ad-61bcdb2cf51f"),
                FormId = form2.Id,
                Order = 5,
                Text = "Describe a challenging project you completed.",
                AnswerType = AnswerType.Text
            };
            var question26 = new QuestionEntity
            {
                Id = new Guid("5a71b2f1-98d4-47f9-910e-2d7b1c6fdc3c"),
                FormId = form2.Id,
                Order = 6,
                Text = "What are your monthly salary expectations?",
                AnswerType = AnswerType.Decimal
            };

            /***************************************************************************************
            * Questions for Form 3 - Event Registration
            */
            var question31 = new QuestionEntity
            {
                Id = new Guid("8ecf5473-7df1-40c5-b20b-b4324f0c3f1e"),
                FormId = form3.Id,
                Order = 1,
                Text = "What is your full name?",
                AnswerType = AnswerType.Text
            };
            var question32 = new QuestionEntity
            {
                Id = new Guid("6d572b73-43d1-4a0a-bbc4-120c6fb832cf"),
                FormId = form3.Id,
                Order = 2,
                Text = "What is your email address?",
                AnswerType = AnswerType.Text
            };
            var question33 = new QuestionEntity
            {
                Id = new Guid("f3c29d15-853f-4f95-8be2-5ae25a2dfad1"),
                FormId = form3.Id,
                Order = 3,
                Text = "How many guests will you be bringing?",
                AnswerType = AnswerType.Integer
            };
            var question34 = new QuestionEntity
            {
                Id = new Guid("ba6f53c2-0199-4db5-9e9f-6d5e0f6ec8e0"),
                FormId = form3.Id,
                Order = 4,
                Text = "Do you have any dietary restrictions?",
                AnswerType = AnswerType.Text
            };

            /***************************************************************************************
            * Questions for Form 4 - Volunteer signup
            */
            var question41 = new QuestionEntity
            {
                Id = new Guid("4b67af82-29d4-4c48-9bd8-915b43b8c254"),
                FormId = form4.Id,
                Order = 1,
                Text = "Will you be available for the whole day of 17.11.2024?",
                AnswerType = AnswerType.Text
            };
            var question42 = new QuestionEntity
            {
                Id = new Guid("c3f47e4f-e1b1-4e51-8a9f-b2c28fffc4a3"),
                FormId = form4.Id,
                Order = 2,
                Text = "What volunteer experience do you have?",
                AnswerType = AnswerType.Text
            };
            var question43 = new QuestionEntity
            {
                Id = new Guid("ef3d6340-2e5d-4c0a-8e93-8ed30d7f3b0a"),
                FormId = form4.Id,
                Order = 3,
                Text = "Do you have any special skills or certifications?",
                AnswerType = AnswerType.Text
            };
            var question44 = new QuestionEntity
            {
                Id = new Guid("6ec35d18-0b97-4c9f-bf3d-1d8cf34f4d5e"),
                FormId = form4.Id,
                Order = 4,
                Text = "Please provide your phone number so we can contact you",
                AnswerType = AnswerType.Text
            };
            foreach (var question in new[]
            {
                question11, question12, question13, question14, question15, question16,
                question21, question22, question23, question24, question25, question26,
                question31, question32, question33, question34,
                question41, question42, question43, question44
            })
            {
                Questions.Add(question);
            }

            /***************************************************************************************
            * SelectOptions for Question11 - How satisfied are you with our service?
            */
            var option1 = new SelectOptionEntity
            {
                Id = new Guid("b8189619-b77f-4038-9441-f6785db3e25b"),
                QuestionId = question11.Id,
                Option = "Very Satisfied"
            };
            var option2 = new SelectOptionEntity
            {
                Id = new Guid("00fc620d-c945-412f-9555-08e3cb076884"),
                QuestionId = question11.Id,
                Option = "Satisfied"
            };
            var option3 = new SelectOptionEntity
            {
                Id = new Guid("8bbecf1d-9aef-4490-988d-57656790a4e4"), 
                QuestionId = question11.Id, 
                Option = "Average"
            };
            var option4 = new SelectOptionEntity
            {
                Id = new Guid("8d929a82-d180-4505-8676-6c81a6a270a9"),
                QuestionId = question11.Id,
                Option = "Dissatisfied"
            };
            var option5 = new SelectOptionEntity
            {
                Id = new Guid("ca10f647-ffd2-45d8-bdc9-5782840f2582"),
                QuestionId = question11.Id,
                Option = "Very Dissatisfied"
            };

            /***************************************************************************************
            * SelectOptions for Question12 - What product or service did you use?
            */
            var option6 = new SelectOptionEntity
            {
                Id = new Guid("c214111e-fc43-4b6b-97dc-9dcb50b045e9"),
                QuestionId = question12.Id,
                Option = "Product A"
            };
            var option7 = new SelectOptionEntity
            {
                Id = new Guid("3b77b8b2-f8af-4d83-8d1c-4af5d69f7ab8"),
                QuestionId = question12.Id,
                Option = "Product B"
            };
            var option8 = new SelectOptionEntity
            {
                Id = new Guid("1de6ff96-6b74-429b-8c56-129f1c7f5d0e"),
                QuestionId = question12.Id,
                Option = "Service X"
            };
            var option9 = new SelectOptionEntity
            {
                Id = new Guid("9f658d6f-48a7-49c9-9632-8c4cbd0513bb"),
                QuestionId = question12.Id,
                Option = "Service Y"
            };

            /***************************************************************************************
            * SelectOptions for question13 - Would you recommend us to others?
            */
            var option10 = new SelectOptionEntity
            {
                Id = new Guid("5b82e3f4-43f6-4c60-8a98-d3e1f098f2a2"),
                QuestionId = question13.Id,
                Option = "Yes"
            };
            var option11 = new SelectOptionEntity
            {
                Id = new Guid("3cb8de1b-4541-4713-90cf-91a4a6d3c499"),
                QuestionId = question13.Id,
                Option = "No"
            };

            /***************************************************************************************
            * SelectOptions for question14 - How often do you use our services?
            */
            var option12 = new SelectOptionEntity
            {
                Id = new Guid("2271f1d4-bd21-4a1b-ae4d-50fc8d0c8ab7"),
                QuestionId = question14.Id,
                Option = "Daily"
            };
            var option13 = new SelectOptionEntity
            {
                Id = new Guid("fb69af58-4fda-4c8c-945f-687f6787c4c7"),
                QuestionId = question14.Id,
                Option = "Weekly"
            };
            var option14 = new SelectOptionEntity
            {
                Id = new Guid("c37d0e25-f1fd-4115-bd20-f0d7f9dfc6db"),
                QuestionId = question14.Id,
                Option = "Monthly"
            };
            var option15 = new SelectOptionEntity
            {
                Id = new Guid("7c518cc1-2bfa-4b36-9b5f-f97b1db5482d"),
                QuestionId = question14.Id,
                Option = "Rarely"
            };
            foreach (var option in new[]
            {
                option1, option2, option3, option4, option5,
                option6, option7, option8, option9,
                option10, option11,
                option12, option13, option14, option15
            })
            {
                SelectOptions.Add(option);
            }

            /***************************************************************************************
            * CompletedForms
            */
            var completedForm11 = new CompletedFormEntity
            {
                Id = new Guid("2feb50ff-d066-416e-b3bf-10bc84fab6d8"),
                FormId = form1.Id,
                UserId = user2.Id,
                CompletedDate = DateTime.UtcNow
            };
            var completedForm12 = new CompletedFormEntity
            {
                Id = new Guid("7f6ac607-78db-4131-9259-48144d085a5e"),
                FormId = form1.Id,
                UserId = user3.Id,
                CompletedDate = DateTime.UtcNow.AddDays(-1)
            };
            var completedForm21 = new CompletedFormEntity
            {
                Id = new Guid("07b2be2c-6fab-4303-b554-e1e742f526e0"),
                FormId = form2.Id,
                UserId = user1.Id,
                CompletedDate = DateTime.UtcNow
            };
            var completedForm22 = new CompletedFormEntity
            {
                Id = new Guid("32ca3cf6-e40f-4bc7-890d-45a68133c25c"),
                FormId = form2.Id,
                UserId = user4.Id,
                CompletedDate = DateTime.UtcNow.AddDays(-2)
            };
            var completedForm31 = new CompletedFormEntity
            {
                Id = new Guid("1eec4ee0-636c-4f84-b75d-59d5449187c4"),
                FormId = form3.Id,
                UserId = user1.Id,
                CompletedDate = DateTime.UtcNow.AddDays(-1)
            };
            var completedForm32 = new CompletedFormEntity
            {
                Id = new Guid("f88e09fb-7c39-40f9-bca6-c5d9500cf88a"),
                FormId = form3.Id,
                UserId = user2.Id,
                CompletedDate = DateTime.UtcNow.AddDays(-3)
            };
            var completedForm41 = new CompletedFormEntity
            {
                Id = new Guid("33f40fbb-c82c-4e81-9370-a63a369a0608"),
                FormId = form4.Id,
                UserId = user1.Id,
                CompletedDate = DateTime.UtcNow.AddDays(-2)
            };
            var completedForm42 = new CompletedFormEntity
            {
                Id = new Guid("e31adcc7-e112-4248-a6ac-08cab8a6fd47"),
                FormId = form4.Id,
                UserId = user2.Id,
                CompletedDate = DateTime.UtcNow.AddDays(-1)
            };
            CompletedForms.Add(completedForm11);
            CompletedForms.Add(completedForm12);
            CompletedForms.Add(completedForm21);
            CompletedForms.Add(completedForm22);
            CompletedForms.Add(completedForm31);
            CompletedForms.Add(completedForm32);
            CompletedForms.Add(completedForm41);
            CompletedForms.Add(completedForm42);

            /***************************************************************************************
            * Answers for CompletedForm 11
            */
            var answer111 = new AnswerEntity
            {
                Id = new Guid("b4505f75-f177-4076-832d-8fd1677c9a18"),
                QuestionId = question11.Id,
                CompletedFormId = completedForm11.Id,
                SelectOptionId = option1.Id,
            };
            var answer112 = new AnswerEntity
            {
                Id = new Guid("1e392b3a-5fbd-4998-92f2-c5d3b8f4e9bc"),
                QuestionId = question12.Id,
                CompletedFormId = completedForm11.Id,
                SelectOptionId = option7.Id
            };
            var answer113 = new AnswerEntity
            {
                Id = new Guid("e3b43a75-5d41-4472-9b25-f2ff097b8f23"),
                QuestionId = question13.Id,
                CompletedFormId = completedForm11.Id,
                SelectOptionId = option10.Id
            };
            var answer114 = new AnswerEntity
            {
                Id = new Guid("7dbb8f17-d0d8-4a25-9bd8-927cda91f6e2"),
                QuestionId = question14.Id,
                CompletedFormId = completedForm11.Id,
                SelectOptionId = option13.Id
            };
            var answer115 = new AnswerEntity
            {
                Id = new Guid("bf92d6de-4eb1-4e89-b438-bfbfd61e1b36"),
                QuestionId = question15.Id,
                CompletedFormId = completedForm11.Id,
                TextAnswer = "More appointment options"
            };
            var answer116 = new AnswerEntity
            {
                Id = new Guid("03cbb637-96c8-4c12-9b1f-84c9b2c3f1ab"),
                QuestionId = question16.Id,
                CompletedFormId = completedForm11.Id,
                TextAnswer = "Overall great experience, thank you!"
            };

            /***************************************************************************************
            * Answers for CompletedForm 12
            */
            var answer121 = new AnswerEntity
            {
                Id = new Guid("f1085f56-0899-4da0-ae53-f26c92981fda"),
                CompletedFormId = completedForm12.Id,
                QuestionId = question11.Id,
                SelectOptionId = option5.Id,
            };
            var answer122 = new AnswerEntity
            {
                Id = new Guid("b5de3913-c9cb-4497-a281-0dcdd79f0eb0"),
                QuestionId = question12.Id,
                CompletedFormId = completedForm12.Id,
                SelectOptionId = option8.Id
            };
            var answer123 = new AnswerEntity
            {
                Id = new Guid("923522df-4ce5-4d55-8508-ba92ac258971"),
                QuestionId = question13.Id,
                CompletedFormId = completedForm12.Id,
                SelectOptionId = option11.Id
            };
            var answer124 = new AnswerEntity
            {
                Id = new Guid("926196db-bdcf-4cfc-b9d6-0db261942b1e"),
                QuestionId = question14.Id,
                CompletedFormId = completedForm12.Id,
                SelectOptionId = option14.Id
            };
            var answer125 = new AnswerEntity
            {
                Id = new Guid("41d5e284-449a-4acf-bd7a-4defe94e5a26"),
                QuestionId = question15.Id,
                CompletedFormId = completedForm12.Id,
                TextAnswer = "Your services not being terrible"
            };
            var answer126 = new AnswerEntity
            {
                Id = new Guid("8715aaed-fb67-4c04-bd70-a85fd8a4b349"),
                QuestionId = question16.Id,
                CompletedFormId = completedForm12.Id,
                TextAnswer = "I honestly don't understand how it is possible that you have any customers"
            };


            /***************************************************************************************
            * Answers for CompletedForm 21
            */
            var answer211 = new AnswerEntity
            {
                Id = new Guid("62f94798-1c4d-491e-8a4e-b344c1de6ef1"),
                QuestionId = question21.Id,
                CompletedFormId = completedForm21.Id,
                TextAnswer = "Software Developer" // Job position applied for
            };
            var answer212 = new AnswerEntity
            {
                Id = new Guid("d1395798-8d8c-4379-b918-9077f3a2b896"),
                QuestionId = question22.Id,
                CompletedFormId = completedForm21.Id,
                NumberAnswer = 5 
            };
            var answer213 = new AnswerEntity
            {
                Id = new Guid("7b5930b9-7b54-4c9e-bab7-6b3e4c1c3a93"),
                QuestionId = question23.Id,
                CompletedFormId = completedForm21.Id,
                TextAnswer = "Problem-solving, team collaboration"
            };
            var answer214 = new AnswerEntity
            {
                Id = new Guid("c451b2f0-fad7-486f-94a9-8dce61f2d69a"),
                QuestionId = question24.Id,
                CompletedFormId = completedForm21.Id,
                TextAnswer = "To contribute to a progressive team"
            };
            var answer215 = new AnswerEntity
            {
                Id = new Guid("39d7db2c-918c-43b9-a018-13f2c0f7f153"),
                QuestionId = question25.Id,
                CompletedFormId = completedForm21.Id,
                TextAnswer = "Built an API that scaled to handle thousands of users"
            };
            var answer216 = new AnswerEntity
            {
                Id = new Guid("82b14c0e-95c9-42e5-8b8e-1e7a2c3b18bc"),
                QuestionId = question26.Id,
                CompletedFormId = completedForm21.Id,
                NumberAnswer = 4500.00
            };

            /***************************************************************************************
            * Answers for CompletedForm 31
            */
            var answer311 = new AnswerEntity
            {
                Id = new Guid("ec97a68d-f91a-4b9d-8577-a462b3f7c93e"),
                QuestionId = question31.Id,
                CompletedFormId = completedForm31.Id,
                TextAnswer = "Alex Brown" 
            };
            var answer312 = new AnswerEntity
            {
                Id = new Guid("f14a7c6e-a0a5-4b09-9754-75b8d4e5b4b5"),
                QuestionId = question32.Id,
                CompletedFormId = completedForm31.Id,
                TextAnswer = "alex.brown@example.com" 
            };
            var answer313 = new AnswerEntity
            {
                Id = new Guid("93c7b4db-2c5f-4f1e-9ad7-4a2b8f8e8f25"),
                QuestionId = question33.Id,
                CompletedFormId = completedForm31.Id,
                NumberAnswer = 2 
            };
            var answer314 = new AnswerEntity
            {
                Id = new Guid("b0d5e2c9-48f6-4728-90f2-1c3c3d9e2f71"),
                QuestionId = question34.Id,
                CompletedFormId = completedForm31.Id,
                TextAnswer = "Vegetarian" 
            };

            /***************************************************************************************
            * Answers for CompletedForm 41
            */
            var answer411 = new AnswerEntity
            {
                Id = new Guid("0d79c1e4-cf4e-4a3e-9d4b-34d8f4e7b2b5"),
                QuestionId = question41.Id,
                CompletedFormId = completedForm41.Id,
                TextAnswer = "Yes" 
            };
            var answer412 = new AnswerEntity
            {
                Id = new Guid("1f39c5a5-6b2e-4823-b4f9-02c5d4b1c9e7"),
                QuestionId = question42.Id,
                CompletedFormId = completedForm41.Id,
                TextAnswer = "Worked with local charity for 3 years"
            };
            var answer413 = new AnswerEntity
            {
                Id = new Guid("7a8d9f1b-392a-4e59-9d5b-29d8d1c9f3c3"),
                QuestionId = question43.Id,
                CompletedFormId = completedForm41.Id,
                TextAnswer = "Certified in First Aid" 
            };
            var answer414 = new AnswerEntity
            {
                Id = new Guid("82c9e7b2-d4c3-4f89-98d5-b2d7c8c7a1b5"),
                QuestionId = question44.Id,
                CompletedFormId = completedForm41.Id,
                TextAnswer = "123-456-7890" 
            };

            foreach (var answer in new[]
            {
                answer111, answer112, answer113, answer114, answer115, answer116,
                answer121, answer122, answer123, answer124, answer125, answer126,
                answer211, answer212, answer213, answer214, answer215, answer216,
                answer311, answer312, answer313, answer314,
                answer411, answer412, answer413, answer414
            })
            {
                Answers.Add(answer);
            }
        }
    }
}
