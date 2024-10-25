using AutoMapper;
using SharpForms.Api.BL.Facades.Form;
using SharpForms.Api.BL.IntegrationTests.Fixtures;
using SharpForms.Common.Models.Form;
using SharpForms.Common.Models.Question;
using SharpForms.Common.Models.User;
using Xunit;

namespace SharpForms.Api.BL.IntegrationTests.Form
{
    public class FormDetailFacadeTests : FacadeTestFixture
    {
        private readonly IFormDetailFacade _formDetailFacade;

        public FormDetailFacadeTests()
        {
            _formDetailFacade = new FormDetailFacade(_formRepository, _mapper);
        }

        [Fact]
        public void Get_FormDetailModel_By_Id()
        {
            var formId = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8"); // replace with a valid ID from your in-memory storage
            
            var model = _formDetailFacade.GetById(formId);
            
            Assert.NotNull(model);
            Assert.Equal("Customer Feedback", model.Name);
            Assert.NotNull(model.Creator);
            Assert.NotNull(model.Questions);
            Assert.True(model.Questions.Count > 0); // or specify an expected count
        }

        [Fact]
        public void Get_FormDetailModel_Returns_Null_When_Not_Found()
        {
            var formId = Guid.NewGuid(); // Generate a random GUID not in storage

            var model = _formDetailFacade.GetById(formId);

            Assert.Null(model);
        }
        
        [Fact]
        public void Update_FormDetailModel()
        {
            var existingId = new Guid("8e1c3878-d661-4a57-86b4-d30ed1592558"); 
            var model = _formDetailFacade.GetById(existingId);

            Assert.NotNull(model);

            // Update the form's name
            model.Name = "Updated Form Name";

            var updatedId = _formDetailFacade.Update(model);
            Assert.Equal(existingId, updatedId); // Ensure the returned ID matches

            var updatedModel = _formDetailFacade.GetById(existingId);
            Assert.NotNull(updatedModel);
            Assert.Equal("Updated Form Name", updatedModel.Name); // Verify the name has been updated
        }

        [Fact]
        public void Delete_Form_By_Id()
        {
            var formIdToDelete = new Guid("01e7e4c9-1ad7-4688-883e-69b6591338b8");
            var modelBeforeDelete = _formDetailFacade.GetById(formIdToDelete);

            Assert.NotNull(modelBeforeDelete);

            _formDetailFacade.Delete(formIdToDelete);

            var modelAfterDelete = _formDetailFacade.GetById(formIdToDelete);
            Assert.Null(modelAfterDelete); // Verify the form no longer exists
        }

        [Fact]
        public void CreateOrUpdate_FormDetailModel_Creates_New_Form()
        {
            var formId = new Guid("9a40ab03-98cc-4bd8-ba9c-72e247060a0b");
            var newFormModel = new FormDetailModel
            {
                Id = formId,
                Name = "New Test Form",
                OpenSince = DateTime.UtcNow,
                OpenUntil = DateTime.UtcNow.AddDays(7),
            };

            var createdId = _formDetailFacade.CreateOrUpdate(newFormModel);
            var createdModel = _formDetailFacade.GetById(createdId.Value);

            Assert.NotNull(createdModel);
            Assert.Equal("New Test Form", createdModel.Name); // Verify the created form's name
        }

        [Fact]
        public void CreateOrUpdate_FormDetailModel_Updates_Existing_Form()
        {
            var existingId = new Guid("8e1c3878-d661-4a57-86b4-d30ed1592558");
            var model = _formDetailFacade.GetById(existingId);

            Assert.NotNull(model);

            // Update some properties
            model.Name = "Updated Form Name";

            var updatedId = _formDetailFacade.CreateOrUpdate(model);
            Assert.Equal(existingId, updatedId);

            var updatedModel = _formDetailFacade.GetById(existingId);
            Assert.NotNull(updatedModel);
            Assert.Equal("Updated Form Name", updatedModel.Name);
        }
    }
}
