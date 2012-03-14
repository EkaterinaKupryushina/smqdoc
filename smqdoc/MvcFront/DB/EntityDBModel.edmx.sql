
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 03/14/2012 09:59:25
-- Generated from EDMX file: D:\Work\smqdoc.net\MvcFront\DB\EntityDBModel.edmx
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

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[UserAccounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserAccounts];
GO
IF OBJECT_ID(N'[dbo].[UserGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserGroups];
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
    [Email] nvarchar(50)  NOT NULL
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

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------