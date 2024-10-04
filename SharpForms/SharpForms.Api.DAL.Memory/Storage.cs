using System.Collections.Generic;
using SharpForms.Api.DAL.Common.Entities;

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
    }
}
