---Заполняем пользователей
INSERT INTO [smqdoc].[dbo].[UserAccounts]
           ([Login]
           ,[FirstName]
           ,[SecondName]
           ,[LastName]
           ,[Status]
           ,[LastAccess]
           ,[IsAdmin]
           ,[Password])
     VALUES
           ('Pavel','Павел','Сердюков','Алексеевич',0,NULL,1,'Pavel'),
           ('Dima','Дмитрий','Хаустов','Геннадьевич',0,NULL,1,'Dima'),
           ('TestUser1','TestUser1F','TestUser1S','TestUser1L',0,NULL,1,'TestUser1'),
           ('TestUser2','TestUser2F','TestUser2S','TestUser2L',0,NULL,0,'TestUser2'),
           ('TestUser3','TestUser3F','TestUser3S','TestUser3L',1,NULL,0,'TestUser3'),
           ('TestUser4','TestUser4F','TestUser4S','TestUser4L',2,NULL,0,'TestUser4')
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

----заполняем таблицу меток пользователей
INSERT INTO [smqdoc].[dbo].[UserTags]
           ([Name]
           )
     VALUES
           ('Параллель 5 классов'),
           ('Учителя математики'),
           ('Учителя физики'),
           ('Параллель 10 классов'),
           ('Учителя русского языка')
GO