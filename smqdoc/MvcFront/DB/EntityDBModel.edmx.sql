
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 03/25/2012 11:33:21
-- Generated from EDMX file: G:\Works\smqDocNet\smqdoc\MvcFront\DB\EntityDBModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [smqdoc];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_GroupManager]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserGroups] DROP CONSTRAINT [FK_GroupManager];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupUsers_UserGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupUsers] DROP CONSTRAINT [FK_GroupUsers_UserGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupUsers_UserAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupUsers] DROP CONSTRAINT [FK_GroupUsers_UserAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_FieldTeplateDocTemplate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FieldTemplates] DROP CONSTRAINT [FK_FieldTeplateDocTemplate];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentUserAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Documents] DROP CONSTRAINT [FK_DocumentUserAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_DocFieldDocument]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocFields] DROP CONSTRAINT [FK_DocFieldDocument];
GO
IF OBJECT_ID(N'[dbo].[FK_DocFieldFieldTemplate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocFields] DROP CONSTRAINT [FK_DocFieldFieldTemplate];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupTemplateUserGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupTemplates] DROP CONSTRAINT [FK_GroupTemplateUserGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupTemplateDocTemplate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupTemplates] DROP CONSTRAINT [FK_GroupTemplateDocTemplate];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentGroupTemplate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Documents] DROP CONSTRAINT [FK_DocumentGroupTemplate];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAccounts];
GO
IF OBJECT_ID(N'[dbo].[UserGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserGroups];
GO
IF OBJECT_ID(N'[dbo].[DocTemplates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocTemplates];
GO
IF OBJECT_ID(N'[dbo].[FieldTemplates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FieldTemplates];
GO
IF OBJECT_ID(N'[dbo].[Documents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Documents];
GO
IF OBJECT_ID(N'[dbo].[DocFields]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocFields];
GO
IF OBJECT_ID(N'[dbo].[GroupTemplates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupTemplates];
GO
IF OBJECT_ID(N'[dbo].[GroupUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupUsers];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'UserAccounts'
CREATE TABLE [dbo].[UserAccounts] (
    [userid] int IDENTITY(1,1) NOT NULL,
    [Login] nvarchar(50)  NOT NULL,
    [FirstName] nvarchar(50)  NOT NULL,
    [SecondName] nvarchar(50)  NOT NULL,
    [LastName] nvarchar(50)  NOT NULL,
    [Status] int  NOT NULL,
    [LastAccess] datetime  NULL,
    [IsAdmin] bit  NOT NULL,
    [Password] nvarchar(50)  NOT NULL,
    [Email] nvarchar(50)  NOT NULL,
    [LastAccessProfileCode] nvarchar(max)  NULL
);
GO

-- Creating table 'UserGroups'
CREATE TABLE [dbo].[UserGroups] (
    [usergroupid] int IDENTITY(1,1) NOT NULL,
    [FullGroupName] nvarchar(100)  NOT NULL,
    [GroupName] nvarchar(50)  NOT NULL,
    [Managerid] int  NOT NULL,
    [Status] int  NOT NULL
);
GO

-- Creating table 'DocTemplates'
CREATE TABLE [dbo].[DocTemplates] (
    [docteplateid] bigint IDENTITY(1,1) NOT NULL,
    [TemplateName] nvarchar(max)  NOT NULL,
    [Status] int  NOT NULL,
    [Comment] nvarchar(max)  NOT NULL,
    [LastEditDate] datetime  NOT NULL
);
GO

-- Creating table 'FieldTemplates'
CREATE TABLE [dbo].[FieldTemplates] (
    [fieldteplateid] bigint IDENTITY(1,1) NOT NULL,
    [FieldName] nvarchar(max)  NOT NULL,
    [FiledType] int  NOT NULL,
    [Restricted] bit  NULL,
    [MaxVal] float  NULL,
    [MinVal] float  NULL,
    [DocTemplate_docteplateid] bigint  NOT NULL,
    [OrderNumber] int  NOT NULL,
    [Status] int  NOT NULL
);
GO

-- Creating table 'Documents'
CREATE TABLE [dbo].[Documents] (
    [documentid] bigint IDENTITY(1,1) NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [LastEditDate] datetime  NOT NULL,
    [Status] int  NOT NULL,
    [LastComment] nvarchar(max)  NULL,
    [UserAccount_userid] int  NOT NULL,
    [GroupTemplate_grouptemplateid] bigint  NOT NULL
);
GO

-- Creating table 'DocFields'
CREATE TABLE [dbo].[DocFields] (
    [docfieldid] bigint IDENTITY(1,1) NOT NULL,
    [Document_documentid] bigint  NOT NULL,
    [FieldTemplate_fieldteplateid] bigint  NOT NULL,
    [StringValue] nvarchar(max)  NULL,
    [BoolValue] bit  NULL,
    [DoubleValue] float  NULL
);
GO

-- Creating table 'GroupTemplates'
CREATE TABLE [dbo].[GroupTemplates] (
    [grouptemplateid] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [DateStart] datetime  NOT NULL,
    [DateEnd] datetime  NOT NULL,
    [Status] int  NOT NULL,
    [UserGroup_usergroupid] int  NOT NULL,
    [DocTemplate_docteplateid] bigint  NOT NULL
);
GO

-- Creating table 'GroupUsers'
CREATE TABLE [dbo].[GroupUsers] (
    [MemberGroups_usergroupid] int  NOT NULL,
    [Members_userid] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [userid] in table 'UserAccounts'
ALTER TABLE [dbo].[UserAccounts]
ADD CONSTRAINT [PK_UserAccounts]
    PRIMARY KEY CLUSTERED ([userid] ASC);
GO

-- Creating primary key on [usergroupid] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups]
ADD CONSTRAINT [PK_UserGroups]
    PRIMARY KEY CLUSTERED ([usergroupid] ASC);
GO

-- Creating primary key on [docteplateid] in table 'DocTemplates'
ALTER TABLE [dbo].[DocTemplates]
ADD CONSTRAINT [PK_DocTemplates]
    PRIMARY KEY CLUSTERED ([docteplateid] ASC);
GO

-- Creating primary key on [fieldteplateid] in table 'FieldTemplates'
ALTER TABLE [dbo].[FieldTemplates]
ADD CONSTRAINT [PK_FieldTemplates]
    PRIMARY KEY CLUSTERED ([fieldteplateid] ASC);
GO

-- Creating primary key on [documentid] in table 'Documents'
ALTER TABLE [dbo].[Documents]
ADD CONSTRAINT [PK_Documents]
    PRIMARY KEY CLUSTERED ([documentid] ASC);
GO

-- Creating primary key on [docfieldid] in table 'DocFields'
ALTER TABLE [dbo].[DocFields]
ADD CONSTRAINT [PK_DocFields]
    PRIMARY KEY CLUSTERED ([docfieldid] ASC);
GO

-- Creating primary key on [grouptemplateid] in table 'GroupTemplates'
ALTER TABLE [dbo].[GroupTemplates]
ADD CONSTRAINT [PK_GroupTemplates]
    PRIMARY KEY CLUSTERED ([grouptemplateid] ASC);
GO

-- Creating primary key on [MemberGroups_usergroupid], [Members_userid] in table 'GroupUsers'
ALTER TABLE [dbo].[GroupUsers]
ADD CONSTRAINT [PK_GroupUsers]
    PRIMARY KEY NONCLUSTERED ([MemberGroups_usergroupid], [Members_userid] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Managerid] in table 'UserGroups'
ALTER TABLE [dbo].[UserGroups]
ADD CONSTRAINT [FK_GroupManager]
    FOREIGN KEY ([Managerid])
    REFERENCES [dbo].[UserAccounts]
        ([userid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupManager'
CREATE INDEX [IX_FK_GroupManager]
ON [dbo].[UserGroups]
    ([Managerid]);
GO

-- Creating foreign key on [MemberGroups_usergroupid] in table 'GroupUsers'
ALTER TABLE [dbo].[GroupUsers]
ADD CONSTRAINT [FK_GroupUsers_UserGroup]
    FOREIGN KEY ([MemberGroups_usergroupid])
    REFERENCES [dbo].[UserGroups]
        ([usergroupid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Members_userid] in table 'GroupUsers'
ALTER TABLE [dbo].[GroupUsers]
ADD CONSTRAINT [FK_GroupUsers_UserAccount]
    FOREIGN KEY ([Members_userid])
    REFERENCES [dbo].[UserAccounts]
        ([userid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupUsers_UserAccount'
CREATE INDEX [IX_FK_GroupUsers_UserAccount]
ON [dbo].[GroupUsers]
    ([Members_userid]);
GO

-- Creating foreign key on [DocTemplate_docteplateid] in table 'FieldTemplates'
ALTER TABLE [dbo].[FieldTemplates]
ADD CONSTRAINT [FK_FieldTeplateDocTemplate]
    FOREIGN KEY ([DocTemplate_docteplateid])
    REFERENCES [dbo].[DocTemplates]
        ([docteplateid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FieldTeplateDocTemplate'
CREATE INDEX [IX_FK_FieldTeplateDocTemplate]
ON [dbo].[FieldTemplates]
    ([DocTemplate_docteplateid]);
GO

-- Creating foreign key on [UserAccount_userid] in table 'Documents'
ALTER TABLE [dbo].[Documents]
ADD CONSTRAINT [FK_DocumentUserAccount]
    FOREIGN KEY ([UserAccount_userid])
    REFERENCES [dbo].[UserAccounts]
        ([userid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocumentUserAccount'
CREATE INDEX [IX_FK_DocumentUserAccount]
ON [dbo].[Documents]
    ([UserAccount_userid]);
GO

-- Creating foreign key on [Document_documentid] in table 'DocFields'
ALTER TABLE [dbo].[DocFields]
ADD CONSTRAINT [FK_DocFieldDocument]
    FOREIGN KEY ([Document_documentid])
    REFERENCES [dbo].[Documents]
        ([documentid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocFieldDocument'
CREATE INDEX [IX_FK_DocFieldDocument]
ON [dbo].[DocFields]
    ([Document_documentid]);
GO

-- Creating foreign key on [FieldTemplate_fieldteplateid] in table 'DocFields'
ALTER TABLE [dbo].[DocFields]
ADD CONSTRAINT [FK_DocFieldFieldTemplate]
    FOREIGN KEY ([FieldTemplate_fieldteplateid])
    REFERENCES [dbo].[FieldTemplates]
        ([fieldteplateid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocFieldFieldTemplate'
CREATE INDEX [IX_FK_DocFieldFieldTemplate]
ON [dbo].[DocFields]
    ([FieldTemplate_fieldteplateid]);
GO

-- Creating foreign key on [UserGroup_usergroupid] in table 'GroupTemplates'
ALTER TABLE [dbo].[GroupTemplates]
ADD CONSTRAINT [FK_GroupTemplateUserGroup]
    FOREIGN KEY ([UserGroup_usergroupid])
    REFERENCES [dbo].[UserGroups]
        ([usergroupid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupTemplateUserGroup'
CREATE INDEX [IX_FK_GroupTemplateUserGroup]
ON [dbo].[GroupTemplates]
    ([UserGroup_usergroupid]);
GO

-- Creating foreign key on [DocTemplate_docteplateid] in table 'GroupTemplates'
ALTER TABLE [dbo].[GroupTemplates]
ADD CONSTRAINT [FK_GroupTemplateDocTemplate]
    FOREIGN KEY ([DocTemplate_docteplateid])
    REFERENCES [dbo].[DocTemplates]
        ([docteplateid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GroupTemplateDocTemplate'
CREATE INDEX [IX_FK_GroupTemplateDocTemplate]
ON [dbo].[GroupTemplates]
    ([DocTemplate_docteplateid]);
GO

-- Creating foreign key on [GroupTemplate_grouptemplateid] in table 'Documents'
ALTER TABLE [dbo].[Documents]
ADD CONSTRAINT [FK_DocumentGroupTemplate]
    FOREIGN KEY ([GroupTemplate_grouptemplateid])
    REFERENCES [dbo].[GroupTemplates]
        ([grouptemplateid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocumentGroupTemplate'
CREATE INDEX [IX_FK_DocumentGroupTemplate]
ON [dbo].[Documents]
    ([GroupTemplate_grouptemplateid]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------