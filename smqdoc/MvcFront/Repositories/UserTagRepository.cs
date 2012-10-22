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
        private readonly IDocReportRepository _docReportRepository;

        public UserTagRepository(IUnitOfWork unitOfWork, IUserAccountRepository userRepository, IDocReportRepository docReportRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _docReportRepository = docReportRepository;
        }

        public bool SaveUserTag(UserTags tag)
        {
            if (tag.Id == 0)
            {
                _unitOfWork.DbModel.UserTags.AddObject(tag);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, tag);
            }
            else
            {
                _unitOfWork.DbModel.UserTags.ApplyCurrentValues(tag);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, tag);
            }

            return true;
        }

        public void DeleteUserTag(int id)
        {
            var item = _unitOfWork.DbModel.UserTags.SingleOrDefault(x => x.Id == id);
            if (item != null)
            {
                _unitOfWork.DbModel.UserTags.DeleteObject(item);
                _unitOfWork.DbModel.SaveChanges();
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

        private IQueryable<UserTags> GetUserTagsByUserId(long uid)
        {
            var user = _unitOfWork.DbModel.UserAccounts.First(x => x.userid == uid);

            return user.UserTags.AsQueryable();
        }

        private IQueryable<UserTags> GetUserTagsByDocReportId(int docreportId)
        {
            var docReport = _unitOfWork.DbModel.DocReports.First(x => x.docreportid == docreportId);

            return docReport.UserTags.AsQueryable();
        }

        public bool AddUserTagToUser(int userId, int tagId)
        {
            var user = _userRepository.Copy(_unitOfWork, userId);
            var tag = GetUserTagById(tagId);

            var userTags = GetUserTagsByUserId(userId);

            if (!userTags.Any(x => x.Id == tagId))
            {
                user.UserTags.Add(tag);
                _unitOfWork.DbModel.SaveChanges();
            }

            return true;
        }

        public bool RemoveUserTagFromUser(int userId, int tagId)
        {
            var user = _userRepository.Copy(_unitOfWork, userId);
            var tag = GetUserTagById(tagId);

            var userTags = GetUserTagsByUserId(userId);

            if (userTags.Any(x => x.Id == tagId))
            {
                user.UserTags.Remove(tag);
                _unitOfWork.DbModel.SaveChanges();
            }
            return true;
        }

        public bool AddUserTagToDocReport(int docreportId, int tagId)
        {
            var docReport = _docReportRepository.Copy(_unitOfWork, docreportId);
            var tag = GetUserTagById(tagId);

            var userTags = GetUserTagsByDocReportId(docreportId);

            if (!userTags.Any(x => x.Id == tagId))
            {
                docReport.UserTags.Add(tag);
                _unitOfWork.DbModel.SaveChanges();
            }

            return true;
        }

        public bool RemoveUserTagFromDocReport(int docreportId, int tagId)
        {
            var docReport = _docReportRepository.Copy(_unitOfWork, docreportId);
            var tag = GetUserTagById(tagId);

            var userTags = GetUserTagsByDocReportId(docreportId);

            if (userTags.Any(x => x.Id == tagId))
            {
                docReport.UserTags.Remove(tag);
                _unitOfWork.DbModel.SaveChanges();
            }
            return true;
        }
    }
}