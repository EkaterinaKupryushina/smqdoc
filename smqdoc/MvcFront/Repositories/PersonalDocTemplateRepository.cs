using System.Data.Objects;
using System.Linq;
using MvcFront.DB;
using MvcFront.Interfaces;

namespace MvcFront.Repositories
{
    public class PersonalDocTemplateRepository : IPersonalDocTemplateRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonalDocTemplateRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IQueryable<PersonalDocTemplate> GetAllGroupPersonalDocTemplates(int groupId)
        {
            return _unitOfWork.DbModel.PersonalDocTemplates.Where(x => x.UserGroup_usergroupid == groupId);
        }

        public PersonalDocTemplate GetPersonalDocTemplateById(int id)
        {
            if (id == 0)
                return new PersonalDocTemplate();
            return _unitOfWork.DbModel.PersonalDocTemplates.SingleOrDefault(x => x.personaldoctemplateid == id);
        }

        public void SavePersonalDocTemplate(PersonalDocTemplate entity)
        {
            if (entity.personaldoctemplateid == 0)
            {
                _unitOfWork.DbModel.PersonalDocTemplates.AddObject(entity);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, entity);
            }
            else
            {
                _unitOfWork.DbModel.PersonalDocTemplates.ApplyCurrentValues(entity);
                _unitOfWork.DbModel.SaveChanges();
                _unitOfWork.DbModel.Refresh(RefreshMode.StoreWins, entity);
            }
        }

        public void DeletePersonalDocTemplate(int id)
        {
            var asset = GetPersonalDocTemplateById(id);
            _unitOfWork.DbModel.PersonalDocTemplates.DeleteObject(asset);
            _unitOfWork.DbModel.SaveChanges();
        }
    }
}