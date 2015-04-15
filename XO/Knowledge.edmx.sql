
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/15/2015 17:58:31
-- Generated from EDMX file: C:\Users\lidor\documents\visual studio 2013\Projects\XO\XO\Knowledge.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Memory];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'States'
CREATE TABLE [dbo].[States] (
    [Hash] int  NOT NULL,
    [Score] float  NOT NULL,
    [Explored] bit  NOT NULL
);
GO

-- Creating table 'Actions'
CREATE TABLE [dbo].[Actions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Hash] float  NOT NULL,
    [Score] float  NOT NULL,
    [Explored] bit  NOT NULL
);
GO

-- Creating table 'StateAction'
CREATE TABLE [dbo].[StateAction] (
    [PrevState_Hash] int  NOT NULL,
    [Actions_Id] int  NOT NULL
);
GO

-- Creating table 'ActionState'
CREATE TABLE [dbo].[ActionState] (
    [Causes_Id] int  NOT NULL,
    [NextState_Hash] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Hash] in table 'States'
ALTER TABLE [dbo].[States]
ADD CONSTRAINT [PK_States]
    PRIMARY KEY CLUSTERED ([Hash] ASC);
GO

-- Creating primary key on [Id] in table 'Actions'
ALTER TABLE [dbo].[Actions]
ADD CONSTRAINT [PK_Actions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [PrevState_Hash], [Actions_Id] in table 'StateAction'
ALTER TABLE [dbo].[StateAction]
ADD CONSTRAINT [PK_StateAction]
    PRIMARY KEY CLUSTERED ([PrevState_Hash], [Actions_Id] ASC);
GO

-- Creating primary key on [Causes_Id], [NextState_Hash] in table 'ActionState'
ALTER TABLE [dbo].[ActionState]
ADD CONSTRAINT [PK_ActionState]
    PRIMARY KEY CLUSTERED ([Causes_Id], [NextState_Hash] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PrevState_Hash] in table 'StateAction'
ALTER TABLE [dbo].[StateAction]
ADD CONSTRAINT [FK_StateAction_State]
    FOREIGN KEY ([PrevState_Hash])
    REFERENCES [dbo].[States]
        ([Hash])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Actions_Id] in table 'StateAction'
ALTER TABLE [dbo].[StateAction]
ADD CONSTRAINT [FK_StateAction_Action]
    FOREIGN KEY ([Actions_Id])
    REFERENCES [dbo].[Actions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StateAction_Action'
CREATE INDEX [IX_FK_StateAction_Action]
ON [dbo].[StateAction]
    ([Actions_Id]);
GO

-- Creating foreign key on [Causes_Id] in table 'ActionState'
ALTER TABLE [dbo].[ActionState]
ADD CONSTRAINT [FK_ActionState_Action]
    FOREIGN KEY ([Causes_Id])
    REFERENCES [dbo].[Actions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [NextState_Hash] in table 'ActionState'
ALTER TABLE [dbo].[ActionState]
ADD CONSTRAINT [FK_ActionState_State]
    FOREIGN KEY ([NextState_Hash])
    REFERENCES [dbo].[States]
        ([Hash])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ActionState_State'
CREATE INDEX [IX_FK_ActionState_State]
ON [dbo].[ActionState]
    ([NextState_Hash]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------