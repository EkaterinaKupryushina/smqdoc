using System.Data.Objects;
using System.Linq;
using MvcFront.DB;
using MvcFront.Interfaces;

namespace MvcFront.Repositories
{
    public class UserTagRepository : IUserTagRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAccountRepository _userRepository;

        public UserTagRepository(IUnitOfWork unitOfWork, IUserAccountRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public bool SaveUserTag(UserTags tag)
        {
            if (tag.Id == 0)
            {
                tag.Status = (int)UserTagStatus.Active;

                _unitOfWork.DbModel.UserTags.AddObject(tag);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, tag);
            }
            else
            {
                var item = _unitOfWork.DbModel.UserTags.SingleOrDefault(x => x.Id == tag.Id);
                if (item != null)
                {
                    item.Name = tag.Name;
                    item.Status = (int)UserTagStatus.Active;

                    _unitOfWork.DbModel.SaveChanges();
                    _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, item);
                }
            }

            return true;
        }

        public void DeleteUserTag(int id)
        {
            var item = _unitOfWork.DbModel.UserTags.SingleOrDefault(x => x.Id == id);
            if (item != null)
            {
                item.Status = (int)UserTagStatus.Deleted;
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, item);
            }
        }

        public UserTags GetUserTagById(long id)
        {
            if (id == 0)
                return new UserTags();
            return _unitOfWork.DbModel.UserTags.SingleOrDefault(x => x.Id == id);
        }

        public IQueryable<UserTags> GetAllUserTags()
        {
            return _unitOfWork.DbModel.UserTags.AsQueryable();
        }

        private IQueryable<UserTags> GetUserTagsByUserID(long uid)
        {
            var user = _unitOfWork.DbModel.UserAccounts.First(x => x.userid == uid);

            return user.UserTags.AsQueryable();
        }

        public bool AddUserTag(int userId, int tagId)
        {
            var user = _userRepository.Copy(_unitOfWork, userId);
            var tag = GetUserTagById(tagId);

            var userTags = GetUserTagsByUserID(userId);

            if (!userTags.Any(x => x.Id == tagId))
            {
                user.UserTags.Add(tag);
                _unitOfWork.DbModel.SaveChanges();
            }

            return true;
        }

        public bool RemoveUserTag(int userId, int tagId)
        {
            var user = _userRepository.Copy(_unitOfWork, userId);
            var tag = GetUserTagById(tagId);

            var userTags = GetUserTagsByUserID(userId);

            if (userTags.Any(x => x.Id == tagId))
            {
                user.UserTags.Remove(tag);
                _unitOfWork.DbModel.SaveChanges();
            }
            return true;
        }  
    }
}