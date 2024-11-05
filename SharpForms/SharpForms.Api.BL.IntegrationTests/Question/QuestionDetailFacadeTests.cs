using SharpForms.Api.BL.Facades.Answer;
using SharpForms.Api.BL.Facades.Question;
using SharpForms.Api.BL.Facades.User;
using SharpForms.Api.BL.IntegrationTests.Fixtures;
using SharpForms.Common.Models.SelectOption;
using SharpForms.Common.Enums;
using Xunit;
using SharpForms.Api.DAL.Common.Entities;

namespace SharpForms.Api.BL.IntegrationTests.Question
{
    public class QuestionDetailFacadeTests : FacadeTestFixture
    {
        private readonly IQuestionDetailFacade _questionDetailFacade;
        private readonly IQuestionListFacade _questionListFacade;
        private readonly IAnswerListFacade _answerListFacade;
        private readonly IAnswerDetailFacade _answerDetailFacade;
        private readonly IUserListFacade _userListFacade;
        private readonly ISelectOptionFacade _selectOptionFacade;

        public QuestionDetailFacadeTests()
        {
            _selectOptionFacade = new SelectOptionFacade(_selectOptionRepository, _mapper);
            _questionListFacade = new QuestionListFacade(_questionRepository, _mapper);
            _userListFacade = new UserListFacade(_userRepository, _mapper);
            _answerDetailFacade = new AnswerDetailFacade(_answerRepository, _mapper);
            _answerListFacade = new AnswerListFacade(_answerRepository, _mapper);
            _questionDetailFacade = new QuestionDetailFacade(_questionRepository,  _mapper, _answerListFacade, _answerDetailFacade, _selectOptionFacade);
        }

        [Fact]
        public void Get_QuestionDetailModel_By_Id()
        {
            var model = _questionDetailFacade.GetById(new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"));
            
            Assert.NotNull(model);
            Assert.Equal("How satisfied are you with our service?", model.Text);
            Assert.NotNull(model.Answers);
            Assert.Equal(1, model.Order);
            Assert.Equal(AnswerType.Selection, model.AnswerType);
            
            Assert.All(model.Answers, a => Assert.True(a.Answer != "" && a.UserName != null));
        }

        [Fact]
        public void Get_Nonexisting_QuestionDetailModel()
        {
            var questionId = Guid.NewGuid();

            var model = _questionDetailFacade.GetById(questionId);

            Assert.Null(model);
        }
        
        [Fact]
        public void Update_QuestionDetailModel_Keep_Answers()
        {
            var existingId = new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52");
            var model = _questionDetailFacade.GetById(existingId);

            Assert.NotNull(model);

            model.Text = "Updated question text";
            model.Order = 3;

            var updatedId = _questionDetailFacade.Update(model);
            
            var updatedModel = _questionDetailFacade.GetById(updatedId.Value);
            Assert.NotNull(updatedModel);
            Assert.Equal(3, updatedModel.Order);
            Assert.Equal("Updated question text", updatedModel.Text);
            Assert.NotNull(updatedModel.Answers);
        }

        [Fact]
        public void Update_QuestionDetailModel_Delete_Answers()
        {
            var existingId = new Guid("1a43843d-450b-43a9-b2da-ccfe18fcfc52");
            var model = _questionDetailFacade.GetById(existingId);

            Assert.NotNull(model);

            model.AnswerType = AnswerType.Integer;

            var updatedId = _questionDetailFacade.Update(model);

            var updatedModel = _questionDetailFacade.GetById(updatedId.Value);
            Assert.NotNull(updatedModel);
            Assert.Empty(updatedModel.Answers);
        }

        [Fact]
        public void Update_QuestionDetailModel_Add_More_SelectOptions()
        {
            var existingId = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"); // Seed data question 1
            var model = _questionDetailFacade.GetById(existingId);

            Assert.NotNull(model);

            var sOpt1 = new SelectOptionModel{ Id = Guid.NewGuid(), QuestionId = existingId, Value = "Moze byt" };
            var sOpt2 = new SelectOptionModel { Id = Guid.NewGuid(), QuestionId = existingId, Value = "Nic moc" };
            var sOpt3 = new SelectOptionModel { Id = Guid.NewGuid(), QuestionId = existingId, Value = "Hruza" };

            var sOptOld = model.Options[0];

            model.Options.Add(sOpt1);
            model.Options.Add(sOpt2);
            model.Options.Add(sOpt3);

            var updatedId = _questionDetailFacade.Update(model);

            var updatedModel = _questionDetailFacade.GetById(updatedId.Value);
            Assert.NotNull(updatedModel);
            Assert.NotEmpty(updatedModel.Answers);
            Assert.Contains(sOpt1, updatedModel.Options);
            Assert.Contains(sOpt2, updatedModel.Options);
            Assert.Contains(sOpt3, updatedModel.Options);
            Assert.Contains(sOptOld, updatedModel.Options);
        }

        [Fact]
        public void Update_QuestionDetailModel_Remove_One_Existing_SelectOption()
        {
            var existingId = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"); // Seed data question 1
            var model = _questionDetailFacade.GetById(existingId);

            Assert.NotNull(model);
            
            var removedOpt = model.Options[0];
            model.Options.Remove(model.Options[0]);

            var updatedId = _questionDetailFacade.Update(model);

            var updatedModel = _questionDetailFacade.GetById(updatedId.Value);
            Assert.NotNull(updatedModel);
            Assert.Empty(updatedModel.Answers);
            Assert.DoesNotContain(removedOpt, updatedModel.Options);
        }

        [Fact]
        public void Update_QuestionDetailModel_Remove_One_Existing_SelectOption_And_Add_New()
        {
            var existingId = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"); // Seed data question 1
            var model = _questionDetailFacade.GetById(existingId);

            Assert.NotNull(model);
            var sOpt1 = new SelectOptionModel { Id = Guid.NewGuid(), QuestionId = existingId, Value = "Superko" };

            var removedOpt = model.Options[0];
            model.Options.Remove(model.Options[0]);

            model.Options.Add(sOpt1);

            var updatedId = _questionDetailFacade.Update(model);

            var updatedModel = _questionDetailFacade.GetById(updatedId.Value);
            Assert.NotNull(updatedModel);
            Assert.Empty(updatedModel.Answers);
            Assert.DoesNotContain(removedOpt, updatedModel.Options);
            Assert.Contains(sOpt1, updatedModel.Options);
        }

        [Fact]
        public void Update_QuestionDetailModel_Change_Existing_SelectOption()
        {
            var existingId = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"); // Seed data question 1
            var model = _questionDetailFacade.GetById(existingId);

            Assert.NotNull(model);

            model.Options[0].Value = "Nic moc";
            var updatedOpt = model.Options[0];

            var updatedId = _questionDetailFacade.Update(model);

            var updatedModel = _questionDetailFacade.GetById(updatedId.Value);
            Assert.NotNull(updatedModel);
            Assert.Empty(updatedModel.Answers);
            Assert.Contains(updatedOpt, updatedModel.Options);
        }

        [Fact]
        public void Update_QuestionDetailModel_Change_Existing_SelectOption_And_Add_New()
        {
            var existingId = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"); // Seed data question 1
            var model = _questionDetailFacade.GetById(existingId);

            Assert.NotNull(model);
            var sOpt1 = new SelectOptionModel { Id = Guid.NewGuid(), QuestionId = existingId, Value = "Superko" };

            model.Options[0].Value = "Nic moc";
            model.Options.Add(sOpt1);

            var updatedOpt = model.Options[0];

            var updatedId = _questionDetailFacade.Update(model);

            var updatedModel = _questionDetailFacade.GetById(updatedId.Value);
            Assert.NotNull(updatedModel);
            Assert.Empty(updatedModel.Answers);
            Assert.Contains(updatedOpt, updatedModel.Options);
            Assert.Contains(sOpt1, updatedModel.Options);
        }

        [Fact]
        public void Update_QuestionDetailModel_Change_All_SelectOptions()
        {
            var existingId = new Guid("fb9b6ba3-fedc-4c23-b055-386fbbf73ec1"); // Seed data question 1
            var model = _questionDetailFacade.GetById(existingId);

            Assert.NotNull(model);

            var sOptOld = model.Options[0]; 
            var newOpts = new List<SelectOptionModel>();

            var sOpt1 = new SelectOptionModel { Id = Guid.NewGuid(), QuestionId = existingId, Value = "Superko" };
            var sOpt2 = new SelectOptionModel { Id = Guid.NewGuid(), QuestionId = existingId, Value = "Moze byt" };
            var sOpt3 = new SelectOptionModel { Id = Guid.NewGuid(), QuestionId = existingId, Value = "Nic moc" };
            var sOpt4 = new SelectOptionModel { Id = Guid.NewGuid(), QuestionId = existingId, Value = "Bieda" };
            var sOpt5 = new SelectOptionModel { Id = Guid.NewGuid(), QuestionId = existingId, Value = "Hruza" };
            
            newOpts.Add(sOpt1);
            newOpts.Add(sOpt2);
            newOpts.Add(sOpt3);
            newOpts.Add(sOpt4);
            newOpts.Add(sOpt5);

            model.Options = newOpts;

            var updatedId = _questionDetailFacade.Update(model);

            var updatedModel = _questionDetailFacade.GetById(updatedId.Value);
            Assert.NotNull(updatedModel);
            Assert.Empty(updatedModel.Answers);
            Assert.DoesNotContain(sOptOld, updatedModel.Options);
            Assert.Contains(sOpt1, updatedModel.Options);
            Assert.Contains(sOpt2, updatedModel.Options);
            Assert.Contains(sOpt3, updatedModel.Options);
            Assert.Contains(sOpt4, updatedModel.Options);
            Assert.Contains(sOpt5, updatedModel.Options);
        }

        [Fact]
        public void Delete_QuestionDetailModel()
        {
            var existingId = new Guid("28974ee4-fda7-4357-ac75-d7a2388d51cf");

            _questionDetailFacade.Delete(existingId);

            var deletedModel = _questionDetailFacade.GetById(existingId);
            Assert.Null(deletedModel);
        }
    }
}
