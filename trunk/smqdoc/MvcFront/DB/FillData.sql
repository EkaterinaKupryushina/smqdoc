---Заполняем пользователей
INSERT INTO [smqdoc].[dbo].[UserAccounts]
           ([Login]
           ,[FirstName]
           ,[SecondName]
           ,[LastName]
           ,[Status]
           ,[LastAccess]
           ,[IsAdmin]
           ,[Password]
           ,[Email])
     VALUES
           ('Pavel','Павел','Сердюков','Алексеевич',0,NULL,1,'Pavel','p@mail.ru'),
           ('Dima','Дмитрий','Хаустов','Геннадьевич',0,NULL,1,'Dima','d@mail.ru'),
           ('TestUser1','TestUser1F','TestUser1S','TestUser1L',0,NULL,1,'TestUser1','TestUser1@mail.ru'),
           ('TestUser2','TestUser2F','TestUser2S','TestUser2L',0,NULL,0,'TestUser2','TestUser2@mail.ru'),
           ('TestUser3','TestUser3F','TestUser3S','TestUser3L',1,NULL,0,'TestUser3','TestUser3@mail.ru'),
           ('TestUser4','TestUser4F','TestUser4S','TestUser4L',2,NULL,0,'TestUser4','TestUser4@mail.ru')
GO

---Заполняем группы
INSERT INTO [smqdoc].[dbo].[UserGroups]
           ([FullGroupName]
           ,[GroupName]
           ,[Managerid]
           ,[Status])
     VALUES
           ('Полное имя группы 1','Группа 1',1,0),
           ('Полное имя группы 2','Группа 2',2,0),
           ('Полное имя группы 3','Группа 3',1,1),
           ('Полное имя группы 4','Группа 1',2,2),
           ('Полное имя группы 5','Группа 5',1,0)
GO

---заполняем связи группа - пользователь
INSERT INTO [smqdoc].[dbo].[GroupUsers]
           ([MemberGroups_usergroupid]
           ,[Members_userid])
     VALUES
           (1,1),(1,2),(1,3),(1,4),(1,5),(2,1),(2,2),(2,4),(3,1),(3,2),(3,3),(3,4),(3,5),(4,1),(4,3),(4,4),(4,5)
GO

---заполняем шаблоны документов
INSERT INTO [smqdoc].[dbo].[DocTemplates]
           ([TemplateName]
           ,[Status]
           ,[Comment]
           ,[LastEditDate])
     VALUES
           ( 'Шаблон номер 1',0,'Коммент к Шаблон номер 1','2012-03-15 00:00:00.000'),
		   ( 'Шаблон номер 2',1,'','2012-02-15 00:00:00.000'),
		   ( 'Шаблон номер 3',2,'Комент Шаблон номер 3','2011-03-15 00:00:00.000'),
		   ( 'Шаблон номер 4',0,'Шаблон номер 4 комммент длинный длинный','2012-03-25 00:00:00.000'),
		   ( 'Шаблон номер 5',0,'Совсе длинный коммент','2012-03-02 00:00:00.000')
GO

---заполняем шаблоны полей документов
INSERT INTO [smqdoc].[dbo].[FieldTemplates]
           ([FieldName]
           ,[FiledType]
           ,[Restricted]
           ,[MaxVal]
           ,[MinVal]
           ,[DocTemplate_docteplateid]
           ,[OrderNumber]
           ,[Status])
     VALUES
          ('Поле номер шаблон номер 1',0,0,NULL,NULL,1,1,0),
		   ('Поле номер шаблон номер 2',1,1,10,1,1,2,2),
		   ('Поле номер шаблон номер 3',2,0,NULL,NULL,1,3,0),
		   ('Поле номер шаблон номер 4',1,0,NULL,NULL,1,4,1),
		   ('Поле номер шаблон номер 5',1,0,NULL,NULL,1,5,0),
		   ('Поле номер шаблон номер 6',1,0,NULL,NULL,1,6,0),
		   ('Поле номер шаблон номер 7',1,0,NULL,NULL,1,7,0),
		   ('Поле номер шаблон номер 8',0,0,NULL,NULL,2,1,0),
		   ('Поле номер шаблон номер 9',0,0,NULL,NULL,2,2,0),
		   ('Поле номер шаблон номер 10',0,0,NULL,NULL,2,3,0),
		   ('Поле номер шаблон номер 11',2,0,NULL,NULL,3,1,0),
		   ('Поле номер шаблон номер 12',2,0,NULL,NULL,4,1,0),
		   ('Поле номер шаблон номер 13',1,0,NULL,NULL,4,2,0),
		   ('Поле номер шаблон номер 14',1,0,NULL,NULL,4,3,0),
		   ('Поле номер шаблон номер 15',1,0,NULL,NULL,4,4,0),
		   ('Поле номер шаблон номер 16',1,0,NULL,NULL,4,5,0),
		   ('Поле номер шаблон номер 17',0,0,NULL,NULL,4,6,0)
GO

----заполняем таблицу документы
INSERT INTO [smqdoc].[dbo].[Documents]
           ([CreationDate]
           ,[LastEditDate]
           ,[Status]
           ,[LastComment]
           ,[DocTemplate_docteplateid]
           ,[DocumentName]
           ,[UserAccount_userid])
     VALUES
           ('2011-01-01','2011-01-01',0,'Comment',1,'Тестовый документ11',1),
           ('2012-01-01','2012-01-01',0,null,1,'Тестовый документ21',1),
           ('2012-01-01','2012-01-01',1,null,1,'Тестовый документ31',1),
           ('2011-01-01','2011-01-01',0,'Comment',1,'Тестовый документ12',2),
           ('2012-01-01','2012-01-01',0,null,1,'Тестовый документ22',2),
           ('2012-01-01','2012-01-01',1,null,1,'Тестовый документ32',2)           
GO
----заполняем поля документа
INSERT INTO [smqdoc].[dbo].[DocFields]
           ([Document_documentid]
           ,[FieldTemplate_fieldteplateid]
           ,[StringValue]
           ,[BoolValue]
           ,[DoubleValue])
     VALUES
           (1,1,NULL,1,NULL),(1,2,'Значение',NULL,NULL),(1,3,NULL,NULL,3),(1,4,NULL,NULL,4),(1,5,NULL,NULL,5),(1,6,NULL,NULL,6),
			(2,1,NULL,1,NULL),(2,2,'Значение',NULL,NULL),(2,3,NULL,NULL,3),(2,4,NULL,NULL,4),(2,5,NULL,NULL,5),(2,6,NULL,NULL,6),
			(3,1,NULL,1,NULL),(3,2,'Значение',NULL,NULL),(3,3,NULL,NULL,3),(3,4,NULL,NULL,4),(3,5,NULL,NULL,5),(3,6,NULL,NULL,6),
			(4,1,NULL,1,NULL),(4,2,'Значение',NULL,NULL),(4,3,NULL,NULL,3),(4,4,NULL,NULL,4),(4,5,NULL,NULL,5),(4,6,NULL,NULL,6),
			(5,1,NULL,1,NULL),(5,2,'Значение',NULL,NULL),(5,3,NULL,NULL,3),(5,4,NULL,NULL,4),(5,5,NULL,NULL,5),(5,6,NULL,NULL,6),
			(6,1,NULL,1,NULL),(6,2,'Значение',NULL,NULL),(6,3,NULL,NULL,3),(6,4,NULL,NULL,4),(6,5,NULL,NULL,5),(6,6,NULL,NULL,6)
     GO

	 --Добавляем связи шаблнов с группами
INSERT INTO [smqdoc].[dbo].[GroupTemplates]
           ([Name]
           ,[DateStart]
           ,[DateEnd]
           ,[Status]
           ,[UserGroup_usergroupid]
           ,[DocTemplate_docteplateid])
     VALUES
           ('Связь между первым шаблоном и группой',GETDATE(),GETDATE(),0,1,1),
           ('Связь между вторыми шаблоном и группой',GETDATE(),GETDATE(),0,2,2),
           ('Связь между третьими шаблоном и группой',GETDATE(),GETDATE(),0,3,3),
           ('Связь между четвертыми шаблоном и группой',GETDATE(),GETDATE(),0,4,4),
           ('Связь между пятыми шаблоном и группой',GETDATE(),GETDATE(),0,5,5)