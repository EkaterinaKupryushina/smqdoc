﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcFront.DB;

namespace MvcFront.Interfaces
{
    public interface IDocumentRepository
    {
        IQueryable<Document> GetAll();
        Document GetDocumentById(long id);

        Document SaveDocument(Document entity);
        Document DeleteDocument(long id);
        Document ChangeDocumentStatus(long id, DocumentStatus state);
        Document CreateDocumentFromGroupDocument(long groupTemplId,int userId);
    }
}
