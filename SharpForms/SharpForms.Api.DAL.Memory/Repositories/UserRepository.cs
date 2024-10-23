using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using SharpForms.Api.DAL.Common.Entities;
using SharpForms.Api.DAL.Common.Repositories;

namespace SharpForms.Api.DAL.Memory.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IList<UserEntity> _users;
        private readonly IList<CompletedFormEntity> _completedForms;
        private readonly IList<FormEntity> _createdForms;
        private readonly IMapper _mapper;

        public UserRepository(Storage storage, IMapper mapper)
        {
            _users = storage.Users;
            _completedForms = storage.CompletedForms;
            _createdForms = storage.Forms;
            _mapper = mapper;
        }

        public IList<UserEntity> GetAll()
        {
            return _users.Select(user => user.DeepCopy()).ToList();
        }

        public UserEntity? GetById(Guid id)
        {
            var userEntity = _users.SingleOrDefault(user => user.Id == id);

            if (userEntity == null) return null;

            userEntity.CompletedForms = _completedForms
                .Where(form => form.UserId == userEntity.Id)
                .Select(form => form.DeepCopy())
                .ToList();
                
            userEntity.CreatedForms = _createdForms
                .Where(form => form.CreatorId == userEntity.Id)
                .Select(form => form.DeepCopy())
                .ToList();
                
            return userEntity.DeepCopy();
        }

        public Guid Insert(UserEntity user)
        {
            var userCopy = user.DeepCopy();
            _users.Add(userCopy);
            return userCopy.Id;
        }

        public Guid? Update(UserEntity user)
        {
            var existingUser = _users.SingleOrDefault(u => u.Id == user.Id);

            if (existingUser == null) return null;

            _mapper.Map(user, existingUser); // Update user
            return existingUser.Id;
        }

        public void Remove(Guid id)
        {
            var userToRemove = _users.SingleOrDefault(user => user.Id == id);
            if (userToRemove == null)
            {
                return;
            }

            // Set all created forms' CreatorId to null
            var createdFormsToNullify = _createdForms.Where(form => form.CreatorId == userToRemove.Id).ToList();
            foreach (var form in createdFormsToNullify)
            {
                form.CreatorId = null; // Set the CreatorId to null for forms created by this user
            }

            // set all completed forms' UserId to null
            var completedFormsToNullify = _completedForms.Where(form => form.UserId == userToRemove.Id).ToList();
            foreach (var completedForm in completedFormsToNullify)
            {
                completedForm.UserId = null;
            }

            _users.Remove(userToRemove);
        }

        public bool Exists(Guid id)
        {
            return _users.Any(user => user.Id == id);
        }

        public IList<UserEntity> GetAllFiltered(string name)
        {
            return _users.Where(u => u.Name.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}
