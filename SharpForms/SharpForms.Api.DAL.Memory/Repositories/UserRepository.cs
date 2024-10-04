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
            return _users.ToList();
        }

        public UserEntity? GetById(Guid id)
        {
            var userEntity = _users.SingleOrDefault(user => user.Id == id);

            if (userEntity != null)
            {
                userEntity.CompletedForms = _completedForms.Where(form => form.UserId == userEntity.Id).ToList();
                userEntity.CreatedForms = _createdForms.Where(form => form.CreatorId == userEntity.Id).ToList();
            }

            return userEntity;
        }

        public Guid Insert(UserEntity user)
        {
            _users.Add(user);
            return user.Id;
        }

        public Guid? Update(UserEntity user)
        {
            var existingUser = _users.SingleOrDefault(u => u.Id == user.Id);

            if (existingUser == null)
            {
                return null; // Null if not found
            }

            _mapper.Map(user, existingUser); // Update user
            return existingUser.Id;

        }

        public void Remove(Guid id)
        {
            var userToRemove = _users.SingleOrDefault(user => user.Id == id);
            if (userToRemove != null)
            {
                _users.Remove(userToRemove);
            }
        }

        public bool Exists(Guid id)
        {
            return _users.Any(user => user.Id == id);
        }
    }
}
