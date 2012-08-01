﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcFront.DB;
using MvcFront.Enums;

namespace MvcFront.Interfaces
{
    public interface IDocumentRepository
    {
        /// <summary>
        /// Возвращает список всех документов
        /// </summary>
        /// <returns></returns>
        IQueryable<Document> GetAll();

        /// <summary>
        /// Возвращает документ по его Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Document GetDocumentById(long id);

        /// <summary>
        /// Возвращает документ по его Id
        /// </summary>
        /// <param name="id">UserId</param>
        /// <param name="status"> </param>
        /// <returns></returns>
        IQueryable<Document> GetUserDocuments(long id, DocumentStatus? status = null);

        /// <summary>
        /// Сохраняет документ в базу
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Document SaveDocument(Document entity);

        /// <summary>
        /// Ставист документу статус удален
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Document DeleteDocument(long id);
        
        /// <summary>
        /// Меняет стутус документа на новый
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        void ChangeDocumentStatus(long id, DocumentStatus state);

        /// <summary>
        /// Создает документ из Формы документа
        /// </summary>
        /// <param name="docAppointmentlId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Document CreateDocumentFromDocAppointment(long docAppointmentlId, int userId);
    }
}
