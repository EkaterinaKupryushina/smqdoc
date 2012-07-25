
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 07/25/2012 10:36:55
-- Generated from EDMX file: D:\Work\smqDoc\smqdoc.net\smqdoc\MvcFront\DB\EntityDBModel.edmx
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

IF OBJECT_ID(N'[dbo].[FK_ComputableFieldTemplatePartsFieldTemplate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ComputableFieldTemplateParts] DROP CONSTRAINT [FK_ComputableFieldTemplatePartsFieldTemplate];
GO
IF OBJECT_ID(N'[dbo].[FK_DocAppointmentGroupTemplate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocAppointments] DROP CONSTRAINT [FK_DocAppointmentGroupTemplate];
GO
IF OBJECT_ID(N'[dbo].[FK_DocFieldDocument]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocFields] DROP CONSTRAINT [FK_DocFieldDocument];
GO
IF OBJECT_ID(N'[dbo].[FK_DocFieldFieldTemplate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocFields] DROP CONSTRAINT [FK_DocFieldFieldTemplate];
GO
IF OBJECT_ID(N'[dbo].[FK_DocTemplateDocAppointment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocAppointments] DROP CONSTRAINT [FK_DocTemplateDocAppointment];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentDocAppointment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Documents] DROP CONSTRAINT [FK_DocumentDocAppointment];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentUserAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Documents] DROP CONSTRAINT [FK_DocumentUserAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_FieldTemplates_FieldTemplates1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FieldTemplates] DROP CONSTRAINT [FK_FieldTemplates_FieldTemplates1];
GO
IF OBJECT_ID(N'[dbo].[FK_FieldTeplateDocTemplate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FieldTemplates] DROP CONSTRAINT [FK_FieldTeplateDocTemplate];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupManager]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserGroups] DROP CONSTRAINT [FK_GroupManager];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupTemplateUserGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupTemplates] DROP CONSTRAINT [FK_GroupTemplateUserGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupUsers_UserAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupUsers] DROP CONSTRAINT [FK_GroupUsers_UserAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_GroupUsers_UserGroup]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GroupUsers] DROP CONSTRAINT [FK_GroupUsers_UserGroup];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAccountUserTags_UserAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAccountUserTags] DROP CONSTRAINT [FK_UserAccountUserTags_UserAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAccountUserTags_UserTags]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserAccountUserTags] DROP CONSTRAINT [FK_UserAccountUserTags_UserTags];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ComputableFieldTemplateParts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ComputableFieldTemplateParts];
GO
IF OBJECT_ID(N'[dbo].[DocAppointments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocAppointments];
GO
IF OBJECT_ID(N'[dbo].[DocFields]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocFields];
GO
IF OBJECT_ID(N'[dbo].[DocTemplates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocTemplates];
GO
IF OBJECT_ID(N'[dbo].[Documents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Documents];
GO
IF OBJECT_ID(N'[dbo].[FieldTemplates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FieldTemplates];
GO
IF OBJECT_ID(N'[dbo].[GroupTemplates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupTemplates];
GO
IF OBJECT_ID(N'[dbo].[GroupUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupUsers];
GO
IF OBJECT_ID(N'[dbo].[UserAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAccounts];
GO
IF OBJECT_ID(N'[dbo].[UserAccountUserTags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAccountUserTags];
GO
IF OBJECT_ID(N'[dbo].[UserGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserGroups];
GO
IF OBJECT_ID(N'[dbo].[UserTags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserTags];
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
    [OperationExpression] nvarchar(max)  NULL,
    [Integer] bit  NULL,
    [FactFieldTemplate_fieldteplateid] bigint  NULL
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
    [DocAppointment_docappointmentid] bigint  NOT NULL
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
    [PlanedStartDate] datetime  NULL,
    [PlanedEndDate] datetime  NULL,
    [UserGroup_usergroupid] int  NULL,
    [ActualStartDate] datetime  NOT NULL,
    [ActualEndDate] datetime  NOT NULL
);
GO

-- Creating table 'ComputableFieldTemplateParts'
CREATE TABLE [dbo].[ComputableFieldTemplateParts] (
    [computableFieldTemplatePartsID] bigint IDENTITY(1,1) NOT NULL,
    [fkCalculatedFieldTemplateID] bigint  NOT NULL,
    [FieldTemplate_fieldteplateid] bigint  NOT NULL,
    [OrderNumber] int  NOT NULL
);
GO

-- Creating table 'UserTags'
CREATE TABLE [dbo].[UserTags] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'DocAppointments'
CREATE TABLE [dbo].[DocAppointments] (
    [docappointmentid] bigint IDENTITY(1,1) NOT NULL,
    [DocTemplate_docteplateid] bigint  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Status] int  NOT NULL,
    [GroupTemplate_grouptemplateid] bigint  NOT NULL
);
GO

-- Creating table 'GroupUsers'
CREATE TABLE [dbo].[GroupUsers] (
    [MemberGroups_usergroupid] int  NOT NULL,
    [Members_userid] int  NOT NULL
);
GO

-- Creating table 'UserAccountUserTags'
CREATE TABLE [dbo].[UserAccountUserTags] (
    [UserAccounts_userid] int  NOT NULL,
    [UserTags_Id] int  NOT NULL
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

-- Creating primary key on [computableFieldTemplatePartsID] in table 'ComputableFieldTemplateParts'
ALTER TABLE [dbo].[ComputableFieldTemplateParts]
ADD CONSTRAINT [PK_ComputableFieldTemplateParts]
    PRIMARY KEY CLUSTERED ([computableFieldTemplatePartsID] ASC);
GO

-- Creating primary key on [Id] in table 'UserTags'
ALTER TABLE [dbo].[UserTags]
ADD CONSTRAINT [PK_UserTags]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [docappointmentid] in table 'DocAppointments'
ALTER TABLE [dbo].[DocAppointments]
ADD CONSTRAINT [PK_DocAppointments]
    PRIMARY KEY CLUSTERED ([docappointmentid] ASC);
GO

-- Creating primary key on [MemberGroups_usergroupid], [Members_userid] in table 'GroupUsers'
ALTER TABLE [dbo].[GroupUsers]
ADD CONSTRAINT [PK_GroupUsers]
    PRIMARY KEY NONCLUSTERED ([MemberGroups_usergroupid], [Members_userid] ASC);
GO

-- Creating primary key on [UserAccounts_userid], [UserTags_Id] in table 'UserAccountUserTags'
ALTER TABLE [dbo].[UserAccountUserTags]
ADD CONSTRAINT [PK_UserAccountUserTags]
    PRIMARY KEY NONCLUSTERED ([UserAccounts_userid], [UserTags_Id] ASC);
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

-- Creating foreign key on [FieldTemplate_fieldteplateid] in table 'ComputableFieldTemplateParts'
ALTER TABLE [dbo].[ComputableFieldTemplateParts]
ADD CONSTRAINT [FK_ComputableFieldTemplatePartsFieldTemplate]
    FOREIGN KEY ([FieldTemplate_fieldteplateid])
    REFERENCES [dbo].[FieldTemplates]
        ([fieldteplateid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ComputableFieldTemplatePartsFieldTemplate'
CREATE INDEX [IX_FK_ComputableFieldTemplatePartsFieldTemplate]
ON [dbo].[ComputableFieldTemplateParts]
    ([FieldTemplate_fieldteplateid]);
GO

-- Creating foreign key on [UserAccounts_userid] in table 'UserAccountUserTags'
ALTER TABLE [dbo].[UserAccountUserTags]
ADD CONSTRAINT [FK_UserAccountUserTags_UserAccount]
    FOREIGN KEY ([UserAccounts_userid])
    REFERENCES [dbo].[UserAccounts]
        ([userid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [UserTags_Id] in table 'UserAccountUserTags'
ALTER TABLE [dbo].[UserAccountUserTags]
ADD CONSTRAINT [FK_UserAccountUserTags_UserTags]
    FOREIGN KEY ([UserTags_Id])
    REFERENCES [dbo].[UserTags]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAccountUserTags_UserTags'
CREATE INDEX [IX_FK_UserAccountUserTags_UserTags]
ON [dbo].[UserAccountUserTags]
    ([UserTags_Id]);
GO

-- Creating foreign key on [DocTemplate_docteplateid] in table 'DocAppointments'
ALTER TABLE [dbo].[DocAppointments]
ADD CONSTRAINT [FK_DocTemplateDocAppointment]
    FOREIGN KEY ([DocTemplate_docteplateid])
    REFERENCES [dbo].[DocTemplates]
        ([docteplateid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocTemplateDocAppointment'
CREATE INDEX [IX_FK_DocTemplateDocAppointment]
ON [dbo].[DocAppointments]
    ([DocTemplate_docteplateid]);
GO

-- Creating foreign key on [DocAppointment_docappointmentid] in table 'Documents'
ALTER TABLE [dbo].[Documents]
ADD CONSTRAINT [FK_DocumentDocAppointment]
    FOREIGN KEY ([DocAppointment_docappointmentid])
    REFERENCES [dbo].[DocAppointments]
        ([docappointmentid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocumentDocAppointment'
CREATE INDEX [IX_FK_DocumentDocAppointment]
ON [dbo].[Documents]
    ([DocAppointment_docappointmentid]);
GO

-- Creating foreign key on [GroupTemplate_grouptemplateid] in table 'DocAppointments'
ALTER TABLE [dbo].[DocAppointments]
ADD CONSTRAINT [FK_DocAppointmentGroupTemplate]
    FOREIGN KEY ([GroupTemplate_grouptemplateid])
    REFERENCES [dbo].[GroupTemplates]
        ([grouptemplateid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocAppointmentGroupTemplate'
CREATE INDEX [IX_FK_DocAppointmentGroupTemplate]
ON [dbo].[DocAppointments]
    ([GroupTemplate_grouptemplateid]);
GO

-- Creating foreign key on [FactFieldTemplate_fieldteplateid] in table 'FieldTemplates'
ALTER TABLE [dbo].[FieldTemplates]
ADD CONSTRAINT [FK_FieldTemplates_FieldTemplates1]
    FOREIGN KEY ([FactFieldTemplate_fieldteplateid])
    REFERENCES [dbo].[FieldTemplates]
        ([fieldteplateid])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_FieldTemplates_FieldTemplates1'
CREATE INDEX [IX_FK_FieldTemplates_FieldTemplates1]
ON [dbo].[FieldTemplates]
    ([FactFieldTemplate_fieldteplateid]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------